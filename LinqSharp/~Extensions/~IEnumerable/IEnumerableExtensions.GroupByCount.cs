// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    /// <summary>
    /// Groups the elements of a sequence according to the count of group capacity.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="this"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    [Obsolete("Use Chunk instead.")]
    public static IEnumerable<IGrouping<int, TSource>> GroupByCount<TSource>(this IEnumerable<TSource> @this, int size)
    {
        IEnumerable<StructTuple<int, TSource>> GetEnumerable()
        {
            foreach (var (index, sources) in @this.Chunk(size).AsIndexValuePairs())
            {
                foreach (var source in sources)
                {
                    yield return StructTuple.Create(index, source);
                }
            }
        }

        return GetEnumerable().GroupBy(x => x.Item1, x => x.Item2);
    }

    /// <summary>
    /// Groups the elements of a sequence according to the count of group capacity.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="this"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    [Obsolete("Plan to remove, submit an issue if necessary.")]
    public static IEnumerable<IGrouping<int, TSource>> GroupByCount<TSource>(this IEnumerable<TSource> @this, int size, PadDirection padDirection)
    {
        switch (padDirection)
        {
            case PadDirection.Default:
            case PadDirection.Right:
                return @this
                    .Select((v, i) => new { Key = i, Value = v })
                    .GroupBy(x => x.Key / size, x => x.Value);

            case PadDirection.Left:
                var count = @this.Count();
                return @this
                    .Select((v, i) => new { Key = i, Value = v })
                    .GroupBy(x => (x.Key + (size - count % size)) / size, x => x.Value);

            default: throw new NotSupportedException();
        }
    }

#if NET6_0_OR_GREATER
#else
    public static IEnumerable<TSource[]> Chunk<TSource>(this IEnumerable<TSource> @this, int size)
    {
        if (@this == null)
        {
            throw new ArgumentNullException(nameof(@this));
        }
        if (size < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(size));
        }
        return ChunkIterator(@this, size);
    }

    private static IEnumerable<TSource[]> ChunkIterator<TSource>(IEnumerable<TSource> source, int size)
    {
        using IEnumerator<TSource> e = source.GetEnumerator();
        if (!e.MoveNext())
        {
            yield break;
        }

        int i;
        do
        {
            TSource[] array = new TSource[size];
            array[0] = e.Current;
            i = 1;
            for (; i < size; i++)
            {
                if (!e.MoveNext())
                {
                    break;
                }
                array[i] = e.Current;
            }

            if (i != array.Length)
            {
                Array.Resize(ref array, i);
            }
            yield return array;
        }
        while (i >= size && e.MoveNext());
    }
#endif

}
