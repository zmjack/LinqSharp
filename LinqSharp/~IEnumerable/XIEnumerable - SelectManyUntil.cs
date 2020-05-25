using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static IEnumerable<TSource> SelectManyUntil<TSource>(this IEnumerable<TSource> @this, Func<TSource, IEnumerable<TSource>> selector, Func<IEnumerable<TSource>, bool> until)
        {
            IEnumerable<TSource> RecursiveChildren(TSource node)
            {
                var children = selector(node);
                foreach (var child in children)
                {
                    var select = selector(child);
                    if (!until(select))
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