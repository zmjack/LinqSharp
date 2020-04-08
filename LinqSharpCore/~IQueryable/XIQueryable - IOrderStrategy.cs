using LinqSharp.Strategies;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        /// <summary>
        /// Use an OrderStrategy to generate an orberby expression.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static IOrderedQueryable<TEntity> OrderByCaseStrategy<TEntity>(this IQueryable<TEntity> @this, IOrderStrategy<TEntity> strategy)
            => @this.OrderBy(strategy.StrategyExpression);

        /// <summary>
        /// Use an OrderStrategy to generate an orberby expression.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static IOrderedQueryable<TEntity> OrderByCaseDescendingStrategy<TEntity>(this IQueryable<TEntity> @this, IOrderStrategy<TEntity> strategy)
            => @this.OrderByDescending(strategy.StrategyExpression);

        /// <summary>
        /// Use an OrderStrategy to generate an orberby expression.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static IOrderedQueryable<TEntity> ThenByCaseStrategy<TEntity>(this IOrderedQueryable<TEntity> @this, IOrderStrategy<TEntity> strategy)
            => @this.ThenBy(strategy.StrategyExpression);

        /// <summary>
        /// Use an OrderStrategy to generate an orberby expression.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static IOrderedQueryable<TEntity> ThenByCaseDescendingStrategy<TEntity>(this IOrderedQueryable<TEntity> @this, IOrderStrategy<TEntity> strategy)
            => @this.ThenByDescending(strategy.StrategyExpression);

    }

}
