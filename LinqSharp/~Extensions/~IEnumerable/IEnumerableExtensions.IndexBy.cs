// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Query;
using System;
using System.Collections.Generic;

namespace LinqSharp
{
    public static partial class IEnumerableExtensions
    {
        public static IIndexing<TKey, T> IndexBy<T, TKey>(this IEnumerable<T> @this, Func<T, TKey> selector)
        {
            var indexing = new Indexing<TKey, T>();
            foreach (var item in @this)
            {
                var key = selector(item);
                if (!indexing.ContainsKey(key))
                {
                    indexing[key] = new List<T>();
                }
                (indexing[key] as List<T>).Add(item);
            }
            return indexing;
        }

        public static IUniqueIndexing<TKey, T> UniqueIndexBy<T, TKey>(this IEnumerable<T> @this, Func<T, TKey> selector)
        {
            var indexing = new UniqueIndexing<TKey, T>();
            foreach (var item in @this)
            {
                var key = selector(item);
                if (!indexing.ContainsKey(key))
                {
                    indexing[key] = new AnyNullable<T>
                    {
                        HasValue = true,
                        Value = item,
                    };
                }
                else
                {
                    throw new InvalidOperationException("Sequence contains more than one matching element.");
                }
            }
            return indexing;
        }
    }
}
