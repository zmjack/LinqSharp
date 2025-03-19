// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Design;
using System.Linq.Expressions;

namespace LinqSharp;

public static partial class IQueryableExtensions
{
    public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> @this, Func<QueryHelper<TSource>, QueryExpression<TSource>> build)
    {
        var helper = new QueryHelper<TSource>();
        var exp = build(helper);

        if (exp.Expression is null) return @this;
        return @this.Where(exp.Expression);
    }

    public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> @this, IFieldFilter<TSource> filter)
    {
        var helper = new QueryHelper<TSource>();
        var query = filter.Filter(helper);

        if (query.Expression is null) return @this;
        return @this.Where(query.Expression);
    }

    public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> @this, ICoFieldFilter<TSource> filter)
    {
        var helper = new QueryHelper<TSource>();

        var ret = @this;
        foreach (var query in filter.Filter(helper))
        {
            var exp = query.Expression;
            if (exp is null) continue;

            ret = ret.Where(exp);
        }
        return ret;
    }

    public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> @this, IQueryFilter<TSource> filter)
    {
        return filter.Apply(@this);
    }

    public static IQueryable<TSource> FilterBy<TSource, TProperty>(this IQueryable<TSource> @this, Expression<Func<TSource, TProperty>> selector, Expression<Func<TProperty, bool>> filter)
    {
        if (filter is null) return @this;

        var body = filter.Body.RebindNode(filter.Parameters[0], selector.Body)!;
        var expression = (Expression.Lambda(body, false, selector.Parameters[0]) as Expression<Func<TSource, bool>>)!;
        return @this.Where(expression);
    }

    public static IQueryable<TSource> FilterBy<TSource, TProperty>(this IQueryable<TSource> @this, Expression<Func<TSource, TProperty>> selector, IFieldFilter<TProperty> filter)
    {
        if (filter is null) return @this;

        var helper = new QueryHelper<TProperty>();
        var expression = filter.Filter(helper).Expression!;
        return FilterBy(@this, selector, expression);
    }

    public static IQueryable<TSource> FilterBy<TSource, TProperty>(this IQueryable<TSource> @this, Expression<Func<TSource, TProperty>> selector, ICoFieldFilter<TProperty> filter)
    {
        if (filter is null) return @this;

        var helper = new QueryHelper<TProperty>();
        var ret = @this;
        foreach (var query in filter.Filter(helper))
        {
            var exp = query.Expression;
            if (exp is null) continue;

            ret = FilterBy(ret, selector, exp);
        }
        return ret;
    }

}
