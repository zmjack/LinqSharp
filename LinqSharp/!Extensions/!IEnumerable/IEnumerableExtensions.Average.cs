// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Design;
using NStandard.Measures;

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    public static TResult Average<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        using (var enumerator = source.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                TResult current = selector(enumerator.Current);
                if (current is null) continue;

                var op_Addition = GetOpAddition<TResult>() ?? throw new InvalidOperationException($"There is no matching op_Addition method for {typeof(TResult).FullName}.");
                var op_Division = GetOpDivision<TResult>() ?? throw new InvalidOperationException($"There is no matching op_Division method for {typeof(TResult).FullName}.");

                TResult sum = current;
                long count = 1;
                while (enumerator.MoveNext())
                {
                    current = selector(enumerator.Current);
                    if (current is not null)
                    {
                        sum = op_Addition(sum, current);
                        count++;
                    }
                }
                return op_Division(sum, count);
            }
        }

        if (default(TResult) is null) return default!;
        else throw new InvalidOperationException("Sequence contains no elements");
    }
    public static TSource Average<TSource>(this IEnumerable<TSource> source) where TSource : ISummable => Average(source, x => x);
    public static TSource? Average<TSource>(this IEnumerable<TSource?> source) where TSource : struct, ISummable => Average(source, x => x);

    public static TSource QAverage<TSource>(this IEnumerable<TSource> source) where TSource : struct, IMeasurable<decimal>
    {
        if (!source.Any()) throw new InvalidOperationException("Sequence contains no elements");

        return new TSource
        {
            Value = source.Average(x => x.Value)
        };
    }

    public static TSource? QAverage<TSource>(this IEnumerable<TSource?> source) where TSource : struct, IMeasurable<decimal>
    {
        if (!source.Any(x => x.HasValue)) return null;

        return new TSource
        {
            Value = source.Where(x => x.HasValue).Average(x => x!.Value.Value)
        };
    }
}
