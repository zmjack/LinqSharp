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
        var expression = sorter.Sort();
        if (expression is null) return @this;
        else return @this.OrderBy(expression);
    }

    public static IQueryable<TSource> Sort<TSource>(this IQueryable<TSource> @this, ICoQuerySorter<TSource> sorter)
    {
        var enumerator = sorter.Sort().GetEnumerator();
        if (enumerator.MoveNext())
        {
            var ordered = @this.OrderBy(enumerator.Current);
            while (enumerator.MoveNext())
            {
                ordered = ordered.ThenBy(enumerator.Current);
            }
            return ordered;
        }
        return @this;
    }
}
