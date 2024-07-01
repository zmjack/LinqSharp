// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using NStandard.Infrastructure;
using System.Collections;

namespace LinqSharp.Layer;

public class QueryLayer<TSource> : IQueryLayer<TSource>
{
    public int Span { get; }
    public object? Key { get; }

    private readonly TSource[] _elements;
    private readonly IEnumerable<IQueryLayer<TSource>>? _subLayers;

    internal QueryLayer(int span, object? key, TSource[] elements, IEnumerable<IQueryLayer<TSource>>? nestedLayers)
    {
        Span = span;
        Key = key;
        _elements = elements;
        _subLayers = nestedLayers;
    }

    public IEnumerable<IQueryLayer<TSource>> SubLayers
    {
        get
        {
            if (_subLayers is null) throw new InvalidOperationException("No sub layers.");

            foreach (var layer in _subLayers)
            {
                yield return layer;
            }
        }
    }

    public IEnumerator<TSource> GetEnumerator()
    {
        foreach (var element in _elements)
        {
            yield return element;
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _elements.GetEnumerator();
    }

    public IEnumerable<ChainIterator<IQueryLayer<TSource>>> AsChain()
    {
        static IEnumerable<IQueryLayer<TSource>> layer2layers(IQueryLayer<TSource> x) => x.SubLayers;

        var selectors = new Func<IQueryLayer<TSource>, IEnumerable<IQueryLayer<TSource>>>[Span - 1];
        for (int i = 0; i < selectors.Length; i++)
        {
            selectors[i] = layer2layers;
        }

        return Any.Chain(this, selectors);
    }

}
