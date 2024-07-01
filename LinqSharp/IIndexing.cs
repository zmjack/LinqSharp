// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

namespace LinqSharp;

public interface IIndexing<TKey, T> : IDictionary<TKey, IReadOnlyCollection<T>>
{
    IEnumerable<T> AllValues { get; }
}
