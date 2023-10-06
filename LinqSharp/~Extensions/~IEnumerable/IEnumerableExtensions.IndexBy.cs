// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Index;
using System;
using System.Collections.Generic;

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    public static IIndexing<TKey, T> IndexBy<T, TKey>(this IEnumerable<T> @this, Func<T, TKey> selector)
    {
        return new Indexing<TKey, T>(@this, selector);
    }

    public static IUniqueIndexing<TKey, T> UniqueIndexBy<T, TKey>(this IEnumerable<T> @this, Func<T, TKey> selector)
    {
        return new UniqueIndexing<TKey, T>(@this, selector);
    }
}
