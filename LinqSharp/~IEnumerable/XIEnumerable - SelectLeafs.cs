using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static IEnumerable<TSource> SelectLeafs<TSource>(this IEnumerable<TSource> @this, Func<TSource, IEnumerable<TSource>> selector)
        {
            IEnumerable<TSource> RecursiveChildren(TSource node)
            {
                var children = selector(node);
                foreach (var child in children)
                {
                    if (selector(child)?.Any() ?? false)
                    {
                        foreach (var child_ in RecursiveChildren(child))
                            yield return child_;
                    }
                    else yield return child;
                }
            }
            var ret = @this.Select(x => RecursiveChildren(x)).Aggregate((a, b) => a.Concat(b));
            return ret;
        }

    }
}