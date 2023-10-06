// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace LinqSharp;

public interface IIndexable<TKey, TModel>
{
    IIndexing<TKey, TModel> Indexing { get; set; }
#if NET6_0_OR_GREATER
    IEnumerable<TModel> this[TKey key] => Indexing[key];
#else
    IEnumerable<TModel> this[TKey key] { get; }
#endif
}
