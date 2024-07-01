// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard.Infrastructure;

namespace LinqSharp;

public interface IQueryLayer<TSource> : IEnumerable<TSource>, IGrouping<object?, TSource>
{
    int Span { get; }
    IEnumerable<IQueryLayer<TSource>> SubLayers { get; }

    IEnumerable<ChainIterator<IQueryLayer<TSource>>> AsChain();
}
