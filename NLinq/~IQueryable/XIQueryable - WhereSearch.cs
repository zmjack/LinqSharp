using NLinq.Strategies;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace NLinq
{
    public static partial class XIQueryable
    {
        public static IQueryable<TEntity> WhereSearch<TEntity>(this IQueryable<TEntity> @this,
            string searchString,
            Expression<Func<TEntity, object>> searchMembers)
            => @this.WhereStrategy(new WhereSearchStrategy<TEntity>(searchString, searchMembers));

        public static IQueryable<TEntity> WhereSearch<TEntity>(this IQueryable<TEntity> @this,
            string[] searchStrings,
            Expression<Func<TEntity, object>> searchMembers)
        {
            return searchStrings.Aggregate(@this,
                (acc, searchString) => acc.WhereStrategy(new WhereSearchStrategy<TEntity>(searchString, searchMembers)));
        }

        public static IQueryable<TEntity> WhereMatch<TEntity>(this IQueryable<TEntity> @this,
            string searchString,
            Expression<Func<TEntity, object>> searchMembers)
            => @this.WhereStrategy(new WhereMatchStrategy<TEntity>(searchString, searchMembers));

        public static IQueryable<TEntity> WhereMatch<TEntity>(this IQueryable<TEntity> @this,
            string[] searchStrings,
            Expression<Func<TEntity, object>> searchMembers)
        {
            return searchStrings.Aggregate(@this,
                (acc, searchString) => acc.WhereStrategy(new WhereMatchStrategy<TEntity>(searchString, searchMembers)));
        }

    }
}
