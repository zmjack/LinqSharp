// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;

namespace LinqSharp;

public interface IUniqueIndexable<TKey, TModel>
{
    IUniqueIndexing<TKey, TModel> Indexing { get; set; }
#if NET6_0_OR_GREATER
    Tuple<TModel> this[TKey key] => Indexing[key];
#else 
    Tuple<TModel> this[TKey key] { get; }
#endif
}
