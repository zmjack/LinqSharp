using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static IEnumerable<TSource> SelectWhile<TSource>(this IEnumerable<TSource> @this, Func<TSource, IEnumerable<TSource>> selector, Func<IEnumerable<TSource>, bool> @while)
        {
            IEnumerable<TSource> RecursiveChildren(TSource node)
            {
                var selectNode = selector(node);
                if (@while(selectNode))
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

            var ret = @this
                .Select(x => RecursiveChildren(x))
                .Aggregate((a, b) => a.Concat(b));
            return ret;
        }

    }
}