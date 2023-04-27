// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

namespace LinqSharp.Query.Infrastructure
{
    public class QueryExpression<TSource>
    {
        public static readonly Lazy<QueryExpression<TSource>> Empty = new(() => new());
        public static readonly Lazy<QueryExpression<TSource>> False = new(() => new(x => false));
        public static readonly Lazy<QueryExpression<TSource>> True = new(() => new(x => true));

        public Expression<Func<TSource, bool>> Expression { get; private set; }

        public QueryExpression() { }
        public QueryExpression(Expression<Func<TSource, bool>> expression)
        {
            Expression = expression;
        }

        public QueryExpression<TSource> DefaultIfEmpty(QueryExpression<TSource> @default) => Expression is not null ? this : @default;

        public QueryExpression<TSource> And(QueryExpression<TSource> other) => this & other;
        public QueryExpression<TSource> Or(QueryExpression<TSource> other) => this | other;

        public static QueryExpression<TSource> operator &(QueryExpression<TSource> left, QueryExpression<TSource> right)
        {
            if (left.Expression is null && right.Expression is null) return Empty.Value;
            else if (left.Expression is null) return new QueryExpression<TSource>(right.Expression);
            else if (right.Expression is null) return new QueryExpression<TSource>(left.Expression);

            var parameter = left.Expression.Parameters[0];
            var leftExp = left.Expression.Body;
            var rightExp = right.Expression.Body.RebindParameter(right.Expression.Parameters[0], parameter);
            var exp = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(System.Linq.Expressions.Expression.AndAlso(leftExp, rightExp), parameter);
            return new QueryExpression<TSource>(exp);
        }

        public static QueryExpression<TSource> operator |(QueryExpression<TSource> left, QueryExpression<TSource> right)
        {
            if (left.Expression is null && right.Expression is null) return Empty.Value;
            else if (left.Expression is null) return new QueryExpression<TSource>(right.Expression);
            else if (right.Expression is null) return new QueryExpression<TSource>(left.Expression);

            var parameter = left.Expression.Parameters[0];
            var leftExp = left.Expression.Body;
            var rightExp = right.Expression.Body.RebindParameter(right.Expression.Parameters[0], parameter);
            var exp = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(System.Linq.Expressions.Expression.OrElse(leftExp, rightExp), parameter);
            return new QueryExpression<TSource>(exp);
        }

        public static QueryExpression<TSource> operator !(QueryExpression<TSource> operand)
        {
            if (operand.Expression is null) return Empty.Value;

            var parameter = operand.Expression.Parameters[0];
            var opndExp = operand.Expression.Body;
            var exp = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(System.Linq.Expressions.Expression.Not(opndExp), parameter);
            return new QueryExpression<TSource>(exp);
        }

        public override string ToString()
        {
            return Expression?.ToString();
        }

    }
}
