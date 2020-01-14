using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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

        public static IEnumerable<TSource> WhereDynamic<TSource>(this IEnumerable<TSource> @this, Expression<Func<TSource, bool>> predicate)
        {
            var builder = new EnumerableWhereExpressionBuilder<TSource>(@this, predicate);
            return builder.Build();
        }

        public static IEnumerable<TSource> WhereDynamic<TSource>(this IEnumerable<TSource> @this, Expression<Func<TSource, bool>> predicate, Action<EnumerableWhereExpressionBuilder<TSource>> buildExpression)
        {
            var builder = new EnumerableWhereExpressionBuilder<TSource>(@this, predicate);
            buildExpression(builder);
            return builder.Build();
        }

    }
}