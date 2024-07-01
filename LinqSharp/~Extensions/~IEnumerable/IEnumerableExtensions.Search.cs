// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Query;
using LinqSharp.Strategies;
using System.Linq.Expressions;

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    [Obsolete("Use SearchMode instead.")]
    public static IEnumerable<TEntity> Search<TEntity>(this IEnumerable<TEntity> @this, string searchString, Expression<Func<TEntity, object>> searchMembers, SearchOption option = SearchOption.Contains)
    {
        return @this.Where(new QuerySearchStrategy<TEntity>(searchString, searchMembers, option).StrategyExpression!.Compile());
    }

    [Obsolete("Use SearchMode instead.")]
    public static IEnumerable<TEntity> Search<TEntity>(this IEnumerable<TEntity> @this, string[] searchStrings, Expression<Func<TEntity, object>> searchMembers, SearchOption option = SearchOption.Contains)
    {
        return searchStrings.Aggregate(@this, (acc, searchString) => acc.Where(new QuerySearchStrategy<TEntity>(searchString, searchMembers, option).StrategyExpression!.Compile()));
    }

    public static IEnumerable<TEntity> Search<TEntity>(this IEnumerable<TEntity> @this, SearchMode mode, string search, Expression<Func<TEntity, SearchSelector>> selector)
    {
        return @this.Filter(new SearchFilter<TEntity>(mode, [search], selector));
    }

    public static IEnumerable<TEntity> Search<TEntity>(this IEnumerable<TEntity> @this, SearchMode mode, string[] searches, Expression<Func<TEntity, SearchSelector>> selector)
    {
        return @this.Filter(new SearchFilter<TEntity>(mode, searches, selector));
    }

}
