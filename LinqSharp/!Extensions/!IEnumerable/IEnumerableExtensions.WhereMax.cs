// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    public static IEnumerable<TSource> WhereMax<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
    {
        if (source.Any())
        {
            var max = source.Max(selector);
            return source.Where(x => selector(x).Equals(max));
        }
        else return source;
    }
    public static IEnumerable<TSource> WhereMax<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
    {
        if (source.Any())
        {
            var max = source.Max(selector);
            return source.Where(x => selector(x).Equals(max));
        }
        else return source;
    }
    public static IEnumerable<TSource> WhereMax<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
    {
        if (source.Any())
        {
            var max = source.Max(selector);
            return source.Where(x => selector(x).Equals(max));
        }
        else return source;
    }
    public static IEnumerable<TSource> WhereMax<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
    {
        if (source.Any())
        {
            var max = source.Max(selector);
            return source.Where(x => selector(x).Equals(max));
        }
        else return source;
    }
    public static IEnumerable<TSource> WhereMax<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
    {
        if (source.Any())
        {
            var max = source.Max(selector);
            return source.Where(x => selector(x).Equals(max));
        }
        else return source;
    }
    public static IEnumerable<TSource> WhereMax<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
    {
        if (source.Any())
        {
            var max = source.Max(selector);
            return source.Where(x => selector(x).Equals(max));
        }
        else return source;
    }
    public static IEnumerable<TSource> WhereMax<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
    {
        if (source.Any())
        {
            var max = source.Max(selector);
            return source.Where(x => selector(x).Equals(max));
        }
        else return source;
    }
    public static IEnumerable<TSource> WhereMax<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
    {
        if (source.Any())
        {
            var max = source.Max(selector);
            return source.Where(x => selector(x).Equals(max));
        }
        else return source;
    }
    public static IEnumerable<TSource> WhereMax<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
    {
        if (source.Any())
        {
            var max = source.Max(selector);
            return source.Where(x => selector(x).Equals(max));
        }
        else return source;
    }
    public static IEnumerable<TSource> WhereMax<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
    {
        if (source.Any())
        {
            var max = source.Max(selector);
            return source.Where(x => selector(x).Equals(max));
        }
        else return source;
    }
    public static IEnumerable<TSource> WhereMax<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        if (source.Any())
        {
            var max = source.Max(selector);
            if (max is null)
            {
                return source.Where(x => selector(x) is null);
            }
            else
            {
                return source.Where(x => max.Equals(selector(x)));
            }
        }
        else return source;
    }
}
