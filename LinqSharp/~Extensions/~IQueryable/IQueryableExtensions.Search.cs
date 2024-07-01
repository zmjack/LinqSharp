// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Query;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp;

public static partial class IQueryableExtensions
{
    private static readonly MethodInfo method_any_string = MethodAccessor.Enumerable.Any1.MakeGenericMethod(typeof(string));

    [Obsolete("Use SearchMode instead.")]
    public static IQueryable<TEntity> Search<TEntity>(this IQueryable<TEntity> @this, string searchString, Expression<Func<TEntity, object>> searchMembers, SearchOption option = SearchOption.Contains)
    {
        return @this.Filter(h => h.Search(searchString, searchMembers, option));
    }

    [Obsolete("Use SearchMode instead.")]
    public static IQueryable<TEntity> Search<TEntity>(this IQueryable<TEntity> @this, string[] searchStrings, Expression<Func<TEntity, object>> searchMembers, SearchOption option = SearchOption.Contains)
    {
        return @this.Filter(h => h.Search(searchStrings, searchMembers, option));
    }

    public static IQueryable<T> Search<T>(this IQueryable<T> @this, SearchMode mode, string search, Expression<Func<T, SearchSelector>> selector)
    {
        return Search(@this, mode, [search], selector);
    }

    public static IQueryable<TEntity> Search<TEntity>(this IQueryable<TEntity> @this, SearchMode mode, string[] searches, Expression<Func<TEntity, SearchSelector>> selector)
    {
        return @this.Filter(new SearchFilter<TEntity>(mode, searches, selector));
    }
}
