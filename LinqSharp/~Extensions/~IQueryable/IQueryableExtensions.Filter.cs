// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

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

        public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> @this, params IQueryFilter<TSource>[] filters)
        {
            var ret = @this;
            foreach (var filter in filters)
            {
                ret = filter.Apply(ret);
            }
            return ret;
        }

        public static IQueryable<TEntity> Filter<TEntity, TProperty>(this IQueryable<TEntity> @this, Expression<Func<TEntity, TProperty>> fieldSelector, Expression<Func<TProperty, bool>> filter)
        {
            var visitor = new ExpressionRebindVisitor(filter.Parameters[0], fieldSelector.Body);
            var body = visitor.Visit(filter.Body);
            var expression = Expression.Lambda(body, false, fieldSelector.Parameters[0]) as Expression<Func<TEntity, bool>>;
            return @this.Where(expression);
        }

        public static IQueryable<TEntity> Filter<TEntity, TProperty>(this IQueryable<TEntity> @this, Expression<Func<TEntity, TProperty>> fieldSelector, IFieldQueryFilter<TProperty> fieldFilter)
        {
            return Filter(@this, fieldSelector, fieldFilter.Filter);
        }

    }
}
