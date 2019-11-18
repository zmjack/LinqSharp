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
        public static IQueryable<TSource> WhereDynamic<TSource>(this IQueryable<TSource> @this,
            Action<DynamicExpressionBuilder<TSource>> build)
        {
            var builer = new DynamicExpressionBuilder<TSource>().Then(x => build(x));
            return @this.Where(builer.Lambda);
        }

    }
}