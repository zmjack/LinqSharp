using System;
using System.Linq;
using System.Linq.Expressions;

namespace NLinq
{
    public static partial class XIQueryable
    {
        public static double AverageOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int>> selector, double @default = default)
            => source.Any() ? source.Average(selector) : @default;
        public static double AverageOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long>> selector, double @default = default)
            => source.Any() ? source.Average(selector) : @default;
        public static float AverageOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float>> selector, float @default = default)
            => source.Any() ? source.Average(selector) : @default;
        public static double AverageOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double>> selector, double @default = default)
            => source.Any() ? source.Average(selector) : @default;
        public static decimal AverageOrDefault<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, decimal @default = default)
            => source.Any() ? source.Average(selector) : @default;

        public static double AverageOrDefault(this IQueryable<int> source, double @default = default)
            => source.Any() ? source.Average() : @default;
        public static double AverageOrDefault(this IQueryable<long> source, double @default = default)
            => source.Any() ? source.Average() : @default;
        public static float AverageOrDefault(this IQueryable<float> source, float @default = default)
            => source.Any() ? source.Average() : @default;
        public static double AverageOrDefault(this IQueryable<double> source, double @default = default)
            => source.Any() ? source.Average() : @default;
        public static decimal AverageOrDefault(this IQueryable<decimal> source, decimal @default = default)
            => source.Any() ? source.Average() : @default;

    }
}
