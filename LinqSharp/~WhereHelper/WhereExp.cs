// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

namespace LinqSharp
{
    public class WhereExp<TSource>
    {
        public Expression<Func<TSource, bool>> Expression { get; private set; }

        public bool Empty => Expression is null;

        public WhereExp() { }

        public WhereExp(Expression<Func<TSource, bool>> expression)
        {
            Expression = expression;
        }

        public WhereExp<TSource> And(WhereExp<TSource> other) => this & other;
        public WhereExp<TSource> Or(WhereExp<TSource> other) => this | other;

        public static WhereExp<TSource> operator &(WhereExp<TSource> left, WhereExp<TSource> right)
        {
            if (left.Expression is null) return new WhereExp<TSource>(right.Expression);

            var parameter = left.Expression.Parameters[0];
            var leftExp = left.Expression.Body;
            var rightExp = right.Expression.Body.RebindParameter(right.Expression.Parameters[0], parameter);
            var exp = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(System.Linq.Expressions.Expression.AndAlso(leftExp, rightExp), parameter);
            return new WhereExp<TSource>(exp);
        }

        public static WhereExp<TSource> operator |(WhereExp<TSource> left, WhereExp<TSource> right)
        {
            if (left.Expression is null) return new WhereExp<TSource>(right.Expression);

            var parameter = left.Expression.Parameters[0];
            var leftExp = left.Expression.Body;
            var rightExp = right.Expression.Body.RebindParameter(right.Expression.Parameters[0], parameter);
            var exp = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(System.Linq.Expressions.Expression.OrElse(leftExp, rightExp), parameter);
            return new WhereExp<TSource>(exp);
        }

        public static WhereExp<TSource> operator !(WhereExp<TSource> operand)
        {
            var parameter = operand.Expression.Parameters[0];
            var opndExp = operand.Expression.Body;
            var exp = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(System.Linq.Expressions.Expression.Not(opndExp), parameter);
            return new WhereExp<TSource>(exp);
        }

    }
}
