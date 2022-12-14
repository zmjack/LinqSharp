// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

namespace LinqSharp
{
    public class WhereExpression<TSource>
    {
        public static readonly Lazy<WhereExpression<TSource>> Empty = new(() => new());
        public static readonly Lazy<WhereExpression<TSource>> False = new(() => new(x => false));
        public static readonly Lazy<WhereExpression<TSource>> True = new(() => new(x => true));

        public Expression<Func<TSource, bool>> Expression { get; private set; }

        public WhereExpression() { }
        public WhereExpression(Expression<Func<TSource, bool>> expression)
        {
            Expression = expression;
        }

        public WhereExpression<TSource> DefaultIfEmpty(WhereExpression<TSource> @default) => Expression is not null ? this : @default;

        public WhereExpression<TSource> And(WhereExpression<TSource> other) => this & other;
        public WhereExpression<TSource> Or(WhereExpression<TSource> other) => this | other;

        public static WhereExpression<TSource> operator &(WhereExpression<TSource> left, WhereExpression<TSource> right)
        {
            if (left.Expression is null && right.Expression is null) return Empty.Value;
            else if (left.Expression is null) return new WhereExpression<TSource>(right.Expression);
            else if (right.Expression is null) return new WhereExpression<TSource>(left.Expression);

            var parameter = left.Expression.Parameters[0];
            var leftExp = left.Expression.Body;
            var rightExp = right.Expression.Body.RebindParameter(right.Expression.Parameters[0], parameter);
            var exp = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(System.Linq.Expressions.Expression.AndAlso(leftExp, rightExp), parameter);
            return new WhereExpression<TSource>(exp);
        }

        public static WhereExpression<TSource> operator |(WhereExpression<TSource> left, WhereExpression<TSource> right)
        {
            if (left.Expression is null && right.Expression is null) return Empty.Value;
            else if (left.Expression is null) return new WhereExpression<TSource>(right.Expression);
            else if (right.Expression is null) return new WhereExpression<TSource>(left.Expression);

            var parameter = left.Expression.Parameters[0];
            var leftExp = left.Expression.Body;
            var rightExp = right.Expression.Body.RebindParameter(right.Expression.Parameters[0], parameter);
            var exp = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(System.Linq.Expressions.Expression.OrElse(leftExp, rightExp), parameter);
            return new WhereExpression<TSource>(exp);
        }

        public static WhereExpression<TSource> operator !(WhereExpression<TSource> operand)
        {
            if (operand.Expression is null) return Empty.Value;

            var parameter = operand.Expression.Parameters[0];
            var opndExp = operand.Expression.Body;
            var exp = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(System.Linq.Expressions.Expression.Not(opndExp), parameter);
            return new WhereExpression<TSource>(exp);
        }

        public override string ToString()
        {
            return Expression?.ToString();
        }

    }
}
