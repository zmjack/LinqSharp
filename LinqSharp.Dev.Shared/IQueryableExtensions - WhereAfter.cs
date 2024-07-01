// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Strategies;
using System.ComponentModel;
using System.Linq.Expressions;

namespace LinqSharp.Dev;

public static partial class IQueryableExtensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IQueryable<TEntity> WhereAfter<TEntity>(this IQueryable<TEntity> @this,
        Expression<Func<TEntity, DateTime>> memberExp,
        Expression<Func<TEntity, DateTime>> afterExp,
        bool includePoint = true)
    {
        return @this.Where(new QueryAfterStrategy<TEntity>(memberExp, afterExp, includePoint).StrategyExpression);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IQueryable<TEntity> WhereAfter<TEntity>(this IQueryable<TEntity> @this,
        Expression<Func<TEntity, DateTime>> memberExp,
        DateTime after,
        bool includePoint = true)
    {
        return @this.Where(new QueryAfterStrategy<TEntity>(memberExp, after, includePoint).StrategyExpression);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IQueryable<TEntity> WhereAfter<TEntity>(this IQueryable<TEntity> @this,
        Expression<Func<TEntity, DateTime?>> memberExp,
        Expression<Func<TEntity, DateTime>> afterExp,
        bool liftNullToTrue, bool includePoint = true)
    {
        return @this.Where(new QueryAfterStrategy<TEntity>(memberExp, afterExp, liftNullToTrue, includePoint).StrategyExpression);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IQueryable<TEntity> WhereAfter<TEntity>(this IQueryable<TEntity> @this,
        Expression<Func<TEntity, DateTime?>> memberExp,
        DateTime after,
        bool liftNullToTrue, bool includePoint = true)
    {
        return @this.Where(new QueryAfterStrategy<TEntity>(memberExp, after, liftNullToTrue, includePoint).StrategyExpression);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IQueryable<TEntity> WhereAfter<TEntity>(this IQueryable<TEntity> @this,
        Expression<Func<TEntity, object>> yearExp,
        Expression<Func<TEntity, object>> monthExp,
        Expression<Func<TEntity, object>> dayExp,
        DateTime after,
        bool includePoint = true)
    {
        return @this.Where(new QueryAfterStrategy<TEntity>(yearExp, monthExp, dayExp, after, includePoint).StrategyExpression);
    }
}
