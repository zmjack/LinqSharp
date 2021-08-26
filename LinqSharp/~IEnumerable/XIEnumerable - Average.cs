// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static TResult Average<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    TResult current = selector(enumerator.Current);
                    if (current is null) continue;

                    var op_Addition = GetOpAddition<TResult>();
                    var op_Division = GetOpDivision<TResult>();

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
        public static TSource Average<TSource>(this IEnumerable<TSource> source) => Average(source, x => x);

    }
}
