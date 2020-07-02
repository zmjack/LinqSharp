// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        public static IQueryable<TSource> WhereMin<TSource, TResult>(this IQueryable<TSource> sources, Expression<Func<TSource, TResult>> selector)
        {
            return sources.XWhere(h => h.WhereMin(selector));
        }

    }
}
