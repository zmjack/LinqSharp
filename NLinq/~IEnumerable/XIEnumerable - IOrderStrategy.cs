using NLinq.Strategies;
using System.Collections.Generic;
using System.Linq;

namespace NLinq
{
    public static partial class XIEnumerable
    {
        /// <summary>
        /// Use an OrderStrategy to generate an orberby expression.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TEntity> OrderByCaseStrategy<TEntity>(this IEnumerable<TEntity> @this, IOrderStrategy<TEntity> strategy)
            => @this.OrderBy(strategy.StrategyExpression.Compile());

        /// <summary>
        /// Use an OrderStrategy to generate an orberby expression.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TEntity> OrderByCaseDescendingStrategy<TEntity>(this IEnumerable<TEntity> @this, IOrderStrategy<TEntity> strategy)
            => @this.OrderByDescending(strategy.StrategyExpression.Compile());

        /// <summary>
        /// Use an OrderStrategy to generate an orberby expression.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TEntity> ThenByCaseStrategy<TEntity>(this IOrderedEnumerable<TEntity> @this, IOrderStrategy<TEntity> strategy)
            => @this.ThenBy(strategy.StrategyExpression.Compile());

        /// <summary>
        /// Use an OrderStrategy to generate an orberby expression.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TEntity> ThenByCaseDescendingStrategy<TEntity>(this IOrderedEnumerable<TEntity> @this, IOrderStrategy<TEntity> strategy)
            => @this.ThenByDescending(strategy.StrategyExpression.Compile());

    }
}
