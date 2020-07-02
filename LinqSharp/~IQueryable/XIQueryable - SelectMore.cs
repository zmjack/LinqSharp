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
        [Obsolete("This function does not support generating SQL.", true)]
        public static IQueryable<TSource> SelectMore<TSource>(this IQueryable<TSource> @this, Func<TSource, IEnumerable<TSource>> selector)
        {
            throw new NotSupportedException();
        }

        [Obsolete("This function does not support generating SQL.", true)]
        public static IQueryable<TSource> SelectMore<TSource>(this IQueryable<TSource> @this, Func<TSource, IEnumerable<TSource>> childrenSelector, Func<TSource, bool> predicate)
        {
            throw new NotSupportedException();
        }

    }
}