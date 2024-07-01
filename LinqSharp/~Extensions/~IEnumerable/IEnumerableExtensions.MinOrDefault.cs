// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    public static int MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector, int @default = default) => source.Any() ? source.Min(selector) : @default;
    public static long MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector, long @default = default) => source.Any() ? source.Min(selector) : @default;
    public static float MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector, float @default = default) => source.Any() ? source.Min(selector) : @default;
    public static double MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double @default = default) => source.Any() ? source.Min(selector) : @default;
    public static decimal MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector, decimal @default = default) => source.Any() ? source.Min(selector) : @default;

    public static int MinOrDefault(this IEnumerable<int> source, int @default = default) => source.Any() ? source.Min() : @default;
    public static long MinOrDefault(this IEnumerable<long> source, long @default = default) => source.Any() ? source.Min() : @default;
    public static float MinOrDefault(this IEnumerable<float> source, float @default = default) => source.Any() ? source.Min() : @default;
    public static double MinOrDefault(this IEnumerable<double> source, double @default = default) => source.Any() ? source.Min() : @default;
    public static decimal MinOrDefault(this IEnumerable<decimal> source, decimal @default = default) => source.Any() ? source.Min() : @default;

    public static int? MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector, int? @default = default) => source.Any() ? source.Min(selector) : @default;
    public static long? MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector, long? @default = default) => source.Any() ? source.Min(selector) : @default;
    public static float? MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector, float? @default = default) => source.Any() ? source.Min(selector) : @default;
    public static double? MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector, double? @default = default) => source.Any() ? source.Min(selector) : @default;
    public static decimal? MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector, decimal? @default = default) => source.Any() ? source.Min(selector) : @default;

    public static int? MinOrDefault(this IEnumerable<int?> source, int? @default = default) => source.Any() ? source.Min() : @default;
    public static long? MinOrDefault(this IEnumerable<long?> source, long? @default = default) => source.Any() ? source.Min() : @default;
    public static float? MinOrDefault(this IEnumerable<float?> source, float? @default = default) => source.Any() ? source.Min() : @default;
    public static double? MinOrDefault(this IEnumerable<double?> source, double? @default = default) => source.Any() ? source.Min() : @default;
    public static decimal? MinOrDefault(this IEnumerable<decimal?> source, decimal? @default = default) => source.Any() ? source.Min() : @default;

    public static TResult? MinOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult? @default = default) => source.Any() ? source.Min(selector) : @default;
    public static TSource? MinOrDefault<TSource>(this IEnumerable<TSource> source, TSource? @default = default) => source.Any() ? source.Min() : @default;

}
