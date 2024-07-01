// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Numeric;
using NStandard.Measures;

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    public static double AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector, double @default = default) => source.Any() ? source.Average(selector) : @default;
    public static double AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector, long @default = default) => source.Any() ? source.Average(selector) : @default;
    public static float AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector, float @default = default) => source.Any() ? source.Average(selector) : @default;
    public static double AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double @default = default) => source.Any() ? source.Average(selector) : @default;
    public static decimal AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector, decimal @default = default) => source.Any() ? source.Average(selector) : @default;

    public static double AverageOrDefault(this IEnumerable<int> source, double @default = default) => source.Any() ? source.Average() : @default;
    public static double AverageOrDefault(this IEnumerable<long> source, double @default = default) => source.Any() ? source.Average() : @default;
    public static float AverageOrDefault(this IEnumerable<float> source, float @default = default) => source.Any() ? source.Average() : @default;
    public static double AverageOrDefault(this IEnumerable<double> source, double @default = default) => source.Any() ? source.Average() : @default;
    public static decimal AverageOrDefault(this IEnumerable<decimal> source, decimal @default = default) => source.Any() ? source.Average() : @default;

    public static double? AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector, double @default = default) => source.Any() ? source.Average(selector) : @default;
    public static double? AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector, long @default = default) => source.Any() ? source.Average(selector) : @default;
    public static float? AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector, float @default = default) => source.Any() ? source.Average(selector) : @default;
    public static double? AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector, double @default = default) => source.Any() ? source.Average(selector) : @default;
    public static decimal? AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector, decimal @default = default) => source.Any() ? source.Average(selector) : @default;

    public static double? AverageOrDefault(this IEnumerable<int?> source, double @default = default) => source.Any() ? source.Average() : @default;
    public static double? AverageOrDefault(this IEnumerable<long?> source, double @default = default) => source.Any() ? source.Average() : @default;
    public static float? AverageOrDefault(this IEnumerable<float?> source, float @default = default) => source.Any() ? source.Average() : @default;
    public static double? AverageOrDefault(this IEnumerable<double?> source, double @default = default) => source.Any() ? source.Average() : @default;
    public static decimal? AverageOrDefault(this IEnumerable<decimal?> source, decimal @default = default) => source.Any() ? source.Average() : @default;

    public static TResult? AverageOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult? @default = default) where TResult : ISummable => source.Any() ? source.Average(selector) : @default;
    public static TSource? AverageOrDefault<TSource>(this IEnumerable<TSource> source, TSource? @default = default) where TSource : ISummable => source.Any() ? source.Average() : @default;

    public static TSource? AverageOrDefault<TSource>(this IEnumerable<TSource?> source, TSource? @default = default) where TSource : struct, ISummable => source.Any() ? source.Average() : @default;
    public static TSource QAverageOrDefault<TSource>(this IEnumerable<TSource> source, TSource @default = default) where TSource : struct, IMeasurable<decimal>
    {
        if (!source.Any()) return @default;

        return new TSource
        {
            Value = source.Average(x => x.Value)
        };
    }

    public static TSource? QAverageOrDefault<TSource>(this IEnumerable<TSource?> source, TSource? @default = default) where TSource : struct, IMeasurable<decimal>
    {
        if (!source.Any(x => x.HasValue)) return @default;

        return new TSource
        {
            Value = source.Where(x => x.HasValue).Average(x => x!.Value.Value)
        };
    }

}
