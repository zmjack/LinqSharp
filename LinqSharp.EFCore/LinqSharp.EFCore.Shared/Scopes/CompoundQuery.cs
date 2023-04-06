// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Navigation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.EFCore.Scopes
{
    internal static class CompoundQuery
    {
        internal static readonly MemoryCache Include_Cache = new(new MemoryCacheOptions());
        internal static readonly MemoryCache ThenInclude_Cache = new(new MemoryCacheOptions());

        internal static readonly Lazy<MethodInfo> Lazy_IncludeMethod = new(() =>
        {
            return typeof(EntityFrameworkQueryableExtensions).GetMethodViaQualifiedName("Microsoft.EntityFrameworkCore.Query.IIncludableQueryable`2[TEntity,TProperty] Include[TEntity,TProperty](System.Linq.IQueryable`1[TEntity], System.Linq.Expressions.Expression`1[System.Func`2[TEntity,TProperty]])");
        });
        internal static readonly Lazy<MethodInfo> Lazy_ThenIncludeMethod = new(() =>
        {
            return typeof(EntityFrameworkQueryableExtensions).GetMethodViaQualifiedName("Microsoft.EntityFrameworkCore.Query.IIncludableQueryable`2[TEntity,TProperty] ThenInclude[TEntity,TPreviousProperty,TProperty](Microsoft.EntityFrameworkCore.Query.IIncludableQueryable`2[TEntity,TPreviousProperty], System.Linq.Expressions.Expression`1[System.Func`2[TPreviousProperty,TProperty]])");
        });
        internal static readonly Lazy<MethodInfo> Lazy_ThenIncludeMethod_Enumerable = new(() =>
        {
            return typeof(EntityFrameworkQueryableExtensions).GetMethodViaQualifiedName("Microsoft.EntityFrameworkCore.Query.IIncludableQueryable`2[TEntity,TProperty] ThenInclude[TEntity,TPreviousProperty,TProperty](Microsoft.EntityFrameworkCore.Query.IIncludableQueryable`2[TEntity,System.Collections.Generic.IEnumerable`1[TPreviousProperty]], System.Linq.Expressions.Expression`1[System.Func`2[TPreviousProperty,TProperty]])");
        });
    }

    public class CompoundQuery<TEntity> : Scope<CompoundQuery<TEntity>>
        where TEntity : class
    {
        public IQueryable<TEntity> Queryable { get; }
        internal List<List<QueryTarget>> PropertyPathLists { get; } = new();

        internal CompoundQuery(IQueryable<TEntity> queryable)
        {
            Queryable = queryable;
        }

        [Obsolete("May be removed in the future.")]
        public IIncludable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TProperty : class
        {
            var targetPath = new List<QueryTarget>();
            var nav = new IncludeNavigation<TEntity, TProperty>(this, targetPath);
            PropertyPathLists.Add(nav.TargetPath);

            nav.TargetPath.Add(new QueryTarget
            {
                PreviousProperty = typeof(TEntity),
                Property = typeof(TProperty),
                Expression = navigationPropertyPath,
            });
            return nav;
        }

        private IQueryable<TEntity> GetBaseQuery()
        {
            var entityType = typeof(TEntity);
            IQueryable<TEntity> queryable = Queryable;

            foreach (var lists in PropertyPathLists)
            {
                using var enumerator = lists.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    var firstNavigation = enumerator.Current;
                    var includeMethod = CompoundQuery.Include_Cache.GetOrCreate($"{entityType}|{firstNavigation.Property}", entry => CompoundQuery.Lazy_IncludeMethod.Value.MakeGenericMethod(entityType, firstNavigation.Property));
                    queryable = includeMethod.Invoke(null, new object[] { queryable, firstNavigation.Expression }) as IQueryable<TEntity>;

                    while (enumerator.MoveNext())
                    {
                        var navigation = enumerator.Current;
                        var thenIncludeMethod = CompoundQuery.ThenInclude_Cache.GetOrCreate($"{entityType}|{navigation.PreviousProperty}|{navigation.Property}", entry =>
                        {
                            MethodInfo thenIncludeMethod;
                            Type lambdaProperty;

                            if (navigation.PreviousPropertyElement is not null)
                            {
                                thenIncludeMethod = CompoundQuery.Lazy_ThenIncludeMethod_Enumerable.Value;
                                lambdaProperty = navigation.PreviousPropertyElement;
                            }
                            else
                            {
                                thenIncludeMethod = CompoundQuery.Lazy_ThenIncludeMethod.Value;
                                lambdaProperty = navigation.PreviousProperty;
                            }

                            return thenIncludeMethod.MakeGenericMethod(entityType, lambdaProperty, navigation.Property);
                        });

                        queryable = thenIncludeMethod.Invoke(null, new object[] { queryable, navigation.Expression }) as IQueryable<TEntity>;
                    }
                }
            }

            return queryable;
        }

        public IQueryable<TEntity> BuildQuery(params QueryDef<TEntity>[] queryDefs)
        {
            IQueryable<TEntity> queryable = GetBaseQuery();
            if (!queryDefs.Any()) return queryable.Filter(h => h.False);

            if (queryDefs.All(x => x.HasFiltered))
            {
                queryable = queryable.Filter(h =>
                {
                    return h.Or(
                        from def in queryDefs
                        let predicate = def.Predicate
                        where predicate is not null
                        select predicate
                    );
                });
            }

            return queryable;
        }

        public TEntity[] Feed(params QueryDef<TEntity>[] queryDefs)
        {
            if (queryDefs is null) return Array.Empty<TEntity>();
            if (!queryDefs.Any()) return Array.Empty<TEntity>();

            var queryable = BuildQuery(queryDefs);

            TEntity[] entities = queryable.ToArray();
            foreach (var def in queryDefs)
            {
                def.Source = entities;

                if (def.HasFiltered)
                {
                    var predicate = def.Predicate.Compile();
                    def.Result = entities.Where(predicate).ToArray();
                }
                else def.Result = entities;
            }
            return entities;
        }

    }
}
