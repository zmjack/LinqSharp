// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Strategies;
using System;
using System.Linq.Expressions;

namespace LinqSharp.Query
{
    public partial class QueryHelper<TSource>
    {
        #region Return DateTime
        public QueryExpression<TSource> WhereBetween(
            Expression<Func<TSource, DateTime>> memberExp,
            Expression<Func<TSource, DateTime>> startExp,
            Expression<Func<TSource, DateTime>> endExp)
        {
            var strategy = new QueryBetweenStrategy<TSource>(memberExp, startExp, endExp);
            return new QueryExpression<TSource>(strategy.StrategyExpression);
        }

        public QueryExpression<TSource> WhereBetween(
            Expression<Func<TSource, DateTime>> memberExp,
            DateTime start,
            Expression<Func<TSource, DateTime>> endExp)
        {
            var strategy = new QueryBetweenStrategy<TSource>(memberExp, start, endExp);
            return new QueryExpression<TSource>(strategy.StrategyExpression);
        }

        public QueryExpression<TSource> WhereBetween(
            Expression<Func<TSource, DateTime>> memberExp,
            Expression<Func<TSource, DateTime>> startExp,
            DateTime end)
        {
            var strategy = new QueryBetweenStrategy<TSource>(memberExp, startExp, end);
            return new QueryExpression<TSource>(strategy.StrategyExpression);
        }

        public QueryExpression<TSource> WhereBetween(
            Expression<Func<TSource, DateTime>> memberExp,
            DateTime start,
            DateTime end)
        {
            var strategy = new QueryBetweenStrategy<TSource>(memberExp, start, end);
            return new QueryExpression<TSource>(strategy.StrategyExpression);
        }
        #endregion

        #region Return DateTime?
        public QueryExpression<TSource> WhereBetween(
            Expression<Func<TSource, DateTime?>> memberExp,
            Expression<Func<TSource, DateTime>> startExp,
            Expression<Func<TSource, DateTime>> endExp)
        {
            var strategy = new QueryBetweenStrategy<TSource>(memberExp, startExp, endExp);
            return new QueryExpression<TSource>(strategy.StrategyExpression);
        }

        public QueryExpression<TSource> WhereBetween(
            Expression<Func<TSource, DateTime?>> memberExp,
            DateTime start,
            Expression<Func<TSource, DateTime>> endExp)
        {
            var strategy = new QueryBetweenStrategy<TSource>(memberExp, start, endExp);
            return new QueryExpression<TSource>(strategy.StrategyExpression);
        }

        public QueryExpression<TSource> WhereBetween(
            Expression<Func<TSource, DateTime?>> memberExp,
            Expression<Func<TSource, DateTime>> startExp,
            DateTime end)
        {
            var strategy = new QueryBetweenStrategy<TSource>(memberExp, startExp, end);
            return new QueryExpression<TSource>(strategy.StrategyExpression);
        }

        public QueryExpression<TSource> WhereBetween(
            Expression<Func<TSource, DateTime?>> memberExp,
            DateTime start,
            DateTime end)
        {
            var strategy = new QueryBetweenStrategy<TSource>(memberExp, start, end);
            return new QueryExpression<TSource>(strategy.StrategyExpression);
        }
        #endregion

    }

}
