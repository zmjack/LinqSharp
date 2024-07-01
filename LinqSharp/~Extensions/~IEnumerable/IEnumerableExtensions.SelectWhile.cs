// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    public static IEnumerable<TSource> SelectWhile<TSource>(this IEnumerable<TSource> @this, Func<TSource, IEnumerable<TSource>> childrenSelector, Func<TSource, bool> predicate)
    {
        IEnumerable<TSource> RecursiveChildren(TSource node)
        {
            var selectNode = childrenSelector(node);
            if (predicate(node))
            {
                yield return node;

                if (selectNode?.Any() ?? false)
                {
                    var children = selectNode.SelectMany(x => RecursiveChildren(x));
                    foreach (var child in children)
                        yield return child;
                }
            }
        }

        var ret = @this.SelectMany(x => RecursiveChildren(x));
        return ret;
    }

}