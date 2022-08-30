// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Functions.Providers;
using LinqSharp.EFCore.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#if EFCORE3_0_OR_GREATER
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
#else
using Microsoft.EntityFrameworkCore.Query.Expressions;
#endif
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Caching.Memory;
using NStandard;
using NStandard.Caching;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace LinqSharp.EFCore
{
    public static partial class LinqSharpEF
    {
        private static readonly MemoryCache ComplexTypesCache = new(new MemoryCacheOptions());
        private static readonly MemoryCache ConcurrencyResolvingCache = new(new MemoryCacheOptions());
        private static readonly MemoryCache ConcurrencyResolvableEntityCache = new(new MemoryCacheOptions());

        public static Dictionary<ConcurrencyResolvingMode, string[]> GetConcurrencyResolvingDict(Type type)
        {
            return ConcurrencyResolvingCache.GetOrCreate(type, entry =>
            {
                var databaseWinList = new List<string>();
                var checkList = new List<string>();

                foreach (var prop in type.GetProperties())
                {
                    var timestampAttr = prop.GetCustomAttribute<TimestampAttribute>();
                    if (timestampAttr is not null)
                    {
                        checkList.Add(prop.Name);
                        continue;
                    }

                    var concurrencyCheckAttr = prop.GetCustomAttribute<ConcurrencyCheckAttribute>();
                    if (concurrencyCheckAttr is not null)
                    {
                        checkList.Add(prop.Name);
                        continue;
                    }

                    var policyAttr = prop.GetCustomAttribute<ConcurrencyPolicyAttribute>();
                    if (policyAttr is not null)
                    {
                        if (policyAttr.Mode == ConcurrencyResolvingMode.DatabaseWins)
                        {
                            databaseWinList.Add(prop.Name);
                        }
                    }
                }

                var dict = new Dictionary<ConcurrencyResolvingMode, string[]>();
                if (databaseWinList.Any()) dict.Add(ConcurrencyResolvingMode.DatabaseWins, databaseWinList.ToArray());
                if (checkList.Any()) dict.Add(ConcurrencyResolvingMode.Check, checkList.ToArray());

                if (dict.Keys.Any()) return dict;
                else return null;
            });
        }

        public static ValueConverter<TModel, TProvider> BuildConverter<TModel, TProvider>(ProviderAttribute<TModel, TProvider> field)
        {
            return new ValueConverter<TModel, TProvider>(v => field.WriteToProvider(v), v => field.ReadFromProvider(v));
        }

        public static ValueComparer<TModel> BuildComparer<TModel, TProvider>(ProviderAttribute<TModel, TProvider> field)
        {
            return field.GetValueComparer();
        }

        public static void OnModelCreating(DbContext context, ModelBuilder modelBuilder)
        {
            ApplyProviderFunctions(context, modelBuilder);
            ApplyUdFunctions(context, modelBuilder);
            ApplyAnnotations(context, modelBuilder);
            ApplyComplexTypes(context, modelBuilder);
        }

        private static TRet HandleConcurrencyException<TRet>(DbUpdateConcurrencyException exception, Func<TRet> base_SaveChanges, int maxRetry)
        {
            var entries = exception.Entries;
            var resolvablesByTypeGroups = entries.GroupBy(x => x.Entity.GetType()).Where(entriesByType =>
            {
                var entityType = entriesByType.Key;
                var isResolvable = ConcurrencyResolvableEntityCache.GetOrCreate(entityType, entry =>
                {
                    return entityType.GetCustomAttribute<ConcurrencyResolvableAttribute>() is not null;
                });
                return isResolvable;
            });

            for (int retry = 0; retry < maxRetry; retry++)
            {
                foreach (var entriesByType in resolvablesByTypeGroups)
                {
                    var entityType = entriesByType.Key;
                    var resolvingDict = GetConcurrencyResolvingDict(entityType);
                    if (resolvingDict is null) goto throw_exception;

                    foreach (var entry in entriesByType)
                    {
                        var storeValues = entry.GetDatabaseValues();

                        foreach (var propName in resolvingDict[ConcurrencyResolvingMode.DatabaseWins])
                        {
                            entry.CurrentValues[propName] = storeValues[propName];
                        }

                        foreach (var propName in resolvingDict[ConcurrencyResolvingMode.Check])
                        {
                            entry.OriginalValues[propName] = storeValues[propName];
                        }
                    }
                }

                try { return base_SaveChanges(); }
                catch (DbUpdateConcurrencyException ex) { exception = ex; }
                catch (Exception) { throw; }
            }

        throw_exception:
            throw exception;
        }

        public static int SaveChanges(DbContext context, Func<bool, int> base_SaveChanges, bool acceptAllChangesOnSuccess)
        {
            IntelliTrack(context, acceptAllChangesOnSuccess);
            if (context is IConcurrencyResolvableContext resolvable)
            {
                try
                {
                    return base_SaveChanges(acceptAllChangesOnSuccess);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return HandleConcurrencyException(ex, () => base_SaveChanges(acceptAllChangesOnSuccess), resolvable.MaxConcurrencyRetry);
                }
            }
            else return base_SaveChanges(acceptAllChangesOnSuccess);
        }

        public static Task<int> SaveChangesAsync(DbContext context, Func<bool, CancellationToken, Task<int>> base_SaveChangesAsync, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            IntelliTrack(context, acceptAllChangesOnSuccess);
            if (context is IConcurrencyResolvableContext resolvable)
            {
                try
                {
                    return base_SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return HandleConcurrencyException(ex, () => base_SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken), resolvable.MaxConcurrencyRetry);
                }
            }
            else return base_SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public static void ApplyProviderFunctions(DbContext context, ModelBuilder modelBuilder)
        {
            var providerName = context.GetProviderName();
            switch (providerName)
            {
                case DatabaseProviderName.Jet: new JetFuncProvider(modelBuilder).UseAll(); break;

                case DatabaseProviderName.MyCat:
                case DatabaseProviderName.MySql: new MySqlFuncProvider(modelBuilder).UseAll(); break;

                case DatabaseProviderName.Oracle: new OracleFuncProvider(modelBuilder).UseAll(); break;

                case DatabaseProviderName.PostgreSQL: new PostgreSQLFuncProvider(modelBuilder).UseAll(); break;

                case DatabaseProviderName.Sqlite: new SqliteFuncProvider(modelBuilder).UseAll(); break;

                case DatabaseProviderName.SqlServer:
                case DatabaseProviderName.SqlServerCompact35:
                case DatabaseProviderName.SqlServerCompact40: new SqlServerFuncProvider(modelBuilder).UseAll(); break;

                case DatabaseProviderName.Cosmos:
                case DatabaseProviderName.Firebird:
                case DatabaseProviderName.IBM:
                case DatabaseProviderName.OpenEdge:
                default: throw new NotSupportedException();
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
            var entityMethod = modelBuilder.GetType().GetMethodViaQualifiedName("Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder`1[TEntity] Entity[TEntity]()");
            var dbSetProps = context.GetType().GetProperties().Where(x => x.ToString().StartsWith("Microsoft.EntityFrameworkCore.DbSet`1"));

            foreach (var dbSetProp in dbSetProps)
            {
                var modelClass = dbSetProp.PropertyType.GenericTypeArguments[0];
                var entityMethod1 = entityMethod.MakeGenericMethod(modelClass);
                var entityTypeBuilder = entityMethod1.Invoke(modelBuilder, new object[0]);

                if ((annotation & LinqSharpAnnotation.Index) == LinqSharpAnnotation.Index) ApplyIndexes(entityTypeBuilder, modelClass);
                if ((annotation & LinqSharpAnnotation.Provider) == LinqSharpAnnotation.Provider) ApplyProviders(entityTypeBuilder, modelClass);
                if ((annotation & LinqSharpAnnotation.CompositeKey) == LinqSharpAnnotation.CompositeKey) ApplyCompositeKey(entityTypeBuilder, modelClass);
            }
        }

        public static void ApplyComplexTypes(DbContext context, ModelBuilder modelBuilder)
        {
            var types = ComplexTypesCache.GetOrCreate(context.GetType().FullName, entry =>
            {
                var typeList = new HashSet<Type>();
                var dbSetTypes = context.GetType().GetProperties().Where(x => x.PropertyType.IsType(typeof(DbSet<>)));
                foreach (var dbSetType in dbSetTypes)
                {
                    var prop = dbSetType.PropertyType.GetGenericArguments()[0].GetProperties();
                    var fields = prop.Where(x => x.CanRead && x.CanWrite && !x.PropertyType.IsBasicType(true));
                    foreach (var field in fields)
                    {
                        var type = field.PropertyType.For(p => p.IsNullable() ? p.GetGenericArguments()[0] : p);
                        if (type.GetCustomAttributes().Any(x => x.GetType().FullName == "System.ComponentModel.DataAnnotations.Schema.ComplexTypeAttribute"))
                        {
                            typeList.Add(type);
                        }
                    }
                }

                return typeList.ToArray();
            });

            foreach (var type in types) modelBuilder.Owned(type);
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

            var auditorCaches = new CacheSet<Type, Reflector>(auditType => () => Activator.CreateInstance(auditType).GetReflector());

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
                foreach (var kv in entriesByType.AsKeyValuePairs())
                    audits.SetValue(EntityAudit.Parse(kv.Value), kv.Key);

                auditorCaches[attr.EntityAuditorType].Value.DeclaredMethod(nameof(IEntityAuditor<DbContext, object>.BeforeAudit)).Call(context, audits);
            }

            // Resolve OnAuditing
            RefreshEntries();
            foreach (var entriesByType in auditEntriesByTypes)
            {
                var entityType = entriesByType.Key;
                var attr = entityType.GetCustomAttribute<EntityAuditAttribute>();

                var auditType = typeof(EntityAudit<>).MakeGenericType(entityType);
                var audits = Array.CreateInstance(auditType, entriesByType.Count());
                foreach (var kv in entriesByType.AsKeyValuePairs())
                    audits.SetValue(EntityAudit.Parse(kv.Value), kv.Key);

                auditorCaches[attr.EntityAuditorType].Value.DeclaredMethod(nameof(IEntityAuditor<DbContext, object>.OnAuditing)).Call(context, audits);
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
                foreach (var kv in entriesByType.AsKeyValuePairs())
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
            void SetIndex(string[] propertyNames, IndexType type)
            {
                if (propertyNames.Length == 0) throw new ArgumentException("No property specified.", nameof(propertyNames));

                var indexBuilder = hasIndexMethod.Invoke(entityTypeBuilder, new object[] { propertyNames }) as IndexBuilder;
                if (type == IndexType.Unique) indexBuilder.IsUnique();
            }

            PropIndex[] props;
            {
                var list = new List<PropIndex>();
                var _props = modelClass.GetProperties();
                foreach (var prop in _props)
                {
                    var indexes = prop.GetCustomAttributes<IndexFieldAttribute>();
                    foreach (var index in indexes)
                    {
                        list.Add(new PropIndex
                        {
                            Index = index,
                            Name = prop.Name,
                        });
                    }

                    // Because of some unknown bugs in EntityFramework, creating an index causes the first normal index to be dropped, which is defined with ForeignKeyAttribute.
                    // (The problem was found in EntityFrameworkCore 2.2.6)
                    //TODO: Mitigation
                    if (prop.HasAttribute<ForeignKeyAttribute>() && !indexes.Any(x => x.Type == IndexType.Normal && x.Group is null))
                    {
                        list.Add(new PropIndex
                        {
                            Index = new IndexFieldAttribute(IndexType.Normal),
                            Name = prop.Name,
                        });
                    }
                }
                props = list.ToArray();
            }

            foreach (var prop in props.Where(x => x.Index.Group == null))
            {
                SetIndex(new[] { prop.Name }, prop.Index.Type);
            }
            foreach (var group in props.Where(x => x.Index.Group != null).GroupBy(x => new { x.Index.Type, x.Index.Group }))
            {
                SetIndex(group.Select(x => x.Name).ToArray(), group.Key.Type);
            }
        }

        private static void ApplyProviders(object entityTypeBuilder, Type modelClass)
        {
            //TODO: Optimizable
            var propertyMethod = entityTypeBuilder.GetType().GetMethodViaQualifiedName("Microsoft.EntityFrameworkCore.Metadata.Builders.PropertyBuilder Property(System.String)");

            var modelProps = modelClass.GetProperties();
            foreach (var modelProp in modelProps)
            {
                var providerAttr = modelProp.GetCustomAttributes().FirstOrDefault(x => x.GetType().IsExtend(typeof(ProviderAttribute<,>)));

                if (providerAttr is null)
                {
                    var specialProviderAttr = modelProp.GetCustomAttributes().FirstOrDefault(x => x.GetType().IsExtend(typeof(SpecialProviderAttribute)));
                    if (specialProviderAttr is null) continue;

                    providerAttr = (specialProviderAttr as SpecialProviderAttribute).GetTargetProvider(modelProp);
                }

                if (providerAttr is not null)
                {
                    var propertyBuilder = propertyMethod.Invoke(entityTypeBuilder, new object[] { modelProp.Name }) as PropertyBuilder;
                    var hasConversionMethod = typeof(PropertyBuilder).GetMethod(nameof(PropertyBuilder.HasConversion), new[] { typeof(ValueConverter) });
                    var providerAttrType = providerAttr.GetType();

                    dynamic provider = Activator.CreateInstance(providerAttrType);
                    var converter = LinqSharpEF.BuildConverter(provider);
                    hasConversionMethod.Invoke(propertyBuilder, new object[] { converter });

                    var comparer = LinqSharpEF.BuildComparer(provider);
                    if (comparer is not null)
                    {
                        var metadataProperty = typeof(PropertyBuilder).GetProperty(nameof(PropertyBuilder.Metadata));
                        var metadata = metadataProperty.GetValue(propertyBuilder);
#if EFCORE6_0_OR_GREATER
                        var setValueComparerMethod = typeof(MutablePropertyExtensions).GetMethod(nameof(MutablePropertyExtensions.SetStructuralValueComparer));
#else
                        var setValueComparerMethod = typeof(MutablePropertyExtensions).GetMethod(nameof(MutablePropertyExtensions.SetValueComparer));
#endif
                        setValueComparerMethod.Invoke(null, new object[] { metadata, comparer });
                    }
                }
            }
        }

        private static void ResolveAutoAttributes(EntityEntry entry, PropertyInfo[] properties)
        {
            if (!new[] { EntityState.Added, EntityState.Modified }.Contains(entry.State)) return;

            var now = DateTime.Now;
            var nowOffset = DateTimeOffset.Now;

            foreach (var prop in properties)
            {
                var propertyType = prop.PropertyType;
                var oldValue = prop.GetValue(entry.Entity);
                var finalValue = oldValue;
                var attrs = prop.GetCustomAttributes<AutoAttribute>();

                foreach (var attr in attrs)
                {
                    if (attr is AutoCreationTimeAttribute)
                    {
                        if (entry.State == EntityState.Added)
                        {
                            if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?)) { finalValue = now; break; }
                            else if (propertyType == typeof(DateTimeOffset) || propertyType == typeof(DateTimeOffset?)) { finalValue = nowOffset; break; }
                        }
                    }
                    else if (attr is AutoLastWriteTimeAttribute)
                    {
                        if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                        {
                            if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?)) { finalValue = now; break; }
                            else if (propertyType == typeof(DateTimeOffset) || propertyType == typeof(DateTimeOffset?)) { finalValue = nowOffset; break; }
                        }
                    }
                    else
                    {
                        if (attr.States.Contains(entry.State))
                        {
                            if (oldValue is null) finalValue = attr.Format(null);
                            else if (oldValue is string str) finalValue = attr.Format(str);
                            else throw new ArgumentException($"Can not resolve AutoAttribute for property({prop.Name}).");
                        }
                    }
                }

                if (oldValue != finalValue)
                    prop.SetValue(entry.Entity, finalValue);
            }
        }

    }
}
