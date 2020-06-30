using LinqSharp.Strategies;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        public static IOrderedQueryable<TEntity> OrderByCase<TEntity, TRet>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, TRet>> memberExp,
            TRet[] orderValues)
        {
            return @this.OrderBy(new OrderByCaseStrategy<TEntity, TRet>(memberExp, orderValues).StrategyExpression);
        }

        public static IOrderedQueryable<TEntity> OrderByCaseDescending<TEntity, TRet>(this IQueryable<TEntity> @this,
            Expression<Func<TEntity, TRet>> memberExp,
            TRet[] orderValues)
        {
            return @this.OrderByDescending(new OrderByCaseStrategy<TEntity, TRet>(memberExp, orderValues).StrategyExpression);
        }

        public static IOrderedQueryable<TEntity> ThenByCase<TEntity, TRet>(this IOrderedQueryable<TEntity> @this,
            Expression<Func<TEntity, TRet>> memberExp,
            TRet[] orderValues)
        {
            return @this.ThenBy(new OrderByCaseStrategy<TEntity, TRet>(memberExp, orderValues).StrategyExpression);
        }

        public static IOrderedQueryable<TEntity> ThenByCaseDescending<TEntity, TRet>(this IOrderedQueryable<TEntity> @this,
            Expression<Func<TEntity, TRet>> memberExp,
            TRet[] orderValues)
        {
            return @this.ThenByDescending(new OrderByCaseStrategy<TEntity, TRet>(memberExp, orderValues).StrategyExpression);
        }
    }
}
