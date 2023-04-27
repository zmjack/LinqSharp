// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Query;
using NStandard.UnitValues;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
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

            if (default(TResult) is null) return default;
            else throw new InvalidOperationException("Sequence contains no elements");
        }
        public static TSource Average<TSource>(this IEnumerable<TSource> source) where TSource : ISummable => Average(source, x => x);
        public static TSource? Average<TSource>(this IEnumerable<TSource?> source) where TSource : struct, ISummable => Average(source, x => x);

        public static TUnitValue QAverage<TUnitValue>(this IEnumerable<TUnitValue> source) where TUnitValue : struct, IUnitValue, ISummable<TUnitValue>
        {
            if (!source.Any()) throw new InvalidOperationException("Sequence contains no elements");

            var result = new TUnitValue();
            result.QuickAverage(source);

            return result;
        }

        public static TUnitValue? QAverage<TUnitValue>(this IEnumerable<TUnitValue?> source) where TUnitValue : struct, IUnitValue, ISummable<TUnitValue>
        {
            if (!source.Any(x => x.HasValue)) return null;

            var result = new TUnitValue();
            result.QuickAverage(from item in source where item.HasValue select item.Value);

            return result;
        }
    }
}
