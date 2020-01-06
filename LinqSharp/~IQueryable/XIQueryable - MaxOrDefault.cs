using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        public static TResult MaxOrDefault<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, TResult @default = default(TResult))
            => source.Any() ? source.Max(selector) : @default;

        public static TSource MaxOrDefault<TSource>(this IQueryable<TSource> source, TSource @default = default(TSource))
            => source.Any() ? source.Max() : @default;
    }
}
