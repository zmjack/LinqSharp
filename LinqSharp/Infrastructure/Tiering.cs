// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp.Infrastructure
{
    public class Tiering<TTier, TElement> : ITiering<TElement>, IGrouping<object, TElement>
    {
        public int Span { get; }
        public bool Final => Span == 0;
        public object Key { get; }

        private readonly TElement[] _elements;
        private readonly IEnumerable<TTier> _subTiers;

        internal Tiering(int tier, object key, TElement[] elements, IEnumerable<TTier> subTiers)
        {
            Span = tier;
            Key = key;
            _elements = elements;
            _subTiers = subTiers;
        }

        public IEnumerable<ITiering<TElement>> SubTiers
        {
            get
            {
                if (_subTiers is null) throw new InvalidOperationException("No sub tiers.");

                foreach (ITiering<TElement> tier in _subTiers)
                {
                    yield return tier;
                }
            }
        }

        public IEnumerator<TElement> GetEnumerator()
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
    }

}
