// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

namespace LinqSharp.Query;

public class QueryExpression<TSource>
{
    internal static readonly Lazy<QueryExpression<TSource>> Empty = new(() => new());
    internal static readonly Lazy<QueryExpression<TSource>> False = new(() => new(x => false));
    internal static readonly Lazy<QueryExpression<TSource>> True = new(() => new(x => true));

    public ParameterExpression Parameter { get; }
    public Expression<Func<TSource, bool>> Expression { get; }

    public QueryExpression()
    {
        Parameter = System.Linq.Expressions.Expression.Parameter(typeof(TSource));
    }
    public QueryExpression(ParameterExpression parameter)
    {
        if (parameter.Type != typeof(TSource)) throw new ArgumentException($"The parameter type must be {typeof(TSource)}.", nameof(parameter));
        Parameter = System.Linq.Expressions.Expression.Parameter(typeof(TSource));
    }
    public QueryExpression(Expression<Func<TSource, bool>> expression)
    {
        Expression = expression;
        Parameter = expression.Parameters[0];
    }

    public QueryExpression<TSource> DefaultIfEmpty(QueryExpression<TSource> @default) => Expression is not null ? this : @default;

    public QueryExpression<TSource> And(QueryExpression<TSource> other) => this & other;
    public QueryExpression<TSource> Or(QueryExpression<TSource> other) => this | other;

    public static QueryExpression<TSource> operator &(QueryExpression<TSource> left, QueryExpression<TSource> right)
    {
        if (left.Expression is null && right.Expression is null) return Empty.Value;
        else if (left.Expression is null) return new QueryExpression<TSource>(right.Expression);
        else if (right.Expression is null) return new QueryExpression<TSource>(left.Expression);

        var leftExp = left.Expression.Body;
        var rightExp = right.Expression.Body.RebindParameter(right.Expression.Parameters[0], left.Parameter);
        var exp = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(System.Linq.Expressions.Expression.AndAlso(leftExp, rightExp), left.Parameter);
        return new QueryExpression<TSource>(exp);
    }

    public static QueryExpression<TSource> operator |(QueryExpression<TSource> left, QueryExpression<TSource> right)
    {
        if (left.Expression is null && right.Expression is null) return Empty.Value;
        else if (left.Expression is null) return new QueryExpression<TSource>(right.Expression);
        else if (right.Expression is null) return new QueryExpression<TSource>(left.Expression);

        var leftExp = left.Expression.Body;
        var rightExp = right.Expression.Body.RebindParameter(right.Expression.Parameters[0], left.Parameter);
        var exp = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(System.Linq.Expressions.Expression.OrElse(leftExp, rightExp), left.Parameter);
        return new QueryExpression<TSource>(exp);
    }

    public static QueryExpression<TSource> operator !(QueryExpression<TSource> operand)
    {
        if (operand.Expression is null) return Empty.Value;

        var opndExp = operand.Expression.Body;
        var exp = System.Linq.Expressions.Expression.Lambda<Func<TSource, bool>>(System.Linq.Expressions.Expression.Not(opndExp), operand.Parameter);
        return new QueryExpression<TSource>(exp);
    }

    public override string ToString()
    {
        return Expression?.ToString();
    }

}
