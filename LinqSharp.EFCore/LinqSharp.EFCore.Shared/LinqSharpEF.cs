﻿// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Annotations;
using LinqSharp.EFCore.Design;
using LinqSharp.EFCore.Annotations.Params;
using LinqSharp.EFCore.Translators;
using LinqSharp.EFCore.Infrastructure;
using LinqSharp.EFCore.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
#if EFCORE3_1_OR_GREATER
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
#else
using Microsoft.EntityFrameworkCore.Query.Expressions;
#endif
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Caching.Memory;
using NStandard;
using NStandard.Caching;
using NStandard.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using LinqSharp.EFCore.Query;

namespace LinqSharp.EFCore;

public static partial class LinqSharpEF
{
    private static readonly MemoryCache ComplexTypesCache = new(new MemoryCacheOptions());
    private static readonly MemoryCache ConcurrencyResolvingCache = new(new MemoryCacheOptions());
    private static readonly MemoryCache ConcurrencyResolvableEntityCache = new(new MemoryCacheOptions());

    private static InvalidOperationException CanNotUpdate(Type entityType, string id) => new($"Record is locked by client. (#{id})");

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
        UseTranslator<DbRandom>(context, modelBuilder);
        UseAnnotations(context, modelBuilder);
        UseComplexTypes(context, modelBuilder);
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
        int @return;

        IntelliTrack(context, acceptAllChangesOnSuccess);

        if (context.Database is IFacade) (context.Database as IFacade).UpdateState();

        if (context is IConcurrencyResolvableContext resolvable)
        {
            try
            {
                @return = base_SaveChanges(acceptAllChangesOnSuccess);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                @return = HandleConcurrencyException(ex, () => base_SaveChanges(acceptAllChangesOnSuccess), resolvable.MaxConcurrencyRetry);
            }
        }
        else @return = base_SaveChanges(acceptAllChangesOnSuccess);

        if (context.Database is IFacade facade && context.Database.CurrentTransaction is null && facade.EnableWithoutTransaction)
        {
            facade.Trigger_OnCommitted();
        }

        return @return;
    }

    public static async Task<int> SaveChangesAsync(DbContext context, Func<bool, CancellationToken, Task<int>> base_SaveChangesAsync, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        Task<int> @return;

        IntelliTrack(context, acceptAllChangesOnSuccess);

        if (context.Database is IFacade) (context.Database as IFacade).UpdateState();

        if (context is IConcurrencyResolvableContext resolvable)
        {
            try
            {
                @return = base_SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                @return = HandleConcurrencyException(ex, () => base_SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken), resolvable.MaxConcurrencyRetry);
            }
        }
        else @return = base_SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        var result = await @return;

        if (context.Database is IFacade facade && facade.EnableWithoutTransaction && context.Database.CurrentTransaction is null)
        {
            facade.Trigger_OnCommitted();
        }

        return result;
    }

    public static void UseTranslator<TDbFuncProvider>(DbContext context, ModelBuilder modelBuilder) where TDbFuncProvider : Translator, new()
    {
        var providerName = context.GetProviderName();
        var provider = new TDbFuncProvider();
        provider.RegisterAll(providerName, modelBuilder);
    }

    public static void UseAnnotations(DbContext context, ModelBuilder modelBuilder, EntityAnnotation annotation = EntityAnnotation.All)
    {
        var entityMethod = modelBuilder.GetType().GetMethodViaQualifiedName("Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder`1[TEntity] Entity[TEntity]()");
        var dbSetProps = context.GetType().GetProperties().Where(x => x.ToString().StartsWith("Microsoft.EntityFrameworkCore.DbSet`1"));

        foreach (var dbSetProp in dbSetProps)
        {
            var modelClass = dbSetProp.PropertyType.GenericTypeArguments[0];
            var entityMethod1 = entityMethod.MakeGenericMethod(modelClass);
            var entityTypeBuilder = entityMethod1.Invoke(modelBuilder, new object[0]);

            if (annotation.HasFlag(EntityAnnotation.Index)) ApplyIndexes(entityTypeBuilder, modelClass);
            if (annotation.HasFlag(EntityAnnotation.Provider)) ApplyProviders(entityTypeBuilder, modelClass);
            if (annotation.HasFlag(EntityAnnotation.CompositeKey)) ApplyCompositeKey(entityTypeBuilder, modelClass);
        }
    }

    public static void UseComplexTypes(DbContext context, ModelBuilder modelBuilder)
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
                    var type = field.PropertyType.Pipe(p => p.IsNullable() ? p.GetGenericArguments()[0] : p);
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
        IGrouping<Type, EntityEntry>[] entriesByType;
        IGrouping<Type, EntityEntry>[] auditEntriesByType;

        void RefreshEntries()
        {
            entries =
            [
                .. from x in context.ChangeTracker.Entries()
                   where new[] { EntityState.Added, EntityState.Modified, EntityState.Deleted }.Contains(x.State)
                   select x
            ];

            entriesByType =
            [
                .. from x in entries
                   group x by x.Entity.GetType() into g
                   select g
            ];

            auditEntriesByType =
            [
                .. from g in entriesByType
                   where g.Key.HasAttribute<EntityAuditAttribute>()
                   select g
            ];
        }

        var auditorCaches = new CacheSet<Type, Reflector>(auditType => () => Activator.CreateInstance(auditType).GetReflector());

        RefreshEntries();
        foreach (var entriesOfType in entriesByType)
        {
            foreach (var entry in entriesOfType)
            {
                if (new[] { EntityState.Added, EntityState.Modified, EntityState.Deleted }.Contains(entry.State))
                {
                    ResolveAutoAttributes(context, entry, entriesOfType.Key);
                }
            }
        }

        // Resolve BeforeAudit
        foreach (var entriesOfType in auditEntriesByType)
        {
            var entityType = entriesOfType.Key;
            var attr = entityType.GetCustomAttribute<EntityAuditAttribute>();

            var auditType = typeof(EntityAudit<>).MakeGenericType(entityType);
            var audits = Array.CreateInstance(auditType, entriesOfType.Count());
            foreach (var (index, value) in entriesOfType.AsIndexValuePairs())
            {
                audits.SetValue(EntityAudit.Parse(value), index);
            }

            auditorCaches[attr.EntityAuditorType].Value.DeclaredMethod(nameof(IEntityAuditor<DbContext, object>.BeforeAudit)).Call(context, audits);
        }

        // Resolve OnAuditing
        RefreshEntries();
        foreach (var entriesOfType in auditEntriesByType)
        {
            var entityType = entriesOfType.Key;
            var attr = entityType.GetCustomAttribute<EntityAuditAttribute>();

            var auditType = typeof(EntityAudit<>).MakeGenericType(entityType);
            var audits = Array.CreateInstance(auditType, entriesOfType.Count());
            foreach (var (index, value) in entriesOfType.AsIndexValuePairs())
            {
                audits.SetValue(EntityAudit.Parse(value), index);
            }

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
            foreach (var entry in entriesByType)
            {
                predictor.Add(EntityAudit.Parse(entry));
            }
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
        var hasKeyMethod = entityTypeBuilder.GetType().GetMethod(nameof(EntityTypeBuilder.HasKey), [typeof(string[])]);

        var modelProps = modelClass.GetProperties()
            .Where(x => x.GetCustomAttribute<CPKeyAttribute>() is not null)
            .OrderBy(x => x.GetCustomAttribute<CPKeyAttribute>().Order);
        var propNames = modelProps.Select(x => x.Name).ToArray();

        if (propNames.Any())
            hasKeyMethod.Invoke(entityTypeBuilder, [propNames]);
    }

    private static void ApplyIndexes(object entityTypeBuilder, Type modelClass)
    {
        var hasIndexMethod = entityTypeBuilder.GetType().GetMethod(nameof(EntityTypeBuilder.HasIndex), [typeof(string[])]);
        void SetIndex(string[] propertyNames, IndexType type)
        {
            if (propertyNames.Length == 0) throw new ArgumentException("No property specified.", nameof(propertyNames));

            var indexBuilder = hasIndexMethod.Invoke(entityTypeBuilder, [propertyNames]) as IndexBuilder;
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

        foreach (var prop in props.Where(x => x.Index.Group is null))
        {
            SetIndex([prop.Name], prop.Index.Type);
        }
        foreach (var group in props.Where(x => x.Index.Group is not null).GroupBy(x => new { x.Index.Type, x.Index.Group }))
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
            var providerAttr = modelProp.GetCustomAttributes().FirstOrDefault(x => x.GetType().IsExtend(typeof(ProviderAttribute<,>), true));

            if (providerAttr is null)
            {
                var specialProviderAttr = modelProp.GetCustomAttributes().FirstOrDefault(x => x.GetType().IsExtend(typeof(SpecialProviderAttribute), true));
                if (specialProviderAttr is null) continue;

                providerAttr = (specialProviderAttr as SpecialProviderAttribute).GetTargetProvider(modelProp);
            }

            if (providerAttr is not null)
            {
                var propertyBuilder = propertyMethod.Invoke(entityTypeBuilder, [modelProp.Name]) as PropertyBuilder;
                var hasConversionMethod = typeof(PropertyBuilder).GetMethod(nameof(PropertyBuilder.HasConversion), [typeof(ValueConverter)]);

                var providerAttrType = providerAttr.GetType();
                dynamic provider = Activator.CreateInstance(providerAttrType);

                var converter = LinqSharpEF.BuildConverter(provider);
                hasConversionMethod.Invoke(propertyBuilder, [converter]);

                var comparer = LinqSharpEF.BuildComparer(provider);
                if (comparer is not null)
                {
                    var metadataProperty = typeof(PropertyBuilder).GetProperty(nameof(PropertyBuilder.Metadata));
                    var metadata = metadataProperty.GetValue(propertyBuilder);
#if EFCORE6_0_OR_GREATER
                    var setValueComparerMethod = typeof(IMutableProperty).GetMethod(nameof(IMutableProperty.SetValueComparer), new[] { typeof(ValueComparer) });
                    setValueComparerMethod.Invoke(metadata, new object[] { comparer });
#else
                    var setValueComparerMethod = typeof(MutablePropertyExtensions).GetMethod(nameof(MutablePropertyExtensions.SetValueComparer));
                    setValueComparerMethod.Invoke(null, [metadata, comparer]);
#endif
                }
            }
        }
    }

    private static void ResolveAutoAttributes(DbContext context, EntityEntry entry, Type entityType)
    {
        var props = entityType.GetProperties().Where(x => x.CanWrite).ToArray();

        var nowTag = new NowParam
        {
            Now = DateTime.Now,
            NowOffset = DateTimeOffset.Now,
        };

        var isUserTraceable = context is IUserTraceable;
        var currentUser = new Lazy<string>(() =>
        {
            return (context as IUserTraceable).CurrentUser;
        });

        var isRowLockable = context is IRowLockable;
        var ignoreRowLock = new Lazy<bool>(() =>
        {
            return (context as IRowLockable).IgnoreRowLock;
        });

        var originValues = entry.GetDatabaseValues();
        foreach (var prop in props)
        {
            var propertyType = prop.PropertyType;
            var currentValue = prop.GetValue(entry.Entity);
            var finalValue = currentValue;
            var attrs = prop.GetCustomAttributes<AutoAttribute>();

            foreach (var attr in attrs)
            {
                if (attr is ISpecialAutoAttribute)
                {
                    if (!attr.States.Contains((AutoState)entry.State))
                    {
                        if (entry.State == EntityState.Modified)
                        {
                            finalValue = originValues[prop.Name];
                        }
                        continue;
                    }

                    if (attr is SpecialAutoAttribute<NowParam> attr_now)
                    {
                        finalValue = attr_now.Format(entry.Entity, propertyType, nowTag);
                    }
                    else if (attr is SpecialAutoAttribute<UserParam> attr_user)
                    {
                        if (!isUserTraceable) throw new InvalidOperationException($"The context needs to implement {nameof(IUserTraceable)}.");

                        finalValue = attr_user.Format(entry.Entity, propertyType, new UserParam
                        {
                            CurrentUser = currentUser.Value,
                        });
                    }
                    else if (attr is SpecialAutoAttribute<LockParam> attr_lock)
                    {
                        if (!isRowLockable) throw new InvalidOperationException($"The context needs to implement {nameof(IRowLockable)}.");

                        finalValue = attr_lock.Format(entry.Entity, propertyType, new LockParam
                        {
                            IgnoreRowLock = ignoreRowLock.Value,
                            Origin = originValues[prop.Name],
                            Current = currentValue,
                        });
                    }
                    else throw new NotImplementedException($"{attr.GetType()} was not processed.");
                }
                else
                {
                    if (!attr.States.Contains((AutoState)entry.State)) continue;

                    finalValue = attr.Format(entry.Entity, propertyType, currentValue);
                }
            }

            if (currentValue != finalValue)
            {
                prop.SetValue(entry.Entity, finalValue);
            }
        }
    }

}
