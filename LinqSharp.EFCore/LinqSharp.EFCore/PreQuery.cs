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
    internal static class PreQuery
    {
        public static MemoryCache IncludeCache = new(new MemoryCacheOptions());
        private const string IncludeMethodName = "Microsoft.EntityFrameworkCore.Query.IIncludableQueryable`2[TEntity,TProperty] Include[TEntity,TProperty](System.Linq.IQueryable`1[TEntity], System.Linq.Expressions.Expression`1[System.Func`2[TEntity,TProperty]])";
        private const string ThenIncludeMethodName = "Microsoft.EntityFrameworkCore.Query.IIncludableQueryable`2[TEntity,TProperty] ThenInclude[TEntity,TPreviousProperty,TProperty](Microsoft.EntityFrameworkCore.Query.IIncludableQueryable`2[TEntity,TPreviousProperty], System.Linq.Expressions.Expression`1[System.Func`2[TPreviousProperty,TProperty]])";
        private const string ThenIncludeMethodName_Enumerable = "Microsoft.EntityFrameworkCore.Query.IIncludableQueryable`2[TEntity,TProperty] ThenInclude[TEntity,TPreviousProperty,TProperty](Microsoft.EntityFrameworkCore.Query.IIncludableQueryable`2[TEntity,System.Collections.Generic.IEnumerable`1[TPreviousProperty]], System.Linq.Expressions.Expression`1[System.Func`2[TPreviousProperty,TProperty]])";

        public static TEntity[] Excute<TDbContext, TEntity>(TDbContext context, params PreQuery<TDbContext, TEntity>[] preQueries!!) where TDbContext : DbContext where TEntity : class
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
                    var pathType = typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(firstNavigation.PreviousPropertyType, firstNavigation.PreviousPropertyType));

                    var includeMethodDefinition = IncludeCache.GetOrCreate($"{entityType}:Include", entry => typeof(EntityFrameworkQueryableExtensions).GetMethodViaQualifiedName(IncludeMethodName));
                    var includeMethod = includeMethodDefinition.MakeGenericMethod(firstNavigation.PreviousPropertyType, firstNavigation.PropertyType);
                    queryable = includeMethod.Invoke(null, new object[] { queryable, firstNavigation.NavigationPropertyPath }) as IQueryable<TEntity>;

                    if (enumerator.MoveNext())
                    {
                        var navigation = enumerator.Current;
                        var thenIncludeMethodDefinition = IncludeCache.GetOrCreate($"{entityType}:ThenInclude", entry => typeof(EntityFrameworkQueryableExtensions).GetMethodViaQualifiedName(ThenIncludeMethodName));
                        var thenIncludeMethodDefinition_Enumerable = IncludeCache.GetOrCreate($"{entityType}:ThenInclude_Enumerable", entry => typeof(EntityFrameworkQueryableExtensions).GetMethodViaQualifiedName(ThenIncludeMethodName_Enumerable));

                        void ExcuteThenInclude()
                        {
                            MethodInfo thenIncludeMethod;
                            if (navigation.PropertyType.IsImplement<IEnumerable>())
                                thenIncludeMethod = thenIncludeMethodDefinition_Enumerable.MakeGenericMethod(entityType, navigation.PreviousPropertyType, navigation.PropertyType);
                            else thenIncludeMethod = thenIncludeMethodDefinition.MakeGenericMethod(entityType, navigation.PreviousPropertyType, navigation.PropertyType);

                            queryable = thenIncludeMethod.Invoke(null, new object[] { queryable, navigation.NavigationPropertyPath }) as IQueryable<TEntity>;
                        }
                        ExcuteThenInclude();

                        while (enumerator.MoveNext())
                        {
                            ExcuteThenInclude();
                        }
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
                preQuery.Entities = preQuery.HasFiltered ? entities.Where(preQuery.LocalPredicate) : entities;
            }
            return entities;
        }
    }

    public class PreQuery<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        public TDbContext Context { get; private set; }
        public Func<TDbContext, DbSet<TEntity>> DbSetSelector { get; private set; }
        public IncludeNavigation<TDbContext, TEntity> Navigation { get; internal set; }
        public Expression<Func<TEntity, bool>> Predicate { get; private set; }
        public Func<TEntity, bool> LocalPredicate { get; private set; }
        public bool HasFiltered { get; private set; }
        public TEntity[] Source { get; internal set; }
        public IEnumerable<TEntity> Entities { get; internal set; }
        public string Name { get; private set; }
        public bool NoTracking { get; private set; }

        public PreQuery(TDbContext context!!, Func<TDbContext, DbSet<TEntity>> dbSetSelector!!)
        {
            Context = context;
            DbSetSelector = dbSetSelector;
        }

        public PreQuery<TDbContext, TEntity> As(string name)
        {
            Name = name;
            return this;
        }

        public IncludeNavigation<TDbContext, TEntity, TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TProperty : class
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
            Predicate = predicate;
            LocalPredicate = predicate.Compile();
            HasFiltered = true;
            return this;
        }

        public IEnumerable<TEntity> ToEnumerable(bool useFreshSource = false)
        {
            if (useFreshSource || Source is null) PreQuery.Excute(Context, this);
            return Entities;
        }

    }
}
