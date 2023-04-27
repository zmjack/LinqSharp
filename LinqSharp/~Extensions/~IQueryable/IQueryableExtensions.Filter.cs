// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Query;
using LinqSharp.Query.Infrastructure;
using System;
using System.Linq;

namespace LinqSharp
{
    public static partial class IQueryableExtensions
    {
        public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> @this, Func<QueryHelper<TSource>, QueryExpression<TSource>> build)
        {
            var helper = new QueryHelper<TSource>();
            var whereExp = build(helper);

            if (whereExp.Expression is not null)
            {
                return @this.Where(whereExp.Expression);
            }
            else return @this;
        }

        public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> @this, IQueryFilter<TSource> filter)
        {
            return filter.Apply(@this);
        }
    }
}
