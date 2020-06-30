using LinqSharp.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static IEnumerable<TEntity> Search<TEntity>(this IEnumerable<TEntity> @this, string searchString, Expression<Func<TEntity, object>> searchMembers, SearchOption option = SearchOption.Contains)
        {
            return @this.Where(new WhereSearchStrategy<TEntity>(searchString, searchMembers, option).StrategyExpression.Compile());
        }

        public static IEnumerable<TEntity> Search<TEntity>(this IEnumerable<TEntity> @this, string[] searchStrings, Expression<Func<TEntity, object>> searchMembers, SearchOption option = SearchOption.Contains)
        {
            return searchStrings.Aggregate(@this, (acc, searchString) => acc.Where(new WhereSearchStrategy<TEntity>(searchString, searchMembers, option).StrategyExpression.Compile()));
        }

    }
}
