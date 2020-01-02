using Dawnx.Definition;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NLinq.ProviderFunctions;
using NStandard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace NLinq
{
    public static partial class NLinqUtility
    {
        public static ValueConverter<TModel, TProvider> BuildConverter<TModel, TProvider>(IProvider<TModel, TProvider> field)
        {
            return new ValueConverter<TModel, TProvider>(v => field.ConvertToProvider(v), v => field.ConvertFromProvider(v));
        }

        public static void ApplyProviderFunctions(DbContext context, ModelBuilder modelBuilder)
        {
            var providerName = context.GetProviderName();

            switch (providerName)
            {
                case DatabaseProviderName.Cosmos: goto default;
                case DatabaseProviderName.Firebird: goto default;
                case DatabaseProviderName.IBM: goto default;
                case DatabaseProviderName.OpenEdge: goto default;

                case DatabaseProviderName.Jet:
                    modelBuilder.HasDbFunction(typeof(PJet).GetMethod(nameof(PJet.Rnd)));
                    break;

                case DatabaseProviderName.MyCat:
                case DatabaseProviderName.MySql:
                    modelBuilder.HasDbFunction(typeof(PMySql).GetMethod(nameof(PMySql.Rand)));
                    break;

                case DatabaseProviderName.Oracle:
                    modelBuilder.HasDbFunction(typeof(POracle).GetMethod(nameof(POracle.Random)));
                    break;

                case DatabaseProviderName.PostgreSQL:
                    modelBuilder.HasDbFunction(typeof(PPostgreSQL).GetMethod(nameof(PPostgreSQL.Random)));
                    break;

                case DatabaseProviderName.Sqlite:
                    modelBuilder.HasDbFunction(typeof(PSqlite).GetMethod(nameof(PSqlite.Random)));
                    break;

                case DatabaseProviderName.SqlServer:
                case DatabaseProviderName.SqlServerCompact35:
                case DatabaseProviderName.SqlServerCompact40:
                    modelBuilder.HasDbFunction(typeof(PSqlServer).GetMethod(nameof(PSqlServer.Rand)));
                    break;

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

        public static void ApplyAnnotations(DbContext context, ModelBuilder modelBuilder, NLinqAnnotation annotation = NLinqAnnotation.All)
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

                if ((annotation & NLinqAnnotation.Index) == NLinqAnnotation.Index)
                    ApplyIndexes(entityTypeBuilder, modelClass);
                if ((annotation & NLinqAnnotation.Provider) == NLinqAnnotation.Provider)
                    ApplyProviders(entityTypeBuilder, modelClass);
                if ((annotation & NLinqAnnotation.CompositeKey) == NLinqAnnotation.CompositeKey)
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
            var entries = context.ChangeTracker.Entries()
                .Where(x => new[] { EntityState.Added, EntityState.Modified, EntityState.Deleted }.Contains(x.State))
                .ToArray();

            foreach (var entry in entries)
            {
                // Resolve TrackAttributes
                var entity = entry.Entity;
                var entityType = entity.GetType();
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    var props = entityType.GetProperties().Where(x => x.CanWrite).ToArray();
                    ResolveTrackAttributes(entry, props);
                }

                // Resolve Monitors
                var entityMonitor = entity as IEntityMonitor;
                if (entityMonitor != null)
                {
                    var paramType = typeof(EntityMonitorInvokerParameter<>).MakeGenericType(entityType);
                    var param = Activator.CreateInstance(paramType) as IEntityMonitorInvokerParameter;
                    param.State = entry.State;
                    param.Entity = entity;
                    param.PropertyEntries = entry.Properties;

                    EntityMonitor.GetMonitor(entityType.FullName)?.DynamicInvoke(param);
                }
            }

            // Resolve EntityTracker
            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                var entityType = entity.GetType();
                if (entityType.IsImplement(typeof(IEntityTracker<,>)))
                {
                    //TODO: Use TypeReflectionCacheContainer to optimize it in the futrue.
                    var trackerType = typeof(IEntityTracker<,>).MakeGenericType(context.GetType(), entityType);
                    var onInsertingMethod = trackerType.GetMethod(nameof(DefEntityTracker.OnInserting));
                    var onUpdatingMethod = trackerType.GetMethod(nameof(DefEntityTracker.OnUpdating));
                    var onDeletingMethod = trackerType.GetMethod(nameof(DefEntityTracker.OnDeleting));

                    var origin = Activator.CreateInstance(entry.Entity.GetType());
                    foreach (var originValue in entry.OriginalValues.Properties)
                        origin.GetReflector().Property(originValue.Name).Value = entry.OriginalValues[originValue.Name];

                    switch (entry.State)
                    {
                        case EntityState.Added: onInsertingMethod.Invoke(entity, new object[] { context }); break;
                        case EntityState.Modified: onUpdatingMethod.Invoke(entity, new object[] { context, origin }); break;
                        case EntityState.Deleted: onDeletingMethod.Invoke(entity, new object[] { context }); break;
                    }
                }
            }

            CompleteTracker(context);
        }

        private static void CompleteTracker(DbContext context)
        {
            var entries = context.ChangeTracker.Entries()
                .Where(x => new[] { EntityState.Added, EntityState.Modified, EntityState.Deleted }.Contains(x.State))
                .Where(x => x.Entity.GetType().IsImplement(typeof(IEntityTracker<,>)))
                .ToArray();

            foreach (var entry in entries)
            {
                //TODO: Use TypeReflectionCacheContainer to optimize it in the futrue.
                var entity = entry.Entity;
                var entityType = entity.GetType();
                var trackerType = typeof(IEntityTracker<,>).MakeGenericType(context.GetType(), entityType);
                var onCompletingMethod = trackerType.GetMethod(nameof(DefEntityTracker.OnCompleting));
                onCompletingMethod.Invoke(entity, new object[] { context, entry.State });
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

                    // Because of some unknown BUG in the EntityFramework, creating an index causes the first normal index to be dropped, which is defined with ForeignKeyAttribute.
                    // (The problem was found in EntityFrameworkCore 2.2.6)
                    //TODO: Here is the temporary solution
                    if (isForeignKey)
                        setIndex(new[] { propertyNames[0] }, false, false);
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
                    hasConversionMethod.Invoke(propertyBuilder, new object[] { NLinqUtility.BuildConverter(provider) });
                }
            }
        }

        private static void ResolveTrackAttributes(EntityEntry entry, PropertyInfo[] properties)
        {
            var props_TrackCreationTime = properties.Where(x => x.IsMarkedAs<TrackCreationTimeAttribute>());
            var props_TrackLastWrite = properties.Where(x => x.IsMarkedAs<TrackLastWriteTimeAttribute>());
            var props_TrackLower = properties.Where(x => x.IsMarkedAs<TrackLowerAttribute>());
            var props_TrackUpper = properties.Where(x => x.IsMarkedAs<TrackUpperAttribute>());
            var props_TrackTrim = properties.Where(x => x.IsMarkedAs<TrackTrimAttribute>());
            var props_TrackCondensed = properties.Where(x => x.IsMarkedAs<TrackCondensedAttribute>());
            var props_Track = properties.Where(x => x.IsMarkedAs<TrackAttribute>());

            var now = DateTime.Now;
            switch (entry.State)
            {
                case EntityState.Added:
                    SetPropertiesValue(props_TrackCreationTime, entry, v => now);
                    SetPropertiesValue(props_TrackLastWrite, entry, v => now);
                    SetPropertiesValue(props_TrackLower, entry, v => (v as string)?.ToLower());
                    SetPropertiesValue(props_TrackUpper, entry, v => (v as string)?.ToUpper());
                    SetPropertiesValue(props_TrackTrim, entry, v => (v as string)?.Trim());
                    SetPropertiesValue(props_TrackCondensed, entry, v => ((v as string) ?? "").Unique());
                    SetPropertiesValueForTrack(props_Track, entry);
                    break;

                case EntityState.Modified:
                    SetPropertiesValue(props_TrackLastWrite, entry, v => now);
                    SetPropertiesValue(props_TrackLower, entry, v => (v as string)?.ToLower());
                    SetPropertiesValue(props_TrackUpper, entry, v => (v as string)?.ToUpper());
                    SetPropertiesValue(props_TrackTrim, entry, v => (v as string)?.Trim());
                    SetPropertiesValue(props_TrackCondensed, entry, v => (v as string)?.Unique());
                    SetPropertiesValueForTrack(props_Track, entry);
                    break;
            }
        }

        private static void SetPropertiesValue(IEnumerable<PropertyInfo> properties, EntityEntry entry,
            Func<object, object> evalMethod)
        {
            foreach (var prop in properties)
            {
                var oldValue = prop.GetValue(entry.Entity);
                prop.SetValue(entry.Entity, evalMethod(oldValue));
            }
        }

        private static void SetPropertiesValueForTrack(IEnumerable<PropertyInfo> properties, EntityEntry entry)
        {
            foreach (var prop in properties)
            {
                var trackAttr = prop.GetCustomAttribute<TrackAttribute>();

                var type = trackAttr.Type;
                var csharp = trackAttr.CSharp;
                var entity = entry.Entity;
                var entityType = entity.GetType()
                    .For(self => self.Module.FullyQualifiedName != "<In Memory Module>" ? self : self.BaseType);

                Script shell;
                if (type != null)
                {
                    var references = new[] { type.Assembly.FullName };

                    //If the invoked method is 'Method<T>(this T @this),
                    //  the correct pattern is '@this.Method'
                    shell = CSharpScript.Create($"using static {type.Namespace}.{type.Name};",
                        ScriptOptions.Default.AddReferences(references), entityType)
                        .ContinueWith(csharp);
                }
                else shell = CSharpScript.Create(csharp, ScriptOptions.Default, entityType);

                var scriptState = shell.RunAsync(entity).Result;
                prop.SetValue(entity, scriptState.ReturnValue);
            }
        }


    }
}
