using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static int MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector, int @default = default(int))
            => source.Any() ? source.Min(selector) : @default;
        public static long MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector, long @default = default(long))
            => source.Any() ? source.Min(selector) : @default;
        public static float MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector, float @default = default(float))
            => source.Any() ? source.Min(selector) : @default;
        public static double MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double @default = default(double))
            => source.Any() ? source.Min(selector) : @default;
        public static decimal MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector, decimal @default = default(decimal))
            => source.Any() ? source.Min(selector) : @default;
        public static int? MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector, int? @default = default(int?))
            => source.Any() ? source.Min(selector) : @default;
        public static long? MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector, long? @default = default(long?))
            => source.Any() ? source.Min(selector) : @default;
        public static float? MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector, float? @default = default(float?))
            => source.Any() ? source.Min(selector) : @default;
        public static double? MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector, double? @default = default(double?))
            => source.Any() ? source.Min(selector) : @default;
        public static decimal? MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector, decimal? @default = default(decimal?))
            => source.Any() ? source.Min(selector) : @default;
        public static TResult MinOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult @default = default(TResult))
            => source.Any() ? source.Min(selector) : @default;

        public static int MinOrDefault(this IEnumerable<int> source, int @default = default(int))
            => source.Any() ? source.Min() : @default;
        public static long MinOrDefault(this IEnumerable<long> source, long @default = default(long))
            => source.Any() ? source.Min() : @default;
        public static float MinOrDefault(this IEnumerable<float> source, float @default = default(float))
            => source.Any() ? source.Min() : @default;
        public static double MinOrDefault(this IEnumerable<double> source, double @default = default(double))
            => source.Any() ? source.Min() : @default;
        public static decimal MinOrDefault(this IEnumerable<decimal> source, decimal @default = default(decimal))
            => source.Any() ? source.Min() : @default;
        public static int? MinOrDefault(this IEnumerable<int?> source, int? @default = default(int?))
            => source.Any() ? source.Min() : @default;
        public static long? MinOrDefault(this IEnumerable<long?> source, long? @default = default(long?))
            => source.Any() ? source.Min() : @default;
        public static float? MinOrDefault(this IEnumerable<float?> source, float? @default = default(float?))
            => source.Any() ? source.Min() : @default;
        public static double? MinOrDefault(this IEnumerable<double?> source, double? @default = default(double?))
            => source.Any() ? source.Min() : @default;
        public static decimal? MinOrDefault(this IEnumerable<decimal?> source, decimal? @default = default(decimal?))
            => source.Any() ? source.Min() : @default;
        public static TSource MinOrDefault<TSource>(this IEnumerable<TSource> source, TSource @default = default(TSource))
            => source.Any() ? source.Min() : @default;
    }

}
