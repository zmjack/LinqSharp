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
    public static IQueryable<TEntity> WhereBefore<TEntity>(this IQueryable<TEntity> @this,
        Expression<Func<TEntity, DateTime>> memberExp,
        Expression<Func<TEntity, DateTime>> beforeExp,
        bool includePoint = true)
    {
        return @this.Where(new QueryBeforeStrategy<TEntity>(memberExp, beforeExp, includePoint).StrategyExpression);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IQueryable<TEntity> WhereBefore<TEntity>(this IQueryable<TEntity> @this,
        Expression<Func<TEntity, DateTime>> memberExp,
        DateTime before,
        bool includePoint = true)
    {
        return @this.Where(new QueryBeforeStrategy<TEntity>(memberExp, before, includePoint).StrategyExpression);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IQueryable<TEntity> WhereBefore<TEntity>(this IQueryable<TEntity> @this,
        Expression<Func<TEntity, DateTime?>> memberExp,
        Expression<Func<TEntity, DateTime>> beforeExp,
        bool liftNullToTrue, bool includePoint = true)
    {
        return @this.Where(new QueryBeforeStrategy<TEntity>(memberExp, beforeExp, liftNullToTrue, includePoint).StrategyExpression);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IQueryable<TEntity> WhereBefore<TEntity>(this IQueryable<TEntity> @this,
        Expression<Func<TEntity, DateTime?>> memberExp,
        DateTime before,
        bool liftNullToTrue, bool includePoint = true)
    {
        return @this.Where(new QueryBeforeStrategy<TEntity>(memberExp, before, liftNullToTrue, includePoint).StrategyExpression);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IQueryable<TEntity> WhereBefore<TEntity>(this IQueryable<TEntity> @this,
        Expression<Func<TEntity, object>> yearExp,
        Expression<Func<TEntity, object>> monthExp,
        Expression<Func<TEntity, object>> dayExp,
        DateTime before,
        bool includePoint = true)
    {
        return @this.Where(new QueryBeforeStrategy<TEntity>(yearExp, monthExp, dayExp, before, includePoint).StrategyExpression);
    }
}
