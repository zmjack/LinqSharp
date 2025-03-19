// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Design;

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    public static IEnumerable<TSource> Sort<TSource>(this IEnumerable<TSource> @this, ILocalSorter<TSource> sorter)
    {
        return @this.OrderBy(sorter.Sort());
    }

    public static IEnumerable<TSource> Sort<TSource>(this IEnumerable<TSource> @this, ICoLocalSorter<TSource> sorter)
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
