// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Query;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
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
            var ret = @this;
            foreach (var filter in filters)
            {
                ret = filter.Apply(ret);
            }
            return ret;
        }

    }
}