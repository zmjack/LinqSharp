// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using NStandard.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp.Query
{
    public class Layer<TSource> : IEnumerable<TSource>, IGrouping<object, TSource>
    {
        public int Span { get; }
        public object Key { get; }

        private readonly TSource[] _elements;
        private readonly IEnumerable<Layer<TSource>> _nestedLayers;

        internal Layer(int span, object key, TSource[] elements, IEnumerable<Layer<TSource>> nestedLayers)
        {
            Span = span;
            Key = key;
            _elements = elements;
            _nestedLayers = nestedLayers;
        }

        public IEnumerable<Layer<TSource>> NestedLayers
        {
            get
            {
                if (_nestedLayers is null) throw new InvalidOperationException("No nested layers.");

                foreach (Layer<TSource> layer in _nestedLayers)
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

        public IEnumerable<ChainIterator<Layer<TSource>>> AsChain()
        {
            var layer2layers = (Layer<TSource> x) => x.NestedLayers;
            var selectors = new Func<Layer<TSource>, IEnumerable<Layer<TSource>>>[Span - 1];
            for (int i = 0; i < selectors.Length; i++)
            {
                selectors[i] = layer2layers;
            }

            return Any.Chain(this, selectors);
        }

    }
}
