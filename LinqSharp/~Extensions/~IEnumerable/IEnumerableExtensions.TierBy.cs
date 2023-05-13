// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LinqSharp
{
    public static partial class IEnumerableExtensions
    {
        private static IEnumerable<Layer<TSource>> LayerByCore<TSource>(this TSource[] @this, params Func<TSource, object>[] tierSelectors)
        {
            var span = tierSelectors.Length;
            if (tierSelectors.Length > 1)
            {
                return
                    from g in @this.GroupBy(tierSelectors[0])
                    let elements = g.ToArray()
                    select new Layer<TSource>(span, g.Key, elements, LayerByCore(elements, tierSelectors.Skip(1).ToArray()));
            }
            else
            {
                return
                    from g in @this.GroupBy(tierSelectors[0])
                    let elements = g.ToArray()
                    select new Layer<TSource>(span, g.Key, elements, null);
            }
        }

        public static Layer<TSource> LayerBy<TSource>(this TSource[] @this, params Func<TSource, object>[] tierSelectors)
        {
            if (tierSelectors is null) throw new ArgumentNullException(nameof(tierSelectors));
            if (!tierSelectors.Any()) throw new ArgumentException("The tier selectors can not be empty.", nameof(tierSelectors));

            var span = tierSelectors.Length + 1;
            return new Layer<TSource>(span, null, @this, LayerByCore(@this, tierSelectors));
        }
    }
}
