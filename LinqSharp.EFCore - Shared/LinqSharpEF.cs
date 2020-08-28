// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#if EFCore2
using Microsoft.EntityFrameworkCore.Query.Expressions;
#else
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
#endif
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NStandard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace LinqSharp.EFCore
{
    public static partial class LinqSharpEF
    {
        private const string SaveChangesName = "Int32 SaveChanges(Boolean)";
        private const string SaveChangesAsyncName = "System.Threading.Tasks.Task`1[System.Int32] SaveChangesAsync(Boolean, System.Threading.CancellationToken)";

        public static int HandleConcurrencyExceptionRetryCount = 1;
        public static CacheContainer<Type, Dictionary<ConflictWin, string[]>> ConcurrencyWins = new CacheContainer<Type, Dictionary<ConflictWin, string[]>>()
        {
            CacheMethod = type => () =>
            {
                var storeWins = new List<string>();
                var clientWins = new List<string>();
                var combines = new List<string>();
                var throws = new List<string>();

                foreach (var prop in type.GetProperties())
                {
                    var attr = prop.GetCustomAttribute<ConcurrencyPolicyAttribute>();
                    if (attr != null)
                    {
                        switch (attr.ConflictWin)
                        {
                            case ConflictWin.Store: storeWins.Add(prop.Name); break;
                            case ConflictWin.Client: storeWins.Add(prop.Name); break;
                            case ConflictWin.Combine: combines.Add(prop.Name); break;
                        }
                    }
                    else
                    {
                        throws.Add(prop.Name);
                    }
                }

                var dict = new Dictionary<ConflictWin, string[]>
                {
                    [ConflictWin.Store] = storeWins.ToArray(),
                    [ConflictWin.Client] = clientWins.ToArray(),
                    [ConflictWin.Combine] = combines.ToArray(),
                    [ConflictWin.Throw] = throws.ToArray(),
                };
                return dict;
            }
        };

        public static ValueConverter<TModel, TProvider> BuildConverter<TModel, TProvider>(IProvider<TModel, TProvider> field)
        {
            return new ValueConverter<TModel, TProvider>(v => field.WriteToProvider(v), v => field.ReadFromProvider(v));
        }

        public static void OnModelCreating(DbContext context, Action<ModelBuilder> baseOnModelCreating, ModelBuilder modelBuilder)
        {
            ApplyProviderFunctions(context, modelBuilder);
            ApplyUdFunctions(context, modelBuilder);
            ApplyAnnotations(context, modelBuilder);
            baseOnModelCreating(modelBuilder);
        }

        private static TRet HandleConcurrencyException<TRet>(DbUpdateConcurrencyException ex, Func<TRet> saveChanges)
        {
            for (int retry = 0; retry < HandleConcurrencyExceptionRetryCount; retry++)
            {
                try
                {
                    var entries = ex.Entries;
                    foreach (var entry in entries)
                    {
                        var storeValues = entry.GetDatabaseValues();
                        var originalValues = entry.OriginalValues.Clone();

                        //entry.OriginalValues.SetValues(storeValues);

                        var entityType = entry.Entity.GetType();
                        var wins = ConcurrencyWins[entityType].Value;

                        foreach (var propName in wins[ConflictWin.Store])
                        {
                            entry.OriginalValues[propName] = storeValues[propName];
                            entry.Property(propName).IsModified = false;
                        }

                        foreach (var propName in wins[ConflictWin.Combine])
                        {
                            if (!Equals(originalValues[propName], storeValues[propName]))
                            {
                                entry.OriginalValues[propName] = storeValues[propName];
                                entry.Property(propName).IsModified = false;
                            }
                        }
                    }

                    return saveChanges();
                }
                catch (DbUpdateConcurrencyException _ex) { ex = _ex; }
                catch (Exception _ex) { throw _ex; }
            }

            throw ex;
        }

        public static int SaveChanges(DbContext context, Func<bool, int> baseSaveChanges, bool acceptAllChangesOnSuccess)
        {
            IntelliTrack(context, acceptAllChangesOnSuccess);
            return baseSaveChanges(acceptAllChangesOnSuccess);
            //try
            //{
            //    return baseSaveChanges(acceptAllChangesOnSuccess);
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    return HandleConcurrencyException(ex, () => baseSaveChanges(acceptAllChangesOnSuccess));
            //}
        }

        public static Task<int> SaveChangesAsync(DbContext context, Func<bool, CancellationToken, Task<int>> baseSaveChangesAsync, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            IntelliTrack(context, acceptAllChangesOnSuccess);
            return baseSaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            //try
            //{
            //    return baseSaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            //}
            //catch (DbUpdateConcurrencyException ex)
            //{
            //    return HandleConcurrencyException(ex, () => baseSaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken));
            //}
        }

        public static void ApplyProviderFunctions(DbContext context, ModelBuilder modelBuilder)
        {
            var providerName = context.GetProviderName();
            switch (providerName)
            {
                case DatabaseProviderName.Cosmos:
                case DatabaseProviderName.Firebird:
                case DatabaseProviderName.IBM:
                case DatabaseProviderName.OpenEdge:
                default: throw new NotSupportedException();

                case DatabaseProviderName.Jet: JetFuncProvider.Register(modelBuilder); break;

                case DatabaseProviderName.MyCat:
                case DatabaseProviderName.MySql: MySqlFuncProvider.Register(modelBuilder); break;

                case DatabaseProviderName.Oracle: OracleFuncProvider.Register(modelBuilder); break;

                case DatabaseProviderName.PostgreSQL: PostgreSQLFuncProvider.Register(modelBuilder); break;

                case DatabaseProviderName.Sqlite: SqliteFuncProvider.Register(modelBuilder); break;

                case DatabaseProviderName.SqlServer:
                case DatabaseProviderName.SqlServerCompact35:
                case DatabaseProviderName.SqlServerCompact40: SqlServerFuncProvider.Register(modelBuilder); break;

            }
        }

        public static void ApplyUdFunctions(DbContext context, ModelBuilder modelBuilder)
        {
            var providerName = context.GetProviderName();

            var types = Assembly.GetEntryAssembly().GetTypesWhichImplements<IUdFunctionContainer>();
            var methods = types.SelectMany(type => type.GetMethods().Where(x => x.GetCustomAttribute<UdFunctionAttribute>()?.ProviderName == providerName));
            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<UdFunctionAttribute>();
                modelBuilder.HasDbFunction(method, x =>
                {
                    x.HasName(attr.Name);
                    x.HasSchema(attr.Schema);
                });
            }
        }

        public static void ApplyAnnotations(DbContext context, ModelBuilder modelBuilder, LinqSharpAnnotation annotation = LinqSharpAnnotation.All)
        {
            var entityMethod = modelBuilder.GetType()
                .GetMethodViaQualifiedName("Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder`1[TEntity] Entity[TEntity]()");
            var dbSetProps = context.GetType().GetProperties()
                .Where(x => x.ToString().StartsWith("Microsoft.EntityFrameworkCore.DbSet`1"));

            foreach (var dbSetProp in dbSetProps)
            {
                var modelClass = dbSetProp.PropertyType.GenericTypeArguments[0];
                var entityMethod1 = entityMethod.MakeGenericMethod(modelClass);
                var entityTypeBuilder = entityMethod1.Invoke(modelBuilder, new object[0]);

                if ((annotation & LinqSharpAnnotation.Index) == LinqSharpAnnotation.Index)
                    ApplyIndexes(entityTypeBuilder, modelClass);
                if ((annotation & LinqSharpAnnotation.Provider) == LinqSharpAnnotation.Provider)
                    ApplyProviders(entityTypeBuilder, modelClass);
                if ((annotation & LinqSharpAnnotation.CompositeKey) == LinqSharpAnnotation.CompositeKey)
                    ApplyCompositeKey(entityTypeBuilder, modelClass);
            }
        }

        /// <summary>
        /// This method should be called before 'base.SaveChanges'.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <returns></returns>
        public static void IntelliTrack(DbContext context, bool acceptAllChangesOnSuccess)
        {
            EntityEntry[] entries;
            IGrouping<Type, EntityEntry>[] auditEntriesByTypes;
            void RefreshEntries()
            {
                entries = context.ChangeTracker.Entries()
                   .Where(x => new[] { EntityState.Added, EntityState.Modified, EntityState.Deleted }.Contains(x.State))
                   .ToArray();
                auditEntriesByTypes = entries
                    .GroupBy(x => x.Entity.GetType())
                    .Where(x => x.Key.HasAttribute<EntityAuditAttribute>())
                    .ToArray();
            }

            var auditorCaches = new CacheContainer<Type, Reflector>
            {
                CacheMethod = auditType => () => Activator.CreateInstance(auditType).GetReflector(),
            };

            RefreshEntries();
            foreach (var entry in entries)
            {
                // Resolve AutoAttributes
                var entity = entry.Entity;
                var entityType = entity.GetType();
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    var props = entityType.GetProperties().Where(x => x.CanWrite).ToArray();
                    ResolveAutoAttributes(entry, props);
                }
            }

            // Resolve BeforeAudit
            foreach (var entriesByType in auditEntriesByTypes)
            {
                var entityType = entriesByType.Key;
                var attr = entityType.GetCustomAttribute<EntityAuditAttribute>();

                var auditType = typeof(EntityAudit<>).MakeGenericType(entityType);
                var audits = Array.CreateInstance(auditType, entriesByType.Count());
                foreach (var kv in entriesByType.AsKvPairs())
                    audits.SetValue(EntityAudit.Parse(kv.Value), kv.Key);

                auditorCaches[attr.EntityAuditorType].Value
                    .DeclaredMethod(nameof(IEntityAuditor<DbContext, object>.BeforeAudit)).Call(context, audits);
            }

            // Resolve OnAuditing
            RefreshEntries();
            foreach (var entriesByType in auditEntriesByTypes)
            {
                var entityType = entriesByType.Key;
                var attr = entityType.GetCustomAttribute<EntityAuditAttribute>();

                var auditType = typeof(EntityAudit<>).MakeGenericType(entityType);
                var audits = Array.CreateInstance(auditType, entriesByType.Count());
                foreach (var kv in entriesByType.AsKvPairs())
                    audits.SetValue(EntityAudit.Parse(kv.Value), kv.Key);

                auditorCaches[attr.EntityAuditorType].Value
                    .DeclaredMethod(nameof(IEntityAuditor<DbContext, object>.OnAuditing)).Call(context, audits);
            }

            // Resolve OnAudited
            CompleteAudit(context);
        }

        private static void CompleteAudit(DbContext context)
        {
            var entries = context.ChangeTracker.Entries()
                .Where(x => new[] { EntityState.Added, EntityState.Modified, EntityState.Deleted }.Contains(x.State))
                .ToArray();
            var predictor = new AuditPredictor();

            var auditEntriesByTypes = entries
                .GroupBy(x => x.Entity.GetType())
                .Where(x => x.Key.HasAttribute<EntityAuditAttribute>())
                .ToArray();

            // Complete EntityAudit
            foreach (var entriesByType in auditEntriesByTypes)
            {
                var attr = entriesByType.Key.GetCustomAttribute<EntityAuditAttribute>();
                foreach (var kv in entriesByType.AsKvPairs())
                    predictor.Add(EntityAudit.Parse(kv.Value));
            }

            foreach (var entriesByType in auditEntriesByTypes)
            {
                var entityType = entriesByType.Key;
                var attr = entityType.GetCustomAttribute<EntityAuditAttribute>();
                var auditor = Activator.CreateInstance(attr.EntityAuditorType);
                auditor.GetReflector().DeclaredMethod(nameof(IEntityAuditor<DbContext, object>.OnAudited)).Call(context, predictor);
            }
        }

        private static void ApplyCompositeKey(object entityTypeBuilder, Type modelClass)
        {
            var hasKeyMethod = entityTypeBuilder.GetType().GetMethod(nameof(EntityTypeBuilder.HasKey), new[] { typeof(string[]) });

            var modelProps = modelClass.GetProperties()
                .Where(x => x.GetCustomAttribute<CPKeyAttribute>() != null)
                .OrderBy(x => x.GetCustomAttribute<CPKeyAttribute>().Order);
            var propNames = modelProps.Select(x => x.Name).ToArray();

            if (propNames.Any())
                hasKeyMethod.Invoke(entityTypeBuilder, new object[] { propNames });
        }

        private static void ApplyIndexes(object entityTypeBuilder, Type modelClass)
        {
            var hasIndexMethod = entityTypeBuilder.GetType().GetMethod(nameof(EntityTypeBuilder.HasIndex), new[] { typeof(string[]) });
            void setIndex(string[] propertyNames, bool unique, bool isForeignKey)
            {
                if (propertyNames.Length > 0)
                {
                    var indexBuilder = hasIndexMethod.Invoke(entityTypeBuilder, new object[] { propertyNames }) as IndexBuilder;
                    if (unique) indexBuilder.IsUnique();

                    // Because of some unknown BUG in EntityFramework, creating an index causes the first normal index to be dropped, which is defined with ForeignKeyAttribute.
                    // (The problem was found in EntityFrameworkCore 2.2.6)
                    //TODO: Here is the temporary solution
                    if (isForeignKey) setIndex(new[] { propertyNames[0] }, false, false);
                }
            }

            var props = modelClass.GetProperties().Select(prop => new
            {
                Index = prop.GetCustomAttribute<IndexAttribute>(),
                IsForeignKey = prop.GetCustomAttribute<ForeignKeyAttribute>() != null,
                prop.Name,
            }).Where(x => x.Index != null);

            foreach (var prop in props.Where(x => x.Index.Group == null))
            {
                switch (prop.Index.Type)
                {
                    case IndexType.Normal: setIndex(new[] { prop.Name }, false, prop.IsForeignKey); break;
                    case IndexType.Unique: setIndex(new[] { prop.Name }, true, prop.IsForeignKey); break;
                }
            }
            foreach (var group in props.Where(x => x.Index.Group != null).GroupBy(x => new { x.Index.Type, x.Index.Group }))
            {
                switch (group.Key.Type)
                {
                    case IndexType.Normal: setIndex(group.Select(x => x.Name).ToArray(), false, group.First().IsForeignKey); break;
                    case IndexType.Unique: setIndex(group.Select(x => x.Name).ToArray(), true, group.First().IsForeignKey); break;
                }
            }
        }

        private static void ApplyProviders(object entityTypeBuilder, Type modelClass)
        {
            var propertyMethod = entityTypeBuilder.GetType().GetMethodViaQualifiedName("Microsoft.EntityFrameworkCore.Metadata.Builders.PropertyBuilder Property(System.String)");

            var modelProps = modelClass.GetProperties();
            foreach (var modelProp in modelProps)
            {
                var attr = modelProp.GetCustomAttribute<ProviderAttribute>();
                if (attr != null)
                {
                    var propertyBuilder = propertyMethod.Invoke(entityTypeBuilder, new object[] { modelProp.Name }) as PropertyBuilder;
                    var hasConversionMethod = typeof(PropertyBuilder).GetMethod(nameof(PropertyBuilder.HasConversion), new[] { typeof(ValueConverter) });

                    dynamic provider = Activator.CreateInstance(attr.ProviderType);
                    hasConversionMethod.Invoke(propertyBuilder, new object[] { LinqSharpEF.BuildConverter(provider) });
                }
            }
        }

        private static void ResolveAutoAttributes(EntityEntry entry, PropertyInfo[] properties)
        {
            var props_AutoCreationTime = properties.Where(x => x.HasAttribute<AutoCreationTimeAttribute>());
            var props_AutoLastWrite = properties.Where(x => x.HasAttribute<AutoLastWriteTimeAttribute>());
            var props_AutoLower = properties.Where(x => x.HasAttribute<AutoLowerAttribute>());
            var props_AutoUpper = properties.Where(x => x.HasAttribute<AutoUpperAttribute>());
            var props_AutoTrim = properties.Where(x => x.HasAttribute<AutoTrimAttribute>());
            var props_AutoCondensed = properties.Where(x => x.HasAttribute<AutoCondensedAttribute>());

            var now = DateTime.Now;
            if (entry.State == EntityState.Added)
            {
                SetPropertiesValue(props_AutoCreationTime, entry, v => now);
            }

            if (new[] { EntityState.Added, EntityState.Modified }.Contains(entry.State))
            {
                SetPropertiesValue(props_AutoLastWrite, entry, v => now);
                SetPropertiesValue(props_AutoLower, entry, v => (v as string)?.ToLower());
                SetPropertiesValue(props_AutoUpper, entry, v => (v as string)?.ToUpper());
                SetPropertiesValue(props_AutoTrim, entry, v => (v as string)?.Trim());
                SetPropertiesValue(props_AutoCondensed, entry, v => ((v as string) ?? "").Unique());
            }
        }

        private static void SetPropertiesValue(IEnumerable<PropertyInfo> properties, EntityEntry entry, Func<object, object> evalMethod)
        {
            foreach (var prop in properties)
            {
                var oldValue = prop.GetValue(entry.Entity);
                prop.SetValue(entry.Entity, evalMethod(oldValue));
            }
        }

    }
}
