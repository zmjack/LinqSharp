// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Filters;
using System.Linq.Expressions;

namespace LinqSharp;

public static partial class IQueryableExtensions
{
    public static IQueryable<T> Search<T>(this IQueryable<T> @this, SearchMode mode, string search, Expression<Func<T, SearchSelector>> selector)
    {
        return Search(@this, mode, [search], selector);
    }

    public static IQueryable<TEntity> Search<TEntity>(this IQueryable<TEntity> @this, SearchMode mode, string[] searches, Expression<Func<TEntity, SearchSelector>> selector)
    {
        return @this.Filter(new SearchFilter<TEntity>(mode, searches, selector));
    }
}
