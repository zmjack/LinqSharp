// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NStandard;
using System;
using System.ComponentModel;
using System.Linq;

namespace LinqSharp.EFCore
{
    public interface ICacheable<TDataSource> where TDataSource : class, new()
    {
        public TDataSource Source { get; }
        public void OnCached();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ICacheableExtensions
    {
        private static readonly MemoryCache CacheablePropertiesCache = new(new MemoryCacheOptions());
        private static readonly MemoryCache CacheableQueryMethodCache = new(new MemoryCacheOptions());

        [Obsolete("Obsolete", true)]
        public static void FetchData<TDataSource>(this ICacheable<TDataSource> cacheable, params DbContext[] contexts) where TDataSource : class, new() => FetchData(new[] { cacheable }, contexts);

        [Obsolete("Obsolete")]
        public static void FetchData<TDataSource>(this ICacheable<TDataSource>[] cacheables, params DbContext[] contexts) where TDataSource : class, new()
        {
            if (contexts.Any(x => x is null)) throw new ArgumentNullException("Any context can not be null.");

            foreach (var context in contexts)
            {
                var contextType = context.GetType();
                var sourceType = typeof(TDataSource);

                // TODO: Use direct function to optimize.
                var props = CacheablePropertiesCache.GetOrCreate($"{sourceType}:{contextType}", entry =>
                {
                    return typeof(TDataSource).GetProperties().Where(x =>
                    {
                        if (x.PropertyType.IsType(typeof(QueryDef<>)))
                        {
                            var sourceContextType = x.PropertyType.GetGenericArguments()[0];
                            return sourceContextType.IsType(contextType) || sourceContextType.IsExtend(contextType);
                        }
                        else return false;
                    }).ToArray();
                });

                foreach (var prop in props)
                {
                    var propertyType = prop.PropertyType;
                    var args = propertyType.GetGenericArguments();
                    var preQueryContextType = args[0];
                    var entityType = args[1];
                    var queryMethod = CacheableQueryMethodCache.GetOrCreate($"{preQueryContextType},{entityType}", entry =>
                    {
                        return typeof(CompoundQuery<,>).MakeGenericType(preQueryContextType, entityType).GetMethod(nameof(CompoundQuery<DbContext, string>.Feed));
                    });

                    var preQueries = Array.CreateInstance(propertyType, cacheables.Count());
                    foreach (var (index, value) in cacheables.AsIndexValuePairs())
                    {
                        var preQuery = prop.GetValue(value.Source);
                        preQueries.SetValue(preQuery, index);
                    }
                    queryMethod.Invoke(null, new object[] { preQueries, context });
                }
            }

            foreach (var cacheable in cacheables)
            {
                cacheable.OnCached();
            }
        }
    }

}
