using NLinq.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NLinq
{
    public static partial class XIEnumerable
    {
        public static IOrderedEnumerable<TEntity> OrderByCase<TEntity, TRet>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, TRet>> memberExp,
            TRet[] orderValues)
            => @this.OrderByCaseStrategy(new OrderByCaseStrategy<TEntity, TRet>(memberExp, orderValues));

        public static IOrderedEnumerable<TEntity> OrderByCaseDescending<TEntity, TRet>(this IEnumerable<TEntity> @this,
            Expression<Func<TEntity, TRet>> memberExp,
            TRet[] orderValues)
            => @this.OrderByCaseDescendingStrategy(new OrderByCaseStrategy<TEntity, TRet>(memberExp, orderValues));

        public static IOrderedEnumerable<TEntity> ThenByCase<TEntity, TRet>(this IOrderedEnumerable<TEntity> @this,
            Expression<Func<TEntity, TRet>> memberExp,
            TRet[] orderValues)
            => @this.ThenByCaseStrategy(new OrderByCaseStrategy<TEntity, TRet>(memberExp, orderValues));

        public static IOrderedEnumerable<TEntity> ThenByCaseDescending<TEntity, TRet>(this IOrderedEnumerable<TEntity> @this,
            Expression<Func<TEntity, TRet>> memberExp,
            TRet[] orderValues)
            => @this.ThenByCaseDescendingStrategy(new OrderByCaseStrategy<TEntity, TRet>(memberExp, orderValues));

    }
}
