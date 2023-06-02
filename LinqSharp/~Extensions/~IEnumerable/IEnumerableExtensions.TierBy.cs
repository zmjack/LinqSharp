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
        private static IEnumerable<Layer<TSource>> LayerByCore<TSource>(this TSource[] @this, params Func<TSource, object>[] layerSelectors)
        {
            var span = layerSelectors.Length;
            if (layerSelectors.Length > 1)
            {
                return
                    from g in @this.GroupBy(layerSelectors[0])
                    let elements = g.ToArray()
                    select new Layer<TSource>(span, g.Key, elements, LayerByCore(elements, layerSelectors.Skip(1).ToArray()));
            }
            else
            {
                return
                    from g in @this.GroupBy(layerSelectors[0])
                    let elements = g.ToArray()
                    select new Layer<TSource>(span, g.Key, elements, null);
            }
        }

        public static Layer<TSource> LayerBy<TSource>(this TSource[] @this, params Func<TSource, object>[] layerSelectors)
        {
            if (layerSelectors is null) throw new ArgumentNullException(nameof(layerSelectors));
            if (!layerSelectors.Any()) throw new ArgumentException("The layer selectors can not be empty.", nameof(layerSelectors));

            var span = layerSelectors.Length + 1;
            return new Layer<TSource>(span, null, @this, LayerByCore(@this, layerSelectors));
        }
    }
}
