using Dawnx.Utilities;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NLinq
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