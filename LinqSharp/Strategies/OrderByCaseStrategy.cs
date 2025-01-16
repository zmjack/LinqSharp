// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System.Linq.Expressions;

namespace LinqSharp.Strategies;

public class OrderByCaseStrategy<TEntity, TRet> : IQueryStrategy<TEntity, int>
{
    public Expression<Func<TEntity, int>> StrategyExpression { get; }

    public OrderByCaseStrategy(
        Expression<Func<TEntity, TRet>> memberExp,
        TRet[] orderValues)
    {
        var valueLength = orderValues.Length;
        var lambdaExp = orderValues.Reverse().Pairs().Aggregate(null as Expression, (acc, pair) =>
        {
            var (index, value) = pair;
            var compareExp = Expression.Equal(memberExp.Body, Expression.Constant(value));

            if (acc is null)
            {
                return
                    Expression.Condition(
                        compareExp,
                        Expression.Constant(valueLength - 1 - index),
                        Expression.Constant(valueLength));
            }
            else
            {
                return
                    Expression.Condition(
                        compareExp,
                        Expression.Constant(valueLength - 1 - index),
                        acc);
            }
        }) ?? Expression.Constant(0);

        StrategyExpression = Expression.Lambda<Func<TEntity, int>>(lambdaExp, memberExp.Parameters);
    }

}
