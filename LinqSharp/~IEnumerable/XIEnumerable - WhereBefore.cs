using LinqSharp.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static IEnumerable<TEntity> WhereBefore<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            Expression<Func<TEntity, DateTime>> beforeExp,
            bool includePoint = true)
        {
            return @this.Where(new WhereBeforeStrategy<TEntity>(memberExp, beforeExp, includePoint).StrategyExpression.Compile());
        }

        public static IEnumerable<TEntity> WhereBefore<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime>> memberExp,
            DateTime before,
            bool includePoint = true)
        {
            return @this.Where(new WhereBeforeStrategy<TEntity>(memberExp, before, includePoint).StrategyExpression.Compile());
        }

        public static IEnumerable<TEntity> WhereBefore<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            Expression<Func<TEntity, DateTime>> beforeExp,
            bool liftNullToTrue, bool includePoint = true)
        {
            return @this.Where(new WhereBeforeStrategy<TEntity>(memberExp, beforeExp, liftNullToTrue, includePoint).StrategyExpression.Compile());
        }

        public static IEnumerable<TEntity> WhereBefore<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, DateTime?>> memberExp,
            DateTime before,
            bool liftNullToTrue, bool includePoint = true)
        {
            return @this.Where(new WhereBeforeStrategy<TEntity>(memberExp, before, liftNullToTrue, includePoint).StrategyExpression.Compile());
        }

        public static IEnumerable<TEntity> WhereBefore<TEntity>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, object>> yearExp,
            Expression<Func<TEntity, object>> monthExp,
            Expression<Func<TEntity, object>> dayExp,
            DateTime before,
            bool includePoint = true)
        {
            string GetPart(TEntity x, Expression<Func<TEntity, object>> exp, int totalWidth)
            {
                return exp.Compile()(x).ToString().PadLeft(totalWidth, '0');
            }

            return @this.Where(x =>
            {
                if (includePoint)
                    return string.CompareOrdinal($"{GetPart(x, yearExp, 4)}-{GetPart(x, monthExp, 2)}-{GetPart(x, dayExp, 2)}", before.ToString("yyyy-MM-dd")) <= 0;
                else return string.CompareOrdinal($"{GetPart(x, yearExp, 4)}-{GetPart(x, monthExp, 2)}-{GetPart(x, dayExp, 2)}", before.ToString("yyyy-MM-dd")) < 0;
            });
        }

    }
}
