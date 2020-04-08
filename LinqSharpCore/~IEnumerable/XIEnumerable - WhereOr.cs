using NStandard.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        [Obsolete(ObsoleteMessage.MayChangeOrBeRemoved)]
        public static IEnumerable<TSource> WhereOr<TSource>(this IEnumerable<TSource> @this, params Expression<Func<TSource, bool>>[] predicates)
        {
            var parameter = predicates[0].Parameters[0];
            return @this.Where(predicates
                .Select(predicate => predicate.RebindParameter(predicate.Parameters[0], parameter))
                .LambdaJoin(Expression.OrElse).Compile());
        }

    }
}