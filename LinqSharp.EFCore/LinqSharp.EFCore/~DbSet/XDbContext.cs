// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using NStandard;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace LinqSharp.EFCore
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class XDbContext
    {
        public static DatabaseProviderName GetProviderName(this DbContext @this)
        {
            return @this.Database.ProviderName switch
            {
                string name when name.Contains(DatabaseProviderName.Cosmos.ToString()) => DatabaseProviderName.Cosmos,
                string name when name.Contains(DatabaseProviderName.Firebird.ToString()) => DatabaseProviderName.Firebird,
                string name when name.Contains(DatabaseProviderName.IBM.ToString()) => DatabaseProviderName.IBM,
                string name when name.Contains(DatabaseProviderName.Jet.ToString()) => DatabaseProviderName.Jet,
                string name when name.Contains(DatabaseProviderName.MyCat.ToString()) => DatabaseProviderName.MyCat,
                string name when name.Contains(DatabaseProviderName.MySql.ToString()) => DatabaseProviderName.MySql,
                string name when name.Contains(DatabaseProviderName.OpenEdge.ToString()) => DatabaseProviderName.OpenEdge,
                string name when name.Contains(DatabaseProviderName.Oracle.ToString()) => DatabaseProviderName.Oracle,
                string name when name.Contains(DatabaseProviderName.PostgreSQL.ToString()) => DatabaseProviderName.PostgreSQL,
                string name when name.Contains(DatabaseProviderName.Sqlite.ToString()) => DatabaseProviderName.Sqlite,
                string name when name.Contains(DatabaseProviderName.SqlServer.ToString()) => DatabaseProviderName.SqlServer,
                string name when name.Contains(DatabaseProviderName.SqlServerCompact35.ToString()) => DatabaseProviderName.SqlServerCompact35,
                string name when name.Contains(DatabaseProviderName.SqlServerCompact40.ToString()) => DatabaseProviderName.SqlServerCompact40,
                _ => DatabaseProviderName.Unknown,
            };
        }

        public static string GetTableName<TEntity>(this DbContext @this) where TEntity : class
        {
            var entityTypes = @this.Model.GetEntityTypes();
            var entityType = entityTypes.First(x => x.ClrType == typeof(TEntity));
            return entityType.GetAnnotation("Relational:TableName").Value.ToString();
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public static DirectScope BeginDirectScope(this DbContext @this) => new();
#pragma warning restore IDE0060 // Remove unused parameter

        private static readonly MemoryCache CacheablePropertiesCache = new(new MemoryCacheOptions());
        private static readonly MemoryCache CacheableQueryMethodCache = new(new MemoryCacheOptions());

        public static void ApplyCache<TDbContext, TDataSource>(this TDbContext @this, ICacheable<TDataSource>[] cacheables) where TDbContext : DbContext where TDataSource : class, new()
        {
            // TODO: Use direct function to optimize.
            var props = CacheablePropertiesCache.GetOrCreate($"{typeof(TDbContext)}|{typeof(TDataSource)}", entry =>
            {
                var a = $"{typeof(TDbContext)}|{typeof(TDataSource)}";
                return typeof(TDataSource).GetProperties().Where(x =>
                {
                    var dbContextType = typeof(TDbContext);
                    var expectedDbContextType = x.PropertyType.GetGenericArguments()[0];
                    return (dbContextType.IsType(expectedDbContextType) || dbContextType.IsExtend(expectedDbContextType)) && x.PropertyType.IsType(typeof(PreQuery<,>));
                }).ToArray();
            });

            foreach (var prop in props)
            {
                var preQueryType = prop.PropertyType;
                var args = preQueryType.GetGenericArguments();
                var dbContextType = args[0];
                var entityType = args[1];
                var queryMethod = CacheableQueryMethodCache.GetOrCreate($"{typeof(TDbContext)}|{entityType}", entry =>
                {
                    return typeof(PreQuery).GetMethod(nameof(PreQuery.Execute), BindingFlags.Public | BindingFlags.Static).MakeGenericMethod(dbContextType, entityType);
                });

                var preQueries = Array.CreateInstance(preQueryType, cacheables.Count());
                foreach (var kv in cacheables.AsKvPairs())
                {
                    var preQuery = prop.GetValue(kv.Value.Source);
                    preQueries.SetValue(preQuery, kv.Key);
                }
                queryMethod.Invoke(null, new object[] { @this, preQueries });
            }
        }
    }
}
