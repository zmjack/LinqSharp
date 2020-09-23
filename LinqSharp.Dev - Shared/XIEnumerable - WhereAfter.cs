// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp.Dev
{
    public static partial class XIEnumerable
    {
        public static IEnumerable<TEntity> WhereAfter<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            Expression<Func<TEntity, DateTime>> afterExp,
            bool includePoint = true)
        {
            return @this.Where(new WhereAfterStrategy<TEntity>(memberExp, afterExp, includePoint).StrategyExpression.Compile());
        }

        public static IEnumerable<TEntity> WhereAfter<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            DateTime after,
            bool includePoint = true)
        {
            return @this.Where(new WhereAfterStrategy<TEntity>(memberExp, after, includePoint).StrategyExpression.Compile());
        }

        public static IEnumerable<TEntity> WhereAfter<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            Expression<Func<TEntity, DateTime>> afterExp,
            bool liftNullToTrue, bool includePoint = true)
        {
            return @this.Where(new WhereAfterStrategy<TEntity>(memberExp, afterExp, liftNullToTrue, includePoint).StrategyExpression.Compile());
        }

        public static IEnumerable<TEntity> WhereAfter<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            DateTime after,
            bool liftNullToTrue, bool includePoint = true)
        {
            return @this.Where(new WhereAfterStrategy<TEntity>(memberExp, after, liftNullToTrue, includePoint).StrategyExpression.Compile());
        }

        public static IEnumerable<TEntity> WhereAfter<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, object>> yearExp,
            Expression<Func<TEntity, object>> monthExp,
            Expression<Func<TEntity, object>> dayExp,
            DateTime after,
            bool includePoint = true)
        {
            string GetPart(TEntity x, Expression<Func<TEntity, object>> exp, int totalWidth)
            {
                return exp.Compile()(x).ToString().PadLeft(totalWidth, '0');
            }

            return @this.Where(x =>
            {
                if (includePoint)
                    return string.CompareOrdinal($"{GetPart(x, yearExp, 4)}-{GetPart(x, monthExp, 2)}-{GetPart(x, dayExp, 2)}", after.ToString("yyyy-MM-dd")) >= 0;
                else return string.CompareOrdinal($"{GetPart(x, yearExp, 4)}-{GetPart(x, monthExp, 2)}-{GetPart(x, dayExp, 2)}", after.ToString("yyyy-MM-dd")) > 0;
            });
        }

    }
}
