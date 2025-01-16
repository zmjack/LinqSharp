// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Design;

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> @this, Func<QueryHelper<TSource>, QueryExpression<TSource>> build)
    {
        var helper = new QueryHelper<TSource>();
        var query = build(helper);

        var exp = query.Expression;
        if (exp is null) return @this;

        var predicate = exp.Compile();
        return @this.Where(predicate);
    }

    public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> @this, IFieldFilter<TSource> filter)
    {
        var helper = new QueryHelper<TSource>();
        var query = filter.Filter(helper);

        var exp = query.Expression;
        if (exp is null) return @this;

        var predicate = exp.Compile();
        return @this.Where(predicate);
    }

    public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> @this, ICoroutineFieldFilter<TSource> filter)
    {
        var helper = new QueryHelper<TSource>();

        var ret = @this;
        foreach (var query in filter.Filter(helper))
        {
            var exp = query.Expression;
            if (exp is null) continue;

            var predicate = exp.Compile();
            ret = ret.Where(x => predicate(x));
        }
        return ret;
    }

    public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> @this, ILocalFilter<TSource> filter)
    {
        return filter.Apply(@this);
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
        var query = filter.Filter(helper);
        var exp = query.Expression;
        if (exp is null) return @this;

        var predicate = exp.Compile();
        return @this.Where(x => predicate(selector(x)));
    }

    public static IEnumerable<TSource> FilterBy<TSource, TProperty>(this IEnumerable<TSource> @this, Func<TSource, TProperty> selector, ICoroutineFieldFilter<TProperty> filter)
    {
        if (filter is null) return @this;

        var helper = new QueryHelper<TProperty>();
        var ret = @this;
        foreach (var query in filter.Filter(helper))
        {
            var exp = query.Expression;
            if (exp is null) continue;

            var predicate = exp.Compile();
            ret = FilterBy(ret, selector, predicate);
        }
        return ret;
    }

}