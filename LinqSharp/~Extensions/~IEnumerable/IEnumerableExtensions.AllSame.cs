using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    public static bool AllSame<TSource>(this IEnumerable<TSource> enumerable)
    {
        if (!enumerable.Any()) return true;

        var first = enumerable.First();
        foreach (var element in enumerable)
        {
            if (!element.Equals(first)) return false;
        }
        return true;
    }

    public static bool AllSame<TSource, TResult>(this IEnumerable<TSource> enumerable, Func<TSource, TResult> selector)
    {
        if (!enumerable.Any()) return true;

        var first = enumerable.First();
        var firstValue = selector(first);

        foreach (var element in enumerable)
        {
            var selectValue = selector(element);
            if (!selectValue.Equals(firstValue)) return false;
        }
        return true;
    }

}
