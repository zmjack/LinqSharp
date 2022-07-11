// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Navigation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NStandard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.EFCore
{
    public class PreQuery<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        public Func<TDbContext, DbSet<TEntity>> DbSetSelector { get; private set; }
        public Expression<Func<TEntity, bool>> Predicate { get; private set; }
        public Func<TEntity, bool> LocalPredicate { get; private set; }
        public bool HasFiltered { get; private set; }
        public string Name { get; private set; }
        public bool NoTracking { get; private set; }
        public IncludeNavigation<TDbContext, TEntity> Navigation { get; internal set; }

        public TEntity[] Source { get; internal set; }
        public TEntity[] Result { get; internal set; }

        public PreQuery(Func<TDbContext, DbSet<TEntity>> dbSetSelector!!)
        {
            DbSetSelector = dbSetSelector;
        }

        public PreQuery<TDbContext, TEntity> As(string name)
        {
            Name = name;
            return this;
        }

        public IncludeNavigation<TDbContext, TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TProperty : class
        {
            var navigation = new IncludeNavigation<TDbContext, TEntity>(this);
            return navigation.Include(navigationPropertyPath);
        }

        public PreQuery<TDbContext, TEntity> AsNoTracking()
        {
            NoTracking = true;
            return this;
        }

        public PreQuery<TDbContext, TEntity> Where(Expression<Func<TEntity, bool>> predicate!!)
        {
            if (Predicate is null) Predicate = predicate;
            else Predicate = new[] { Predicate, predicate }.LambdaJoin(Expression.AndAlso);

            LocalPredicate = predicate.Compile();
            HasFiltered = true;
            return this;
        }
    }

    public static class XPreQuery
    {
        private static readonly MemoryCache IncludeCache = new(new MemoryCacheOptions());
        private static readonly MemoryCache ThenIncludeCache = new(new MemoryCacheOptions());
        private static readonly MemoryCache ThenInclude_EnumerableCache = new(new MemoryCacheOptions());

        private static readonly Lazy<MethodInfo> Lazy_IncludeMethod = new(() =>
        {
            return typeof(EntityFrameworkQueryableExtensions).GetMethodViaQualifiedName("Microsoft.EntityFrameworkCore.Query.IIncludableQueryable`2[TEntity,TProperty] Include[TEntity,TProperty](System.Linq.IQueryable`1[TEntity], System.Linq.Expressions.Expression`1[System.Func`2[TEntity,TProperty]])");
        });
        private static readonly Lazy<MethodInfo> Lazy_ThenIncludeMethod = new(() =>
        {
            return typeof(EntityFrameworkQueryableExtensions).GetMethodViaQualifiedName("Microsoft.EntityFrameworkCore.Query.IIncludableQueryable`2[TEntity,TProperty] ThenInclude[TEntity,TPreviousProperty,TProperty](Microsoft.EntityFrameworkCore.Query.IIncludableQueryable`2[TEntity,TPreviousProperty], System.Linq.Expressions.Expression`1[System.Func`2[TPreviousProperty,TProperty]])");
        });
        private static readonly Lazy<MethodInfo> Lazy_ThenIncludeMethod_Enumerable = new(() =>
        {
            return typeof(EntityFrameworkQueryableExtensions).GetMethodViaQualifiedName("Microsoft.EntityFrameworkCore.Query.IIncludableQueryable`2[TEntity,TProperty] ThenInclude[TEntity,TPreviousProperty,TProperty](Microsoft.EntityFrameworkCore.Query.IIncludableQueryable`2[TEntity,System.Collections.Generic.IEnumerable`1[TPreviousProperty]], System.Linq.Expressions.Expression`1[System.Func`2[TPreviousProperty,TProperty]])");
        });

        public static TEntity[] Feed<TDbContext, TEntity>(this PreQuery<TDbContext, TEntity> preQuery, TDbContext context) where TDbContext : DbContext where TEntity : class
        {
            return Feed(new[] { preQuery }, context);
        }

        internal static TEntity[] InnerFeed<TDbContext, TEntity>(PreQuery<TDbContext, TEntity>[] preQueries, TDbContext context) where TDbContext : DbContext where TEntity : class
        {
            return Feed(preQueries, context);
        }

        public static TEntity[] Feed<TDbContext, TEntity>(this IEnumerable<PreQuery<TDbContext, TEntity>> preQueries, TDbContext context) where TDbContext : DbContext where TEntity : class
        {
            var entityType = typeof(TEntity);

            var dbSets = preQueries.Select(x => x.DbSetSelector(context)).ToArray();
            if (!dbSets.Any()) return Array.Empty<TEntity>();
            if (!dbSets.AllSame()) throw new InvalidOperationException("The DbSets are inconsistent.");

            var dbSet = dbSets.First();
            var navigations = from preQuerier in preQueries let navigation = preQuerier.Navigation where navigation is not null select navigation;

            IQueryable<TEntity> queryable;
            if (preQueries.All(x => x.NoTracking))
                queryable = dbSet.AsNoTracking();
            else queryable = dbSet;

            foreach (var lists in navigations.SelectMany(x => x.PropertyPathLists))
            {
                using var enumerator = lists.GetEnumerator();

                if (enumerator.MoveNext())
                {
                    var firstNavigation = enumerator.Current;
                    var includeMethod = IncludeCache.GetOrCreate($"{entityType}|{firstNavigation.Property}", entry => Lazy_IncludeMethod.Value.MakeGenericMethod(entityType, firstNavigation.Property));
                    queryable = includeMethod.Invoke(null, new object[] { queryable, firstNavigation.Path }) as IQueryable<TEntity>;

                    while (enumerator.MoveNext())
                    {
                        var navigation = enumerator.Current;

                        MethodInfo thenIncludeMethod;
                        if (navigation.PreviousProperty.IsImplement<IEnumerable>())
                        {
                            thenIncludeMethod = ThenInclude_EnumerableCache.GetOrCreate($"{entityType}|{navigation.PreviousElement}|{navigation.Property}", entry =>
                            {
                                return Lazy_ThenIncludeMethod_Enumerable.Value.MakeGenericMethod(entityType, navigation.PreviousElement, navigation.Property);
                            });
                        }
                        else
                        {
                            thenIncludeMethod = ThenIncludeCache.GetOrCreate($"{entityType}|{navigation.PreviousProperty}|{navigation.Property}", entry =>
                            {
                                return Lazy_ThenIncludeMethod.Value.MakeGenericMethod(entityType, navigation.PreviousProperty, navigation.Property);
                            });
                        }
                        queryable = thenIncludeMethod.Invoke(null, new object[] { queryable, navigation.Path }) as IQueryable<TEntity>;
                    }
                }
            }

            TEntity[] entities;
            if (preQueries.All(x => x.HasFiltered))
                entities = queryable.XWhere(h => h.Or(from preQuery in preQueries let predicate = preQuery.Predicate where predicate is not null select predicate)).ToArray();
            else entities = queryable.ToArray();

            foreach (var preQuery in preQueries)
            {
                preQuery.Source = entities;
                preQuery.Result = preQuery.HasFiltered ? entities.Where(preQuery.LocalPredicate).ToArray() : entities;
            }
            return entities;
        }
    }

}
