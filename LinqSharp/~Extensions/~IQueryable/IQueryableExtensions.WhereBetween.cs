// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Strategies;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class IQueryableExtensions
    {
        #region Return DateTime
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use Filter(DateTimeRangeFilter) instead.")]
        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            Expression<Func<TEntity, DateTime>> startExp,
            Expression<Func<TEntity, DateTime>> endExp)
        {
            return @this.Where(new QueryBetweenStrategy<TEntity>(memberExp, startExp, endExp).StrategyExpression);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use Filter(DateTimeRangeFilter) instead.")]
        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            DateTime start,
            Expression<Func<TEntity, DateTime>> endExp)
        {
            return @this.Where(new QueryBetweenStrategy<TEntity>(memberExp, start, endExp).StrategyExpression);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use Filter(DateTimeRangeFilter) instead.")]
        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            Expression<Func<TEntity, DateTime>> startExp,
            DateTime end)
        {
            return @this.Where(new QueryBetweenStrategy<TEntity>(memberExp, startExp, end).StrategyExpression);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use Filter(DateTimeRangeFilter) instead.")]
        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            DateTime start,
            DateTime end)
        {
            return @this.Where(new QueryBetweenStrategy<TEntity>(memberExp, start, end).StrategyExpression);
        }
        #endregion

        #region Return DateTime?
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use Filter(DateTimeRangeFilter) instead.")]
        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            Expression<Func<TEntity, DateTime>> startExp,
            Expression<Func<TEntity, DateTime>> endExp)
        {
            return @this.Where(new QueryBetweenStrategy<TEntity>(memberExp, startExp, endExp).StrategyExpression);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use Filter(DateTimeRangeFilter) instead.")]
        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            DateTime start,
            Expression<Func<TEntity, DateTime>> endExp)
        {
            return @this.Where(new QueryBetweenStrategy<TEntity>(memberExp, start, endExp).StrategyExpression);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use Filter(DateTimeRangeFilter) instead.")]
        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            Expression<Func<TEntity, DateTime>> startExp,
            DateTime end)
        {
            return @this.Where(new QueryBetweenStrategy<TEntity>(memberExp, startExp, end).StrategyExpression);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Use Filter(DateTimeRangeFilter) instead.")]
        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            DateTime start,
            DateTime end)
        {
            return @this.Where(new QueryBetweenStrategy<TEntity>(memberExp, start, end).StrategyExpression);
        }
        #endregion

    }
}
