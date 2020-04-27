using System;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        public static IQueryable<TSource> WhereDynamic<TSource>(this IQueryable<TSource> @this, Action<QueryableWhereExpressionBuilder<TSource>> buildExpression)
        {
            var builder = new QueryableWhereExpressionBuilder<TSource>(@this);
            buildExpression(builder);
            return builder.Build();
        }

    }
}