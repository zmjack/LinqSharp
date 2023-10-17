// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System;
using System.Collections.Generic;

namespace LinqSharp;

public interface IUniqueIndexing<TKey, T> : IDictionary<TKey, Ref<T>>
{
    Ref<T> this[TKey key] { get; }
}
