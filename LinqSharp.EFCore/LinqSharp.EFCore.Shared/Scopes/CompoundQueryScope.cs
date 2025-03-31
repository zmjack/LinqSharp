// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Navigation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NStandard;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.EFCore.Scopes;

internal static class CompoundQueryScope
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

public sealed class CompoundQueryScope<T> : Scope<CompoundQueryScope<T>>
    where T : class
{
    public IQueryable<T> Queryable { get; }
    internal List<List<QueryTarget>> PropertyPathLists { get; } = new();

    internal CompoundQueryScope(IQueryable<T> queryable)
    {
        Queryable = queryable;
    }

    [Obsolete("May be removed in the future.")]
    public IIncludable<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath) where TProperty : class
    {
        var targetPath = new List<QueryTarget>();
        var nav = new IncludeNavigation<T, TProperty>(this, targetPath);
        PropertyPathLists.Add(nav.TargetPath);

        nav.TargetPath.Add(new QueryTarget
        {
            PreviousProperty = typeof(T),
            Property = typeof(TProperty),
            Expression = navigationPropertyPath,
        });
        return nav;
    }

    private IQueryable<T> GetBaseQuery()
    {
        var entityType = typeof(T);
        IQueryable<T> queryable = Queryable;

        foreach (var lists in PropertyPathLists)
        {
            using var enumerator = lists.GetEnumerator();
            if (enumerator.MoveNext())
            {
                var firstNavigation = enumerator.Current;
                var includeMethod = CompoundQueryScope.Include_Cache.GetOrCreate($"{entityType}|{firstNavigation.Property}", entry =>
                {
                    return CompoundQueryScope.Lazy_IncludeMethod.Value.MakeGenericMethod(entityType, firstNavigation.Property)!;
                })!;
                queryable = (includeMethod.Invoke(null, [queryable, firstNavigation.Expression]) as IQueryable<T>)!;

                while (enumerator.MoveNext())
                {
                    var navigation = enumerator.Current;
                    var thenIncludeMethod = CompoundQueryScope.ThenInclude_Cache.GetOrCreate($"{entityType}|{navigation.PreviousProperty}|{navigation.Property}", entry =>
                    {
                        MethodInfo thenIncludeMethod;
                        Type lambdaProperty;

                        if (navigation.PreviousPropertyElement is not null)
                        {
                            thenIncludeMethod = CompoundQueryScope.Lazy_ThenIncludeMethod_Enumerable.Value;
                            lambdaProperty = navigation.PreviousPropertyElement;
                        }
                        else
                        {
                            thenIncludeMethod = CompoundQueryScope.Lazy_ThenIncludeMethod.Value;
                            lambdaProperty = navigation.PreviousProperty;
                        }

                        return thenIncludeMethod.MakeGenericMethod(entityType, lambdaProperty, navigation.Property);
                    })!;

                    queryable = (thenIncludeMethod.Invoke(null, [queryable, navigation.Expression]) as IQueryable<T>)!;
                }
            }
        }

        return queryable;
    }

    public IQueryable<T> BuildQuery(params QueryDef<T>[] queryDefs)
    {
        IQueryable<T> queryable = GetBaseQuery();
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

    public T[] Feed(params QueryDef<T>[] queryDefs)
    {
        if (queryDefs is null) return Array.Empty<T>();
        if (!queryDefs.Any()) return Array.Empty<T>();

        var queryable = BuildQuery(queryDefs);

        T[] entities = queryable.ToArray();
        foreach (var def in queryDefs)
        {
            def.Source = entities;

            if (def.HasFiltered)
            {
                var predicate = def.Predicate!.Compile();
                def.Result = entities.Where(predicate).ToArray();
            }
            else def.Result = entities;
        }
        return entities;
    }
}
