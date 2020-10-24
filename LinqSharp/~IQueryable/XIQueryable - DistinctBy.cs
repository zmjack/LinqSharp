// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        [Obsolete("This function does not support generating.", true)]
        public static IQueryable<TSource> DistinctBy<TSource>(this IQueryable<TSource> source, Func<TSource, object> compare)
        {
            throw new NotSupportedException();
        }

    }
}