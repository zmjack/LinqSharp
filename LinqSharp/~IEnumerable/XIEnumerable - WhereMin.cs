using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static IEnumerable<TSource> WhereMin<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            if (source.Any())
            {
                var min = source.Min(selector);
                return source.Where(x => selector(x).Equals(min));
            }
            else return source;
        }
        public static IEnumerable<TSource> WhereMin<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
        {
            if (source.Any())
            {
                var min = source.Min(selector);
                return source.Where(x => selector(x).Equals(min));
            }
            else return source;
        }
        public static IEnumerable<TSource> WhereMin<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
        {
            if (source.Any())
            {
                var min = source.Min(selector);
                return source.Where(x => selector(x).Equals(min));
            }
            else return source;
        }
        public static IEnumerable<TSource> WhereMin<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            if (source.Any())
            {
                var min = source.Min(selector);
                return source.Where(x => selector(x).Equals(min));
            }
            else return source;
        }
        public static IEnumerable<TSource> WhereMin<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source.Any())
            {
                var min = source.Min(selector);
                return source.Where(x => selector(x).Equals(min));
            }
            else return source;
        }
        public static IEnumerable<TSource> WhereMin<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            if (source.Any())
            {
                var min = source.Min(selector);
                return source.Where(x => selector(x).Equals(min));
            }
            else return source;
        }
        public static IEnumerable<TSource> WhereMin<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            if (source.Any())
            {
                var min = source.Min(selector);
                return source.Where(x => selector(x).Equals(min));
            }
            else return source;
        }
        public static IEnumerable<TSource> WhereMin<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            if (source.Any())
            {
                var min = source.Min(selector);
                return source.Where(x => selector(x).Equals(min));
            }
            else return source;
        }
        public static IEnumerable<TSource> WhereMin<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            if (source.Any())
            {
                var min = source.Min(selector);
                return source.Where(x => selector(x).Equals(min));
            }
            else return source;
        }
        public static IEnumerable<TSource> WhereMin<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source.Any())
            {
                var min = source.Min(selector);
                return source.Where(x => selector(x).Equals(min));
            }
            else return source;
        }
        public static IEnumerable<TSource> WhereMin<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source.Any())
            {
                var min = source.Min(selector);
                return source.Where(x => selector(x).Equals(min));
            }
            else return source;
        }

    }
}
