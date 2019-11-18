using Dawnx.Utilities;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NLinq
{
    public static partial class XIQueryable
    {
        public static WhereExpressionBuilder<TSource> Begin<TSource>(this IQueryable<TSource> @this)
        {
            return new WhereExpressionBuilder<TSource>(@this);
        }

        public static WhereExpressionBuilder<TSource> Begin<TSource>(this IQueryable<TSource> @this, Expression<Func<TSource, bool>> predicate)
        {
            return new WhereExpressionBuilder<TSource>(@this, predicate);
        }

    }
}