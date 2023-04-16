// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class IEnumerableExtensions
    {
        public static IEnumerable<ITiering<TSource>> TierBy<TSource>(this TSource[] @this, params Func<TSource, object>[] tierSelectors)
        {
            if (tierSelectors is null) throw new ArgumentNullException(nameof(tierSelectors));
            if (!tierSelectors.Any()) throw new ArgumentException("The tier selectors can not be empty.", nameof(tierSelectors));

            var tier = tierSelectors.Length;
            if (tierSelectors.Length > 1)
            {
                return
                    from g in @this.GroupBy(tierSelectors[0])
                    let elements = g.ToArray()
                    select new Tiering<ITiering<TSource>, TSource>(tier, g.Key, elements, TierBy(elements, tierSelectors.Skip(1).ToArray()));
            }
            else
            {
                return
                    from g in @this.GroupBy(tierSelectors[0])
                    let elements = g.ToArray()
                    select new Tiering<TSource, TSource>(tier, g.Key, elements, null);
            }
        }
    }
}
