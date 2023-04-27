// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LinqSharp.Query
{
    public class Tier<TSource> : IEnumerable<TSource>, IGrouping<object, TSource>
    {
        public int Span { get; }
        public object Key { get; }

        private readonly TSource[] _elements;
        private readonly IEnumerable<Tier<TSource>> _subTiers;

        internal Tier(int span, object key, TSource[] elements, IEnumerable<Tier<TSource>> subTiers)
        {
            Span = span;
            Key = key;
            _elements = elements;
            _subTiers = subTiers;
        }

        public IEnumerable<Tier<TSource>> SubTiers
        {
            get
            {
                if (_subTiers is null) throw new InvalidOperationException("No sub tiers.");

                foreach (Tier<TSource> tier in _subTiers)
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
    }

}
