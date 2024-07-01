// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Query;
using System.Linq.Expressions;

namespace LinqSharp;

public static partial class IQueryableExtensions
{
    public static IQueryable<TSource> WhereMin<TSource, TResult>(this IQueryable<TSource> sources, Expression<Func<TSource, TResult>> selector)
    {
        return sources.Filter(h =>
        {
            if (sources.Any())
            {
                var min = sources.Min(selector);
                var whereExp = Expression.Lambda<Func<TSource, bool>>(
                    Expression.Equal(selector.Body, Expression.Constant(min, typeof(TResult))), selector.Parameters);
                return new QueryExpression<TSource>(whereExp);
            }
            else return new QueryExpression<TSource>(x => false);
        });
    }

}
