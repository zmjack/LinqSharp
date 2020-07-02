// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static double AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector, double @default = default(double))
            => source.Any() ? source.Average(selector) : @default;
        public static double AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector, double @default = default(double))
            => source.Any() ? source.Average(selector) : @default;
        public static float AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector, float @default = default(float))
            => source.Any() ? source.Average(selector) : @default;
        public static double AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector, double @default = default(double))
            => source.Any() ? source.Average(selector) : @default;
        public static decimal AverageOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector, decimal @default = default(decimal))
            => source.Any() ? source.Average(selector) : @default;

        public static double AverageOrDefault(this IEnumerable<int> source, double @default = default(double))
            => source.Any() ? source.Average() : @default;
        public static double AverageOrDefault(this IEnumerable<long> source, double @default = default(double))
            => source.Any() ? source.Average() : @default;
        public static float AverageOrDefault(this IEnumerable<float> source, float @default = default(float))
            => source.Any() ? source.Average() : @default;
        public static double AverageOrDefault(this IEnumerable<double> source, double @default = default(double))
            => source.Any() ? source.Average() : @default;
        public static decimal AverageOrDefault(this IEnumerable<decimal> source, decimal @default = default(decimal))
            => source.Any() ? source.Average() : @default;
    }

}
