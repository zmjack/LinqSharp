// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LinqSharp.EFCore
{
    public interface ICacheable<TDataSource> where TDataSource : class, new()
    {
        public TDataSource Source { get; }
        public void OnCache();
    }

    public static class XICacheable
    {
        private static readonly MemoryCache CacheablePropertiesCache = new(new MemoryCacheOptions());
        private static readonly MemoryCache CacheableQueryMethodCache = new(new MemoryCacheOptions());

        public static void Feed<TDataSource>(this ICacheable<TDataSource>[] cacheables, params DbContext[] contexts) where TDataSource : class, new()
        {
            foreach (var context in contexts)
            {
                var contextType = context.GetType();
                var sourceType = typeof(TDataSource);

                // TODO: Use direct function to optimize.
                var props = CacheablePropertiesCache.GetOrCreate($"{sourceType}:{contextType}", entry =>
                {
                    return typeof(TDataSource).GetProperties().Where(x =>
                    {
                        if (x.PropertyType.IsType(typeof(PreQuery<,>)))
                        {
                            var sourceContextType = x.PropertyType.GetGenericArguments()[0];
                            return sourceContextType.IsType(contextType) || sourceContextType.IsExtend(contextType);
                        }
                        else return false;
                    }).ToArray();
                });

                foreach (var prop in props)
                {
                    var preQueryType = prop.PropertyType;
                    var args = preQueryType.GetGenericArguments();
                    var preQueryContextType = args[0];
                    var entityType = args[1];
                    var queryMethod = CacheableQueryMethodCache.GetOrCreate($"{preQueryContextType},{entityType}", entry =>
                    {
                        return typeof(XPreQuery).GetMethod(nameof(XPreQuery.InnerFeed), BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(preQueryContextType, entityType);
                    });

                    var preQueries = Array.CreateInstance(preQueryType, cacheables.Count());
                    foreach (var kv in cacheables.AsKvPairs())
                    {
                        var preQuery = prop.GetValue(kv.Value.Source);
                        preQueries.SetValue(preQuery, kv.Key);
                    }
                    queryMethod.Invoke(null, new object[] { preQueries, context });
                }
            }

            foreach (var cacheable in cacheables)
            {
                cacheable.OnCache();
            }
        }
    }

}
