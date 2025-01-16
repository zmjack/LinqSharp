// Copyright 2020 zmjack
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
using LinqSharp.EFCore.Scopes;

namespace LinqSharp.EFCore;

public static partial class LinqSharpEF
{
    private static readonly MemoryCache _complexTypesCache = new(new MemoryCacheOptions());
    private static readonly MemoryCache _fieldOptionScopeCache = new(new MemoryCacheOptions());
    private static readonly MemoryCache _concurrencyResolvingCache = new(new MemoryCacheOptions());
    private static readonly MemoryCache _concurrencyResolvableEntityCache = new(new MemoryCacheOptions());

    public static Dictionary<ConcurrencyResolvingMode, string[]>? GetConcurrencyResolvingDict(Type type)
    {
        return _concurrencyResolvingCache.GetOrCreate(type, entry =>
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

    public static ValueComparer<TModel>? BuildComparer<TModel, TProvider>(ProviderAttribute<TModel, TProvider> field)
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
            var isResolvable = _concurrencyResolvableEntityCache.GetOrCreate(entityType, entry =>
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
                        entry.CurrentValues[propName] = storeValues?[propName];
                    }

                    foreach (var propName in resolvingDict[ConcurrencyResolvingMode.Check])
                    {
                        entry.OriginalValues[propName] = storeValues?[propName];
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
        int ret;

        IntelliTrack(context, acceptAllChangesOnSuccess);
        (context.Database as IFacade)?.UpdateState();

        if (context is IConcurrencyResolvableContext resolvable)
        {
            try
            {
                ret = base_SaveChanges(acceptAllChangesOnSuccess);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ret = HandleConcurrencyException(ex, () => base_SaveChanges(acceptAllChangesOnSuccess), resolvable.MaxConcurrencyRetry);
            }
        }
        else ret = base_SaveChanges(acceptAllChangesOnSuccess);

        if (context.Database is IFacade facade && context.Database.CurrentTransaction is null && facade.EnableWithoutTransaction)
        {
            facade.Trigger_OnCommitted();
        }

        return ret;
    }

    public static async Task<int> SaveChangesAsync(DbContext context, Func<bool, CancellationToken, Task<int>> base_SaveChangesAsync, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        Task<int> ret;

        IntelliTrack(context, acceptAllChangesOnSuccess);
        (context.Database as IFacade)?.UpdateState();

        if (context is IConcurrencyResolvableContext resolvable)
        {
            try
            {
                ret = base_SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ret = HandleConcurrencyException(ex, () => base_SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken), resolvable.MaxConcurrencyRetry);
            }
        }
        else ret = base_SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        var result = await ret;

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
        var dbSetProps = context.GetType().GetProperties().Where(x => x.ToString()!.StartsWith("Microsoft.EntityFrameworkCore.DbSet`1"));

        foreach (var dbSetProp in dbSetProps)
        {
            var modelClass = dbSetProp.PropertyType.GenericTypeArguments[0];
            var entityMethod1 = entityMethod.MakeGenericMethod(modelClass);
            var entityTypeBuilder = entityMethod1.Invoke(modelBuilder, [])!;

            if (annotation.HasFlag(EntityAnnotation.Index)) ApplyIndexes(entityTypeBuilder, modelClass);
            if (annotation.HasFlag(EntityAnnotation.Provider)) ApplyProviders(entityTypeBuilder, modelClass);
            if (annotation.HasFlag(EntityAnnotation.CompositeKey)) ApplyCompositeKey(entityTypeBuilder, modelClass);
        }
    }

    public static void UseComplexTypes(DbContext context, ModelBuilder modelBuilder)
    {
        var types = _complexTypesCache.GetOrCreate(context.GetType().FullName!, entry =>
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
        })!;

        foreach (var type in types)
        {
            modelBuilder.Owned(type);
        }
    }

    private class RowLockItem
    {
        public PropertyInfo? Property { get; set; }
        public PropertyInfo[]? LockedProperties { get; set; }
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
            RowLockItem[]? rowLockItems = null;

            var properties = entriesOfType.Key.GetProperties().Where(x => x.CanWrite).ToArray();
            if (properties.Any(x => x.GetCustomAttribute<RowLockAttribute>() is not null))
            {
                var propDict = properties.ToDictionary(x => x.Name, x => x);

                rowLockItems =
                [
                    .. from prop in properties
                       let attr = prop.GetCustomAttribute<RowLockAttribute>()
                       where attr is not null
                       orderby attr.Order
                       select new RowLockItem
                       {
                           Property = prop,
                           LockedProperties = attr.Columns is not null ?
                           [
                               .. from column in attr.Columns select propDict[column]
                           ] : null,
                       }
                ];
            }

            foreach (var entry in entriesOfType)
            {
                if (rowLockItems is not null)
                {
                    ResolveRowLock(context, entry, rowLockItems);
                }

                ResolveAutoAttributes(context, entry, properties);
            }
        }

        // Resolve BeforeAudit
        foreach (var entriesOfType in auditEntriesByType)
        {
            var entityType = entriesOfType.Key;
            var attr = entityType.GetCustomAttribute<EntityAuditAttribute>()!;

            var auditType = typeof(EntityAudit<>).MakeGenericType(entityType);
            var audits = Array.CreateInstance(auditType, entriesOfType.Count());
            foreach (var (index, value) in entriesOfType.Pairs())
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
            var attr = entityType.GetCustomAttribute<EntityAuditAttribute>()!;

            var auditType = typeof(EntityAudit<>).MakeGenericType(entityType);
            var audits = Array.CreateInstance(auditType, entriesOfType.Count());
            foreach (var (index, value) in entriesOfType.Pairs())
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
            foreach (var entry in entriesByType)
            {
                predictor.Add(EntityAudit.Parse(entry));
            }
        }

        foreach (var entriesByType in auditEntriesByTypes)
        {
            var entityType = entriesByType.Key;
            var attr = entityType.GetCustomAttribute<EntityAuditAttribute>()!;
            var auditor = Activator.CreateInstance(attr.EntityAuditorType);
            auditor.GetReflector().DeclaredMethod(nameof(IEntityAuditor<DbContext, object>.OnAudited)).Call(context, predictor);
        }
    }

    private static void ApplyCompositeKey(object entityTypeBuilder, Type modelClass)
    {
        var hasKeyMethod = entityTypeBuilder.GetType().GetMethod(nameof(EntityTypeBuilder.HasKey), [typeof(string[])])!;

        var modelProps = modelClass.GetProperties()
            .Where(x => x.GetCustomAttribute<CPKeyAttribute>() is not null)
            .OrderBy(x => x.GetCustomAttribute<CPKeyAttribute>()!.Order);
        var propNames = modelProps.Select(x => x.Name).ToArray();

        if (propNames.Any())
        {
            hasKeyMethod.Invoke(entityTypeBuilder, [propNames]);
        }
    }

    private static void ApplyIndexes(object entityTypeBuilder, Type modelClass)
    {
        var hasIndexMethod = entityTypeBuilder.GetType().GetMethod(nameof(EntityTypeBuilder.HasIndex), [typeof(string[])])!;
        void SetIndex(string[] propertyNames, IndexType type)
        {
            if (propertyNames.Length == 0) throw new ArgumentException("No property specified.", nameof(propertyNames));

            var indexBuilder = (hasIndexMethod.Invoke(entityTypeBuilder, [propertyNames]) as IndexBuilder)!;
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

        foreach (var prop in props.Where(x => x.Index!.Group is null))
        {
            SetIndex([prop.Name!], prop.Index!.Type);
        }
        foreach (var group in props.Where(x => x.Index!.Group is not null).GroupBy(x => new
        {
            x.Index!.Type,
            x.Index.Group
        }))
        {
            SetIndex(group.Select(x => x.Name!).ToArray(), group.Key.Type);
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

                providerAttr = (specialProviderAttr as SpecialProviderAttribute)!.GetTargetProvider(modelProp);
            }

            if (providerAttr is not null)
            {
                var propertyBuilder = propertyMethod.Invoke(entityTypeBuilder, [modelProp.Name]) as PropertyBuilder;
                var hasConversionMethod = typeof(PropertyBuilder).GetMethod(nameof(PropertyBuilder.HasConversion), [typeof(ValueConverter)])!;

                var providerAttrType = providerAttr.GetType();
                dynamic provider = Activator.CreateInstance(providerAttrType)!;

                var converter = LinqSharpEF.BuildConverter(provider);
                hasConversionMethod.Invoke(propertyBuilder, [converter]);

                var comparer = LinqSharpEF.BuildComparer(provider);
                if (comparer is not null)
                {
                    var metadataProperty = typeof(PropertyBuilder).GetProperty(nameof(PropertyBuilder.Metadata))!;
                    var metadata = metadataProperty.GetValue(propertyBuilder);
#if EFCORE6_0_OR_GREATER
                    var setValueComparerMethod = typeof(IMutableProperty).GetMethod(nameof(IMutableProperty.SetValueComparer), new[] { typeof(ValueComparer) })!;
                    setValueComparerMethod.Invoke(metadata, new object[] { comparer });
#else
                    var setValueComparerMethod = typeof(MutablePropertyExtensions).GetMethod(nameof(MutablePropertyExtensions.SetValueComparer))!;
                    setValueComparerMethod.Invoke(null, [metadata, comparer]);
#endif
                }
            }
        }
    }

    private static AutoMode GetFieldMode(DbContext context, Type genericScope, string scopeName)
    {
        var scopeProp = _fieldOptionScopeCache.GetOrCreate($"{scopeName}&{context.GetType()}", entry =>
        {
            return genericScope
                .MakeGenericType(context.GetType())
                .GetProperty(nameof(Scope<RowLockScope<DbContext>>.Current), BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        })!;
        var scope = scopeProp.GetValue(null) as IAutoModeScope;
        return scope?.Mode ?? AutoMode.Auto;
    }

    private static InvalidOperationException LockedRowUpdatingException(Type type) => new($"Must be unlocked before updating. ({type.Name} can not be modified.)");
    private static InvalidOperationException LockedRowUpdatingException(Type type, PropertyInfo prop) => new($"Must be unlocked before updating. ({type.Name}.{prop.Name} can not be changed.)");
    private static InvalidOperationException LockedRowDeletingException(Type type) => new($"Must be unlocked before deleting. ({type} can not be deleted.)");

    private static void ResolveRowLock(DbContext context, EntityEntry entry, RowLockItem[] lockItems)
    {
        if (!new[] { EntityState.Modified, EntityState.Deleted }.Contains(entry.State)) return;

        var originValues = entry.GetDatabaseValues();
        void OperateThrow(PropertyInfo prop)
        {
            var originValue = originValues?[prop.Name] ?? default;
            var currentValue = prop.GetValue(entry.Entity);

            if (originValue != currentValue)
            {
                throw LockedRowUpdatingException(prop.DeclaringType!, prop);
            }
        }

        void OperateRestore(PropertyInfo prop)
        {
            var originValue = originValues?[prop.Name] ?? default;
            var currentValue = prop.GetValue(entry.Entity);

            if (originValue != currentValue)
            {
                prop.SetValue(entry.Entity, originValue);
            }
        }

        var mode = GetFieldMode(context, typeof(RowLockScope<>), nameof(RowLockScope<DbContext>));
        if (mode == AutoMode.Free) return;

        if (mode == AutoMode.Auto || mode == AutoMode.Suppress)
        {
            foreach (var item in lockItems)
            {
                var originValue = originValues?[item.Property!.Name] ?? default;
                if (originValue is null) continue;

                if (entry.State == EntityState.Deleted)
                {
                    throw LockedRowDeletingException(entry.Entity.GetType());
                }

                if (mode == AutoMode.Auto)
                {
                    if (item.LockedProperties is null)
                    {
                        if (entry.State == EntityState.Modified) throw LockedRowUpdatingException(entry.Entity.GetType());
                    }
                    else
                    {
                        OperateThrow(item.Property!);
                        foreach (var prop in item.LockedProperties)
                        {
                            OperateThrow(prop);
                        }
                    }
                }
                else
                {
                    // Reserve
                    OperateRestore(item.Property!);
                }
            }
        }
    }

    private static void ResolveAutoAttributes(DbContext context, EntityEntry entry, PropertyInfo[] properties)
    {
        if (!new[] { EntityState.Added, EntityState.Modified, EntityState.Deleted }.Contains(entry.State)) return;

        var timestampParam = new TimestampParam
        {
            Now = DateTime.Now,
            NowOffset = DateTimeOffset.Now,
        };

        var userTrace = context as IUserTraceable;
        var userTraceOption = new Lazy<AutoMode>(() =>
        {
            return GetFieldMode(context, typeof(UserTraceScope<>), nameof(UserTraceScope<DbContext>));
        });
        var timestampOption = new Lazy<AutoMode>(() =>
        {
            return GetFieldMode(context, typeof(TimestampScope<>), nameof(TimestampScope<DbContext>));
        });

        var originValues = entry.GetDatabaseValues();
        foreach (var prop in properties)
        {
            var propertyType = prop.PropertyType;
            var currentValue = prop.GetValue(entry.Entity);
            var attrs = prop.GetCustomAttributes<AutoAttribute>();
            var finalValue = currentValue;

            foreach (var attr in attrs)
            {
                if (attr is ISpecialAutoAttribute)
                {
                    if (!attr.States.Contains((AutoState)entry.State))
                    {
                        if (entry.State == EntityState.Modified)
                        {
                            finalValue = originValues?[prop.Name] ?? default;
                        }
                        continue;
                    }

                    AutoMode mode;
                    if (attr is SpecialAutoAttribute<TimestampParam> attr_now)
                    {
                        mode = timestampOption.Value;
                        if (mode == AutoMode.Auto)
                        {
                            finalValue = attr_now.Format(entry.Entity, propertyType, timestampParam);
                        }
                    }
                    else if (attr is SpecialAutoAttribute<UserParam> attr_user)
                    {
                        if (userTrace is null) throw new InvalidOperationException($"The context needs to implement {nameof(IUserTraceable)}.");

                        mode = userTraceOption.Value;
                        if (mode == AutoMode.Auto)
                        {
                            finalValue = attr_user.Format(entry.Entity, propertyType, new UserParam
                            {
                                CurrentUser = userTrace.CurrentUser,
                            });
                        }
                    }
                    else throw new NotImplementedException($"{attr.GetType()} is not processed.");

                    if (mode == AutoMode.Auto) { }
                    else if (mode == AutoMode.Suppress)
                    {
                        finalValue = originValues?[prop.Name] ?? default;
                    }
                    else if (mode == AutoMode.Free)
                    {
                        finalValue = currentValue;
                    }
                    else throw new NotImplementedException($"The option is not supported. ({mode}).");
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
