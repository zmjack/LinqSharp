// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
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
        private readonly IEnumerable<Layer<TSource>> _subTiers;

        internal Layer(int span, object key, TSource[] elements, IEnumerable<Layer<TSource>> subTiers)
        {
            Span = span;
            Key = key;
            _elements = elements;
            _subTiers = subTiers;
        }

        public IEnumerable<Layer<TSource>> NestedTiers
        {
            get
            {
                if (_subTiers is null) throw new InvalidOperationException("No sub tiers.");

                foreach (Layer<TSource> tier in _subTiers)
                {
                    yield return tier;
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

        public IEnumerable<Any.SharedChainItem<Layer<TSource>>> AsChian()
        {
            var tier2tiers = (Layer<TSource> x) => x.NestedTiers;
            var tierSelectors = new Func<Layer<TSource>, IEnumerable<Layer<TSource>>>[Span - 1];
            for (int i = 0; i < tierSelectors.Length; i++)
            {
                tierSelectors[i] = tier2tiers;
            }

            return Any.Chain(this, tierSelectors);
        }

    }
}
