// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Navigation;
using LinqSharp.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NStandard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public Expression Selector { get; private set; }
        public bool HasFiltered { get; private set; }
        public string Name { get; private set; }
        public bool NoTracking { get; private set; }
        internal List<List<QueryTarget>> PropertyPathLists { get; } = new();

        public TEntity[] Source { get; internal set; }
        public TEntity[] Result { get; internal set; }

        public PreQuery(Func<TDbContext, DbSet<TEntity>> dbSetSelector)
        {
            if (dbSetSelector is null) throw new ArgumentNullException(nameof(dbSetSelector));

            DbSetSelector = dbSetSelector;
        }

        public PreQuery<TDbContext, TEntity> As(string name)
        {
            Name = name;
            return this;
        }

        public IIncludable<TDbContext, TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TProperty : class
        {
            var targetPath = new List<QueryTarget>();
            var nav = new IncludeNavigation<TDbContext, TEntity, TProperty>(this, targetPath);
            PropertyPathLists.Add(nav.TargetPath);

            nav.TargetPath.Add(new QueryTarget
            {
                PreviousProperty = typeof(TEntity),
                Property = typeof(TProperty),
                Expression = navigationPropertyPath,
            });
            return nav;
        }

        public PreQuery<TDbContext, TEntity> AsNoTracking()
        {
            NoTracking = true;
            return this;
        }

        public PreQuery<TDbContext, TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            Predicate = Predicate is not null
                ? new[] { Predicate, predicate }.LambdaJoin(Expression.AndAlso)
                : predicate;
            HasFiltered = true;
            return this;
        }

        public PreQuery<TDbContext, TEntity> Filter(Func<QueryHelper<TEntity>, QueryExpression<TEntity>> filter)
        {
            var helper = new QueryHelper<TEntity>();
            var whereExp = filter(helper);

            if (whereExp.Expression is not null)
            {
                return Where(whereExp.Expression);
            }
            else return this;
        }

        public override string ToString()
        {
            return Predicate?.ToString() ?? "<Empty expression>";
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class PreQueryExtensions
    {
        private static readonly MemoryCache Include_Cache = new(new MemoryCacheOptions());
        private static readonly MemoryCache ThenInclude_Cache = new(new MemoryCacheOptions());
        private static readonly MemoryCache Enumerable_ThenInclude_Cache = new(new MemoryCacheOptions());

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

        public static IQueryable<TEntity> ToQuery<TDbContext, TEntity>(this PreQuery<TDbContext, TEntity> preQuery, TDbContext context) where TDbContext : DbContext where TEntity : class
        {
            return ToQuery(new[] { preQuery }, context);
        }

        public static IQueryable<TEntity> ToQuery<TDbContext, TEntity>(this PreQuery<TDbContext, TEntity>[] preQueries, TDbContext context) where TDbContext : DbContext where TEntity : class
        {
            var entityType = typeof(TEntity);

            var dbSets = preQueries.Select(x => x.DbSetSelector(context)).ToArray();
            if (!dbSets.Any()) return null;
            if (!dbSets.AllSame()) throw new InvalidOperationException("The DbSets are inconsistent.");

            var dbSet = dbSets.First();
            IQueryable<TEntity> queryable = preQueries.All(x => x.NoTracking) ? dbSet.AsNoTracking() : dbSet;

            foreach (var lists in preQueries.SelectMany(x => x.PropertyPathLists))
            {
                using var enumerator = lists.GetEnumerator();

                if (enumerator.MoveNext())
                {
                    var firstNavigation = enumerator.Current;
                    var includeMethod = Include_Cache.GetOrCreate($"{entityType}|{firstNavigation.Property}", entry => Lazy_IncludeMethod.Value.MakeGenericMethod(entityType, firstNavigation.Property));
                    queryable = includeMethod.Invoke(null, new object[] { queryable, firstNavigation.Expression }) as IQueryable<TEntity>;

                    while (enumerator.MoveNext())
                    {
                        var navigation = enumerator.Current;

                        MethodInfo thenIncludeMethod;
                        if (navigation.PreviousPropertyElement is not null)
                        {
                            thenIncludeMethod = Enumerable_ThenInclude_Cache.GetOrCreate($"{entityType}|{navigation.PreviousProperty}|{navigation.Property}", entry =>
                            {
                                return Lazy_ThenIncludeMethod_Enumerable.Value.MakeGenericMethod(entityType, navigation.PreviousPropertyElement, navigation.Property);
                            });
                        }
                        else
                        {
                            thenIncludeMethod = ThenInclude_Cache.GetOrCreate($"{entityType}|{navigation.PreviousProperty}|{navigation.Property}", entry =>
                            {
                                return Lazy_ThenIncludeMethod.Value.MakeGenericMethod(entityType, navigation.PreviousProperty, navigation.Property);
                            });
                        }

                        queryable = thenIncludeMethod.Invoke(null, new object[] { queryable, navigation.Expression }) as IQueryable<TEntity>;
                    }
                }
            }

            if (preQueries.All(x => x.HasFiltered))
            {
                queryable = queryable.Filter(h =>
                {
                    return h.Or(
                        from preQuery in preQueries
                        let predicate = preQuery.Predicate
                        where predicate is not null
                        select predicate
                    );
                });
            }

            return queryable;
        }

        public static TEntity[] Feed<TDbContext, TEntity>(this PreQuery<TDbContext, TEntity> preQuery, TDbContext context) where TDbContext : DbContext where TEntity : class
        {
            return FeedAll(new[] { preQuery }, context);
        }

        public static TEntity[] FeedAll<TDbContext, TEntity>(this PreQuery<TDbContext, TEntity>[] preQueries, TDbContext context) where TDbContext : DbContext where TEntity : class
        {
            var queryable = ToQuery(preQueries, context);
            if (queryable is null) return Array.Empty<TEntity>();

            TEntity[] entities = queryable.ToArray();
            foreach (var preQuery in preQueries)
            {
                var predicate = preQuery.Predicate.Compile();
                preQuery.Source = entities;
                preQuery.Result = preQuery.HasFiltered ? entities.Where(predicate).ToArray() : entities;
            }
            return entities;
        }
    }

}
