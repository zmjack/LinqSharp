using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static EnumerableWhereExpressionBuilder<TSource> Begin<TSource>(this IEnumerable<TSource> @this)
        {
            return new EnumerableWhereExpressionBuilder<TSource>(@this);
        }

        public static EnumerableWhereExpressionBuilder<TSource> Begin<TSource>(this IEnumerable<TSource> @this, Expression<Func<TSource, bool>> predicate)
        {
            return new EnumerableWhereExpressionBuilder<TSource>(@this, predicate);
        }

    }
}