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
            var collection = new Indexing<TKey, T>();
            foreach (var item in @this)
            {
                var key = selector(item);
                if (!collection.ContainsKey(key))
                {
                    collection[key] = new List<T>();
                }
                (collection[key] as List<T>).Add(item);
            }
            return collection;
        }
    }
}
