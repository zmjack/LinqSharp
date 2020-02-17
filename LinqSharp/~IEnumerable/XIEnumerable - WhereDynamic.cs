using System;
using System.Collections.Generic;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static IEnumerable<TSource> WhereDynamic<TSource>(this IEnumerable<TSource> @this, Action<EnumerableWhereExpressionBuilder<TSource>> buildExpression)
        {
            var builder = new EnumerableWhereExpressionBuilder<TSource>(@this);
            buildExpression(builder);
            return builder.Build();
        }

    }
}