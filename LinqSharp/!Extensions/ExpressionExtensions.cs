// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Utils;
using NStandard;
using System.ComponentModel;
using System.Linq.Expressions;

namespace LinqSharp;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class ExpressionExtensions
{
    /// <summary>
    /// Rebind parameter of expression, then return itself.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="this"></param>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static T RebindNode<T>(this T @this, Expression origin, Expression target)
        where T : Expression
    {
        if (origin == target) return @this;
        else
        {
            var visitor = new ExpressionRebinder(origin, target);
            return (visitor.Visit(@this) as T)!;
        }
    }

    /// <summary>
    /// Rebind all parameters of the specified expressions, combine them, then return the final expression.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="binary"></param>
    /// <returns></returns>
    public static Expression? ExpressionJoin(this IEnumerable<Expression> @this, Func<Expression, Expression, BinaryExpression> binary)
    {
        return @this.Aggregate(null as Expression, (acc, exp) =>
        {
            if (acc is null) return exp;
            else return binary(acc, exp);
        });
    }

    /// <summary>
    /// Rebind all parameters of the specified expressions, combine them, then return the final expression.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="this"></param>
    /// <param name="binary"></param>
    /// <returns></returns>
    public static T? LambdaJoin<T>(this IEnumerable<T> @this, Func<Expression, Expression, BinaryExpression> binary)
        where T : LambdaExpression
    {
        if (@this.AllSame(x => x.Parameters.Count))
        {
            var parameters = @this.First().Parameters;
            var lambda = Expression.Lambda(@this.Aggregate(null as Expression, (acc, exp) =>
            {

                if (acc is null) return exp.Body;
                else
                {
                    T rebindExp = exp;
                    foreach (var (item1, item2) in Any.Zip(exp.Parameters, parameters))
                    {
                        rebindExp = RebindNode(rebindExp, item1, item2);
                    }
                    return binary(acc, rebindExp.Body);
                }
            })!, parameters) as T;
            return lambda;
        }
        else return null;
    }

}
