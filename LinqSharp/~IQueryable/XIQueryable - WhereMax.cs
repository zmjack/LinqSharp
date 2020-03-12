﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        public static IQueryable<TSource> WhereMax<TSource, TResult>(this IQueryable<TSource> sources, Expression<Func<TSource, TResult>> selector)
        {
            return sources.XWhere(h => h.WhereMaxExp(selector));
        }

    }
}
