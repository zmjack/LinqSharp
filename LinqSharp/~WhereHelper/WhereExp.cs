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
        public Expression<Func<TSource, bool>> Exp { get; private set; }

        public WhereExp(Func<Expression<Func<TSource, bool>>> build)
        {
            Exp = build();
        }

        public WhereExp(Expression<Func<TSource, bool>> exp)
        {
            Exp = exp;
        }

        public WhereExp<TSource> And(WhereExp<TSource> other) => this & other;
        public WhereExp<TSource> Or(WhereExp<TSource> other) => this | other;

        public static WhereExp<TSource> operator &(WhereExp<TSource> left, WhereExp<TSource> right)
        {
            var parameter = left.Exp.Parameters[0];
            var leftExp = left.Exp.Body;
            var rightExp = right.Exp.Body.RebindParameter(right.Exp.Parameters[0], parameter);
            var exp = Expression.Lambda<Func<TSource, bool>>(Expression.AndAlso(leftExp, rightExp), parameter);
            return new WhereExp<TSource>(exp);
        }

        public static WhereExp<TSource> operator |(WhereExp<TSource> left, WhereExp<TSource> right)
        {
            var parameter = left.Exp.Parameters[0];
            var leftExp = left.Exp.Body;
            var rightExp = right.Exp.Body.RebindParameter(right.Exp.Parameters[0], parameter);
            var exp = Expression.Lambda<Func<TSource, bool>>(Expression.OrElse(leftExp, rightExp), parameter);
            return new WhereExp<TSource>(exp);
        }

        public static WhereExp<TSource> operator !(WhereExp<TSource> operand)
        {
            var parameter = operand.Exp.Parameters[0];
            var opndExp = operand.Exp.Body;
            var exp = Expression.Lambda<Func<TSource, bool>>(Expression.Not(opndExp), parameter);
            return new WhereExp<TSource>(exp);
        }
    }

}
