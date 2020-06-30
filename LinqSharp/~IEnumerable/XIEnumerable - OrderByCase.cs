using LinqSharp.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static IOrderedEnumerable<TEntity> OrderByCase<TEntity, TRet>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, TRet>> memberExp,
            TRet[] orderValues)
        {
            return @this.OrderBy(new OrderByCaseStrategy<TEntity, TRet>(memberExp, orderValues).StrategyExpression.Compile());
        }

        public static IOrderedEnumerable<TEntity> OrderByCaseDescending<TEntity, TRet>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, TRet>> memberExp,
            TRet[] orderValues)
        {
            return @this.OrderByDescending(new OrderByCaseStrategy<TEntity, TRet>(memberExp, orderValues).StrategyExpression.Compile());
        }

        public static IOrderedEnumerable<TEntity> ThenByCase<TEntity, TRet>(this IOrderedEnumerable<TEntity> @this,
            Expression<Func<TEntity, TRet>> memberExp,
            TRet[] orderValues)
        {
            return @this.ThenBy(new OrderByCaseStrategy<TEntity, TRet>(memberExp, orderValues).StrategyExpression.Compile());
        }

        public static IOrderedEnumerable<TEntity> ThenByCaseDescending<TEntity, TRet>(this IOrderedEnumerable<TEntity> @this,
            Expression<Func<TEntity, TRet>> memberExp,
            TRet[] orderValues)
        {
            return @this.ThenByDescending(new OrderByCaseStrategy<TEntity, TRet>(memberExp, orderValues).StrategyExpression.Compile());
        }
    }
}
