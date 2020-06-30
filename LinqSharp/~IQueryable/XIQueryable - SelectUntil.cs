using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        [Obsolete("This function does not support generating SQL.", true)]
        public static IQueryable<TSource> SelectUntil<TSource>(this IQueryable<TSource> @this, Func<TSource, IEnumerable<TSource>> childrenSelector, Func<TSource, bool> predicate)
        {
            throw new NotSupportedException();
        }

    }
}