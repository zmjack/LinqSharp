﻿// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Strategies;
using System.ComponentModel;
using System.Linq.Expressions;

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    #region Return DateTime
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IEnumerable<TEntity> WhereBetween<TEntity>(this IEnumerable<TEntity> @this,
        Expression<Func<TEntity, DateTime>> memberExp,
        Expression<Func<TEntity, DateTime>> startExp,
        Expression<Func<TEntity, DateTime>> endExp)
    {
        return @this.Where(new QueryBetweenStrategy<TEntity>(memberExp, startExp, endExp).StrategyExpression.Compile());
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IEnumerable<TEntity> WhereBetween<TEntity>(this IEnumerable<TEntity> @this,
        Expression<Func<TEntity, DateTime>> memberExp,
        DateTime start,
        Expression<Func<TEntity, DateTime>> endExp)
    {
        return @this.Where(new QueryBetweenStrategy<TEntity>(memberExp, start, endExp).StrategyExpression.Compile());
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IEnumerable<TEntity> WhereBetween<TEntity>(this IEnumerable<TEntity> @this,
        Expression<Func<TEntity, DateTime>> memberExp,
        Expression<Func<TEntity, DateTime>> startExp,
        DateTime end)
    {
        return @this.Where(new QueryBetweenStrategy<TEntity>(memberExp, startExp, end).StrategyExpression.Compile());
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IEnumerable<TEntity> WhereBetween<TEntity>(this IEnumerable<TEntity> @this,
        Expression<Func<TEntity, DateTime>> memberExp,
        DateTime start,
        DateTime end)
    {
        return @this.Where(new QueryBetweenStrategy<TEntity>(memberExp, start, end).StrategyExpression.Compile());
    }
    #endregion

    #region Return DateTime?
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IEnumerable<TEntity> WhereBetween<TEntity>(this IEnumerable<TEntity> @this,
        Expression<Func<TEntity, DateTime?>> memberExp,
        Expression<Func<TEntity, DateTime>> startExp,
        Expression<Func<TEntity, DateTime>> endExp)
    {
        return @this.Where(new QueryBetweenStrategy<TEntity>(memberExp, startExp, endExp).StrategyExpression.Compile());
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IEnumerable<TEntity> WhereBetween<TEntity>(this IEnumerable<TEntity> @this,
        Expression<Func<TEntity, DateTime?>> memberExp,
        DateTime start,
        Expression<Func<TEntity, DateTime>> endExp)
    {
        return @this.Where(new QueryBetweenStrategy<TEntity>(memberExp, start, endExp).StrategyExpression.Compile());
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IEnumerable<TEntity> WhereBetween<TEntity>(this IEnumerable<TEntity> @this,
        Expression<Func<TEntity, DateTime?>> memberExp,
        Expression<Func<TEntity, DateTime>> startExp,
        DateTime end)
    {
        return @this.Where(new QueryBetweenStrategy<TEntity>(memberExp, startExp, end).StrategyExpression.Compile());
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Use FilterBy(Func<,>, DateTimeRangeFilter) instead.")]
    public static IEnumerable<TEntity> WhereBetween<TEntity>(this IEnumerable<TEntity> @this,
        Expression<Func<TEntity, DateTime?>> memberExp,
        DateTime start,
        DateTime end)
    {
        return @this.Where(new QueryBetweenStrategy<TEntity>(memberExp, start, end).StrategyExpression.Compile());
    }
    #endregion

}
