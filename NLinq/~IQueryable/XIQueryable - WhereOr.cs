using System;
using System.Linq;
using System.Linq.Expressions;

namespace NLinq
{
    public static partial class XIQueryable
    {
        [Obsolete("This method maybe will be removed. Use Begin instead.")]
        public static IQueryable<TSource> WhereOr<TSource>(this IQueryable<TSource> @this, params Expression<Func<TSource, bool>>[] predicates)
        {
            var parameter = predicates[0].Parameters[0];
            return @this.Where(predicates
                .Select(predicate => predicate.RebindParameter(predicate.Parameters[0], parameter))
                .LambdaJoin(Expression.OrElse));
        }

    }
}