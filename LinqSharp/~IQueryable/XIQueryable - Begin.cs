using System;
using System.Linq;
using System.Linq.Expressions;

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

        public static IQueryable<TSource> WhereDynamic<TSource>(this IQueryable<TSource> @this, Expression<Func<TSource, bool>> predicate)
        {
            var builder = new QueryableWhereExpressionBuilder<TSource>(@this, predicate);
            return builder.Build();
        }

        public static IQueryable<TSource> WhereDynamic<TSource>(this IQueryable<TSource> @this, Expression<Func<TSource, bool>> predicate, Action<QueryableWhereExpressionBuilder<TSource>> buildExpression)
        {
            var builder = new QueryableWhereExpressionBuilder<TSource>(@this, predicate);
            buildExpression(builder);
            return builder.Build();
        }

    }
}