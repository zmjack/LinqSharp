// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Strategies;
using System;
using System.Linq.Expressions;

namespace LinqSharp
{
    public partial class WhereHelper<TSource>
    {
        #region Return DateTime
        public WhereExpression<TSource> WhereBetween(
            Expression<Func<TSource, DateTime>> memberExp,
            Expression<Func<TSource, DateTime>> startExp,
            Expression<Func<TSource, DateTime>> endExp)
        {
            var strategy = new WhereBetweenStrategy<TSource>(memberExp, startExp, endExp);
            return new WhereExpression<TSource>(strategy.StrategyExpression);
        }

        public WhereExpression<TSource> WhereBetween(
            Expression<Func<TSource, DateTime>> memberExp,
            DateTime start,
            Expression<Func<TSource, DateTime>> endExp)
        {
            var strategy = new WhereBetweenStrategy<TSource>(memberExp, start, endExp);
            return new WhereExpression<TSource>(strategy.StrategyExpression);
        }

        public WhereExpression<TSource> WhereBetween(
            Expression<Func<TSource, DateTime>> memberExp,
            Expression<Func<TSource, DateTime>> startExp,
            DateTime end)
        {
            var strategy = new WhereBetweenStrategy<TSource>(memberExp, startExp, end);
            return new WhereExpression<TSource>(strategy.StrategyExpression);
        }

        public WhereExpression<TSource> WhereBetween(
            Expression<Func<TSource, DateTime>> memberExp,
            DateTime start,
            DateTime end)
        {
            var strategy = new WhereBetweenStrategy<TSource>(memberExp, start, end);
            return new WhereExpression<TSource>(strategy.StrategyExpression);
        }
        #endregion

        #region Return DateTime?
        public WhereExpression<TSource> WhereBetween(
            Expression<Func<TSource, DateTime?>> memberExp,
            Expression<Func<TSource, DateTime>> startExp,
            Expression<Func<TSource, DateTime>> endExp)
        {
            var strategy = new WhereBetweenStrategy<TSource>(memberExp, startExp, endExp);
            return new WhereExpression<TSource>(strategy.StrategyExpression);
        }

        public WhereExpression<TSource> WhereBetween(
            Expression<Func<TSource, DateTime?>> memberExp,
            DateTime start,
            Expression<Func<TSource, DateTime>> endExp)
        {
            var strategy = new WhereBetweenStrategy<TSource>(memberExp, start, endExp);
            return new WhereExpression<TSource>(strategy.StrategyExpression);
        }

        public WhereExpression<TSource> WhereBetween(
            Expression<Func<TSource, DateTime?>> memberExp,
            Expression<Func<TSource, DateTime>> startExp,
            DateTime end)
        {
            var strategy = new WhereBetweenStrategy<TSource>(memberExp, startExp, end);
            return new WhereExpression<TSource>(strategy.StrategyExpression);
        }

        public WhereExpression<TSource> WhereBetween(
            Expression<Func<TSource, DateTime?>> memberExp,
            DateTime start,
            DateTime end)
        {
            var strategy = new WhereBetweenStrategy<TSource>(memberExp, start, end);
            return new WhereExpression<TSource>(strategy.StrategyExpression);
        }
        #endregion

    }

}
