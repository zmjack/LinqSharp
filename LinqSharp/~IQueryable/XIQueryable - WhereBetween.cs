﻿// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Strategies;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        #region Return DateTime
        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            Expression<Func<TEntity, DateTime>> startExp,
            Expression<Func<TEntity, DateTime>> endExp)
        {
            return @this.Where(new WhereBetweenStrategy<TEntity>(memberExp, startExp, endExp).StrategyExpression);
        }

        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            DateTime start,
            Expression<Func<TEntity, DateTime>> endExp)
        {
            return @this.Where(new WhereBetweenStrategy<TEntity>(memberExp, start, endExp).StrategyExpression);
        }

        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            Expression<Func<TEntity, DateTime>> startExp,
            DateTime end)
        {
            return @this.Where(new WhereBetweenStrategy<TEntity>(memberExp, startExp, end).StrategyExpression);
        }

        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            DateTime start,
            DateTime end)
        {
            return @this.Where(new WhereBetweenStrategy<TEntity>(memberExp, start, end).StrategyExpression);
        }
        #endregion

        #region Return DateTime?
        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            Expression<Func<TEntity, DateTime>> startExp,
            Expression<Func<TEntity, DateTime>> endExp)
        {
            return @this.Where(new WhereBetweenStrategy<TEntity>(memberExp, startExp, endExp).StrategyExpression);
        }

        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            DateTime start,
            Expression<Func<TEntity, DateTime>> endExp)
        {
            return @this.Where(new WhereBetweenStrategy<TEntity>(memberExp, start, endExp).StrategyExpression);
        }

        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            Expression<Func<TEntity, DateTime>> startExp,
            DateTime end)
        {
            return @this.Where(new WhereBetweenStrategy<TEntity>(memberExp, startExp, end).StrategyExpression);
        }

        public static IQueryable<TEntity> WhereBetween<TEntity>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            DateTime start,
            DateTime end)
        {
            return @this.Where(new WhereBetweenStrategy<TEntity>(memberExp, start, end).StrategyExpression);
        }
        #endregion

    }
}
