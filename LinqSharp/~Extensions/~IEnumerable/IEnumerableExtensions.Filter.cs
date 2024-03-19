// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> @this, Func<QueryHelper<TSource>, QueryExpression<TSource>> build)
    {
        var helper = new QueryHelper<TSource>();
        var whereExp = build(helper);

        if (whereExp.Expression is not null)
        {
            var predicate = whereExp.Expression.Compile();
            return @this.Where(predicate);
        }
        else return @this;
    }

    public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> @this, params ILocalFilter<TSource>[] filters)
    {
        if (filters is null) return @this;

        var ret = @this;
        foreach (var filter in filters)
        {
            if (filter is null) continue;
            ret = filter.Apply(ret);
        }
        return ret;
    }

    public static IEnumerable<TSource> FilterBy<TSource, TProperty>(this IEnumerable<TSource> @this, Func<TSource, TProperty> selector, Func<TProperty, bool> filter)
    {
        if (filter is null) return @this;

        return @this.Where(x => filter(selector(x)));
    }

    public static IEnumerable<TSource> FilterBy<TSource, TProperty>(this IEnumerable<TSource> @this, Func<TSource, TProperty> selector, IFieldFilter<TProperty> filter)
    {
        if (filter is null) return @this;

        var helper = new QueryHelper<TProperty>();
        var expression = filter.Filter(helper).Expression;
        if (expression is null) return @this;

        var predicate = expression.Compile();
        return @this.Where(x => predicate(selector(x)));
    }

    public static IEnumerable<TSource> FilterBy<TSource, TProperty>(this IEnumerable<TSource> @this, Func<TSource, TProperty> selector, IAdvancedFieldFilter<TProperty> extraFilter)
    {
        if (extraFilter is null) return @this;

        var helper = new QueryHelper<TProperty>();
        var ret = @this;
        foreach (var filter in extraFilter.Filter(helper))
        {
            var expression = filter.Expression;
            if (expression is null) return ret;

            var predicate = expression.Compile();
            ret = FilterBy(ret, selector, predicate);
        }
        return ret;
    }

}