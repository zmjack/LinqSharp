// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static IEnumerable<TSource> SelectMore<TSource>(this IEnumerable<TSource> @this, Func<TSource, IEnumerable<TSource>> childrenSelector)
        {
            return SelectMore(@this, childrenSelector, null);
        }

        public static IEnumerable<TSource> SelectMore<TSource>(this IEnumerable<TSource> @this, Func<TSource, IEnumerable<TSource>> childrenSelector, Func<TSource, bool> predicate)
        {
            IEnumerable<TSource> RecursiveChildren(TSource node)
            {
                if (predicate?.Invoke(node) ?? true)
                    yield return node;

                var selectNode = childrenSelector(node);
                if (selectNode?.Any() ?? false)
                {
                    var children = selectNode.SelectMany(x => RecursiveChildren(x));
                    foreach (var child in children)
                    {
                        if (predicate?.Invoke(node) ?? true)
                            yield return child;
                    }
                }
            }

            var ret = @this.SelectMany(x => RecursiveChildren(x));
            return ret;
        }

    }
}