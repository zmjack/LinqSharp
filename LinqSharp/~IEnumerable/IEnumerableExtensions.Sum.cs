// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard.UnitValues;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class IEnumerableExtensions
    {
        public static TResult Sum<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    TResult current = selector(enumerator.Current);
                    if (current is null) continue;

                    var op_Addition = GetOpAddition<TResult>();
                    if (op_Addition is null) throw new InvalidOperationException($"There is no matching op_Addition method for {typeof(TResult).FullName}.");

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
            return default;
        }
        public static TSource Sum<TSource>(this IEnumerable<TSource> source) => Sum(source, x => x);

        public static TSource QSum<TSource>(this IEnumerable<TSource> source) where TSource : struct, IUnitValue, ISummable<TSource>
        {
            var result = new TSource();

            if (!source.Any()) return result;
            else result.QuickSum(source);

            return result;
        }

        public static TSource? QSum<TSource>(this IEnumerable<TSource?> source) where TSource : struct, IUnitValue, ISummable<TSource>
        {
            var result = new TSource();

            if (!source.Any()) return result;
            else result.QuickSum(from item in source where item.HasValue select item.Value);

            return result;
        }

    }

}
