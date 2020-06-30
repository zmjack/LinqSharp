using LinqSharp.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static IEnumerable<TEntity> WhereSearch<TEntity>(this IEnumerable<TEntity> @this, string searchString, Expression<Func<TEntity, object>> searchMembers)
        {
            return @this.WhereStrategy(new WhereSearchStrategy<TEntity>(searchString, searchMembers));
        }

        public static IEnumerable<TEntity> WhereSearch<TEntity>(this IEnumerable<TEntity> @this, string[] searchStrings, Expression<Func<TEntity, object>> searchMembers)
        {
            return searchStrings.Aggregate(@this,
                (acc, searchString) => acc.WhereStrategy(new WhereSearchStrategy<TEntity>(searchString, searchMembers)));
        }

        public static IEnumerable<TEntity> WhereMatch<TEntity>(this IEnumerable<TEntity> @this, string searchString, Expression<Func<TEntity, object>> searchMembers)
        {
            return @this.WhereStrategy(new WhereMatchStrategy<TEntity>(searchString, searchMembers));
        }

        public static IEnumerable<TEntity> WhereMatch<TEntity>(this IEnumerable<TEntity> @this, string[] searchStrings, Expression<Func<TEntity, object>> searchMembers)
        {
            return searchStrings.Aggregate(@this,
                (acc, searchString) => acc.WhereStrategy(new WhereMatchStrategy<TEntity>(searchString, searchMembers)));
        }
    }
}
