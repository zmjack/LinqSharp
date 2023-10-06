// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace LinqSharp.Strategies;

public class QueryStringStrategy<TEntity> : IQueryStrategy<TEntity, bool>
{
    public Expression<Func<TEntity, bool>> StrategyExpression { get; private set; }

    internal QueryStringStrategy()
    {
    }

    public void Init(Expression<Func<TEntity, object>> inExp, Func<Expression, Expression, Expression> compareExp, string searchString)
    {
        if (!searchString.IsNullOrWhiteSpace())
            StrategyExpression = GenerateExpression(inExp, compareExp, searchString);
        else StrategyExpression = x => true;
    }

    private ParameterExpression[] GetParameters(Expression expression)
    {
        if (expression is null) return new ParameterExpression[0];

        if (expression is ParameterExpression)
            return new[] { expression as ParameterExpression };

        //TODO: Maybe this expression can be converted to another expression in static.
        if (expression.NodeType == ExpressionType.Lambda)
        {
            return Enumerable.ToArray((expression as LambdaExpression).Parameters);
        }

        return expression switch
        {
            MemberExpression exp => GetParameters(exp.Expression),
            UnaryExpression exp => GetParameters(exp.Operand),
            MethodCallExpression exp => GetParameters(exp.Object).Concat(exp.Arguments.SelectMany(x => GetParameters(x))).ToArray(),
            NewExpression exp => exp.Arguments.SelectMany(x => GetParameters(x)).ToArray(),
            _ => new ParameterExpression[0],
        };
    }

    private Expression GetReturnStringOrArrayExpression(Expression expression)
    {
        if (expression.Type == typeof(string))
            return expression;
        else if (expression.Type.IsImplement<IEnumerable>())
        {
            var ienumerableGenericType = new[]
            {
                expression.Type,
            }
            .Concat(expression.Type.GetInterfaces()).Pipe(interfaces =>
            {
                var regex = new Regex(@"System\.Collections\.Generic\.IEnumerable`1\[(.+)\]");
                foreach (var @interface in interfaces)
                {
                    var match = regex.Match(@interface.ToString());
                    if (match.Success)
                        return Type.GetType(match.Groups[1].Value);
                }

                throw new NotSupportedException("Only IEnumerable<T> is supported.");
            });

            if (ienumerableGenericType != typeof(string))
            {
                // If the T of IEnumerable<T> is not string,
                // use System.Linq.Enumerable.Select method to convert it into string
                var selectMethod = typeof(Enumerable)
                    .GetMethodViaQualifiedName("System.Collections.Generic.IEnumerable`1[TResult] Select[TSource,TResult](System.Collections.Generic.IEnumerable`1[TSource], System.Func`2[TSource,TResult])")
                    .MakeGenericMethod(ienumerableGenericType, typeof(string));

                var parameter = Expression.Parameter(ienumerableGenericType);
                var lambda = Expression.Lambda(
                    Expression.Call(parameter, typeof(object).GetMethod(nameof(object.ToString))), parameter);
                return Expression.Call(selectMethod, expression, lambda);
            }
            else return expression;
        }
        else return Expression.Call(expression, typeof(object).GetMethod(nameof(object.ToString)));
    }

    private Expression<Func<TEntity, bool>> GenerateExpression(
        Expression<Func<TEntity, object>> inExp,
        Func<Expression, Expression, Expression> compareExp,
        string searchString)
    {
        Expression rightExp = Expression.Constant(searchString);

        switch (inExp.Body)
        {
            case NewExpression exp:
                var lambdaExp = exp.Arguments.Aggregate(null as Expression, (acc, argExp) =>
                {
                    if (acc is null)
                        return compareExp(GetReturnStringOrArrayExpression(argExp), rightExp);
                    else return Expression.OrElse(acc,
                        compareExp(GetReturnStringOrArrayExpression(argExp), rightExp));
                });
                return Expression.Lambda<Func<TEntity, bool>>(lambdaExp, inExp.Parameters);

            default:
                return Expression.Lambda<Func<TEntity, bool>>(
                    compareExp(GetReturnStringOrArrayExpression(inExp.Body), rightExp), inExp.Parameters);
        }
    }

}
