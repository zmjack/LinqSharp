// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Design;

namespace LinqSharp;

public static partial class IQueryableExtensions
{
    public static IQueryable<TSource> Sort<TSource>(this IQueryable<TSource> @this, IQuerySorter<TSource> sorter)
    {
        var rule = sorter.Sort();
        if (!rule.HasValue) return @this;

        var value = rule.Value;
        return value.Descending
            ? @this.OrderByDescending(value.Selector)
            : @this.OrderBy(value.Selector);
    }
    public static IQueryable<TSource> Sort<TSource>(this IQueryable<TSource> @this, ICoQuerySorter<TSource> sorter)
    {
        var enumerator = sorter.Sort().GetEnumerator();
        if (enumerator.MoveNext())
        {
            var rule = enumerator.Current;
            var ordered = rule.Descending
                ? @this.OrderByDescending(rule.Selector)
                : @this.OrderBy(rule.Selector);

            while (enumerator.MoveNext())
            {
                rule = enumerator.Current;
                ordered = rule.Descending
                    ? ordered.ThenByDescending(rule.Selector)
                    : ordered.ThenBy(rule.Selector);
            }
            return ordered;
        }
        return @this;
    }
}
