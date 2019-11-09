using NLinq.Strategies;
using System.Collections.Generic;
using System.Linq;

namespace NLinq
{
    public static partial class XIEnumerable
    {
        /// <summary>
        /// Use a WhereStragtegy to generate a where expression.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="this"></param>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static IEnumerable<TEntity> WhereStrategy<TEntity>(this IEnumerable<TEntity> @this,
            IWhereStrategy<TEntity> strategy)
            => @this.Where(strategy.StrategyExpression.Compile());

    }
}
