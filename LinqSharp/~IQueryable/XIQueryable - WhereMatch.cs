using LinqSharp.Strategies;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        public static IQueryable<TEntity> WhereMatch<TEntity>(this IQueryable<TEntity> @this,
            string searchString,
            Expression<Func<TEntity, object>> searchMembers)
        {
            return @this.XWhere(h => h.WhereMatch(searchString, searchMembers));
        }

        [Obsolete("This function may cause performance problems.")]
        public static IQueryable<TEntity> WhereMatch<TEntity>(this IQueryable<TEntity> @this,
            string[] searchStrings,
            Expression<Func<TEntity, object>> searchMembers)
        {
            return searchStrings.Aggregate(@this,
                (acc, searchString) => acc.WhereStrategy(new WhereMatchStrategy<TEntity>(searchString, searchMembers)));
        }

    }
}
