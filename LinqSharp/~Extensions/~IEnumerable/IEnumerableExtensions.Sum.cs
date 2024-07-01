// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Numeric;
using NStandard.Measures;

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    private static TResult Sum<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        using (var enumerator = source.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                TResult current = selector(enumerator.Current);
                if (current is null) continue;

                var op_Addition = GetOpAddition<TResult>() ?? throw new InvalidOperationException($"There is no matching op_Addition method for {typeof(TResult).FullName}.");

                TResult sum = current;
                while (enumerator.MoveNext())
                {
                    current = selector(enumerator.Current);
                    if (current is not null)
                    {
                        sum = op_Addition(sum, current);
                    }
                }
                return sum;
            }
        }
        return default!;
    }
    public static TSource Sum<TSource>(this IEnumerable<TSource> source) where TSource : ISummable => Sum(source, x => x);
    public static TSource? Sum<TSource>(this IEnumerable<TSource?> source) where TSource : struct, ISummable => Sum(source, x => x);

    public static TSource QSum<TSource>(this IEnumerable<TSource> source) where TSource : struct, IMeasurable<decimal>
    {
        if (!source.Any()) return new TSource();

        return new TSource
        {
            Value = source.Sum(x => x.Value)
        };
    }

    public static TSource? QSum<TSource>(this IEnumerable<TSource?> source) where TSource : struct, IMeasurable<decimal>
    {
        if (!source.Any()) return new TSource();

        return new TSource
        {
            Value = source.Where(x => x.HasValue).Sum(x => x!.Value.Value)
        };
    }

}
