using NStandard;
using System;
using System.Linq;

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