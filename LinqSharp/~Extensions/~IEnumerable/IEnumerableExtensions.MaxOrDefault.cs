// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    public static int MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector, int @default = default) => source.Any() ? source.Max(selector) : @default;
    public static long MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector, long @default = default) => source.Any() ? source.Max(selector) : @default;
    public static float MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector, float @default = default) => source.Any() ? source.Max(selector) : @default;
    public static double MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double @default = default) => source.Any() ? source.Max(selector) : @default;
    public static decimal MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector, decimal @default = default) => source.Any() ? source.Max(selector) : @default;

    public static int? MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector, int? @default = default) => source.Any() ? source.Max(selector) : @default;
    public static long? MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector, long? @default = default) => source.Any() ? source.Max(selector) : @default;
    public static float? MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector, float? @default = default) => source.Any() ? source.Max(selector) : @default;
    public static double? MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector, double? @default = default) => source.Any() ? source.Max(selector) : @default;
    public static decimal? MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector, decimal? @default = default) => source.Any() ? source.Max(selector) : @default;

    public static int MaxOrDefault(this IEnumerable<int> source, int @default = default) => source.Any() ? source.Max() : @default;
    public static long MaxOrDefault(this IEnumerable<long> source, long @default = default) => source.Any() ? source.Max() : @default;
    public static float MaxOrDefault(this IEnumerable<float> source, float @default = default) => source.Any() ? source.Max() : @default;
    public static double MaxOrDefault(this IEnumerable<double> source, double @default = default) => source.Any() ? source.Max() : @default;
    public static decimal MaxOrDefault(this IEnumerable<decimal> source, decimal @default = default) => source.Any() ? source.Max() : @default;

    public static int? MaxOrDefault(this IEnumerable<int?> source, int? @default = default) => source.Any() ? source.Max() : @default;
    public static long? MaxOrDefault(this IEnumerable<long?> source, long? @default = default) => source.Any() ? source.Max() : @default;
    public static float? MaxOrDefault(this IEnumerable<float?> source, float? @default = default) => source.Any() ? source.Max() : @default;
    public static double? MaxOrDefault(this IEnumerable<double?> source, double? @default = default) => source.Any() ? source.Max() : @default;
    public static decimal? MaxOrDefault(this IEnumerable<decimal?> source, decimal? @default = default) => source.Any() ? source.Max() : @default;

    public static TResult MaxOrDefault<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, TResult @default = default) => source.Any() ? source.Max(selector) : @default;
    public static TSource MaxOrDefault<TSource>(this IEnumerable<TSource> source, TSource @default = default) => source.Any() ? source.Max() : @default;

}
