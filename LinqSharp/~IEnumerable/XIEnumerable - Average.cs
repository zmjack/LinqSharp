// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System;
using System.Collections.Generic;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static TResult Average<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            var op_Addition = GetOpAddition<TResult>();
            if (op_Addition is null) throw new InvalidOperationException($"There is no matching op_Addition method for {typeof(TResult).FullName}.");

            var op_Division = GetOpDivision<TResult>();
            if (op_Division is null) throw new InvalidOperationException($"There is no matching op_Division method for {typeof(TResult).FullName}.");

            var count = 0;
            TResult sum = default;
            foreach (var pair in source.AsKvPairs())
            {
                var value = selector(pair.Value);
                if (value is null) continue;

                if (pair.Key == 0) sum = value;
                else sum = (TResult)op_Addition.Invoke(null, new object[] { sum, value });
                count++;
            }

            if (count == 0)
            {
                var type = typeof(TResult);
                if (type.IsClass || type.IsNullable()) return default;
                else throw new InvalidOperationException("Sequence contains no elements");
            }
            else return (TResult)op_Division.Invoke(null, new object[] { sum, count });
        }
        public static TSource Average<TSource>(this IEnumerable<TSource> source) => Average(source, x => x);

    }
}
