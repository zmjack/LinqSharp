// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.Strategies;

[Obsolete]
public class QueryAfterStrategy<TEntity> : IQueryStrategy<TEntity, bool>
{
    public Expression<Func<TEntity, bool>> StrategyExpression { get; }

    private static readonly MethodInfo _Method_DateTime_op_GreaterThanOrEqual = typeof(DateTime).GetMethodViaQualifiedName("Boolean op_GreaterThanOrEqual(System.DateTime, System.DateTime)");
    private static readonly MethodInfo _Method_DateTime_op_GreaterThan = typeof(DateTime).GetMethodViaQualifiedName("Boolean op_GreaterThan(System.DateTime, System.DateTime)");
    private static readonly PropertyInfo _Property_DateTime_HasValue = typeof(DateTime?).GetProperty(nameof(Nullable<DateTime>.HasValue))!;
    private static readonly PropertyInfo _Property_DateTime_Value = typeof(DateTime?).GetProperty(nameof(Nullable<DateTime>.Value))!;

    public QueryAfterStrategy(
        Expression<Func<TEntity, DateTime>> memberExp,
        Expression<Func<TEntity, DateTime>> afterExp,
        bool includePoint)
    {
        var left = memberExp.Body;
        var right = afterExp.Body.RebindNode(afterExp.Parameters[0], memberExp.Parameters[0]);

        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(includePoint ?
            Expression.GreaterThanOrEqual(left, right, false, _Method_DateTime_op_GreaterThanOrEqual)
            : Expression.GreaterThan(left, right, false, _Method_DateTime_op_GreaterThan), memberExp.Parameters);
    }

    public QueryAfterStrategy(
        Expression<Func<TEntity, DateTime>> memberExp,
        DateTime after,
        bool includePoint)
    {
        var left = memberExp.Body;
        var right = Expression.Constant(after);

        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(includePoint ?
            Expression.GreaterThanOrEqual(left, right, false, _Method_DateTime_op_GreaterThanOrEqual)
            : Expression.GreaterThan(left, right, false, _Method_DateTime_op_GreaterThan), memberExp.Parameters);
    }

    public QueryAfterStrategy(
        Expression<Func<TEntity, DateTime?>> memberExp,
        Expression<Func<TEntity, DateTime>> afterExp,
        bool liftNullToTrue, bool includePoint)
    {
        var left = memberExp.Body;
        var right = afterExp.Body.RebindNode(afterExp.Parameters[0], memberExp.Parameters[0]);

        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(
            Expression.Condition(
                Expression.Not(Expression.Property(memberExp.Body, _Property_DateTime_HasValue)),
                    Expression.Constant(liftNullToTrue),
                    includePoint
                        ? Expression.GreaterThanOrEqual(Expression.Property(left, _Property_DateTime_Value), right, false, _Method_DateTime_op_GreaterThanOrEqual)
                        : Expression.GreaterThan(left, right, false, _Method_DateTime_op_GreaterThan)),
            memberExp.Parameters);
    }

    public QueryAfterStrategy(
        Expression<Func<TEntity, DateTime?>> memberExp,
        DateTime after,
        bool liftNullToTrue, bool includePoint)
    {
        var left = memberExp.Body;
        var right = Expression.Constant(after);

        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(
            Expression.Condition(
                Expression.Not(Expression.Property(memberExp.Body, _Property_DateTime_HasValue)),
                    Expression.Constant(liftNullToTrue),
                    includePoint
                        ? Expression.GreaterThanOrEqual(Expression.Property(left, _Property_DateTime_Value), right, false, _Method_DateTime_op_GreaterThanOrEqual)
                        : Expression.GreaterThan(left, right, false, _Method_DateTime_op_GreaterThan)),
            memberExp.Parameters);
    }

    public QueryAfterStrategy(
        Expression<Func<TEntity, object>> yearExp,
        Expression<Func<TEntity, object>> monthExp,
        Expression<Func<TEntity, object>> dayExp,
        DateTime before,
        bool includePoint)
    {
        var parameters = yearExp.Parameters;

        Expression GetFilledExpression(Expression bodyExp, int padLength)
        {
            return RangeEx.Create(0, padLength - 1).Aggregate(null as Expression, (_acc, i) =>
            {
                var stringBody = Expression.Convert(bodyExp, typeof(string));
                return
                    Expression.Condition(
                        Expression.Equal(
                            Expression.Property(stringBody, nameof(string.Length)),
                            Expression.Constant(i)),
                        Expression.Add(
                            Expression.Constant("0".Repeat(padLength - i)),
                            stringBody,
                            MethodAccessor.String.Concat_Object),
                        _acc is null ? stringBody : _acc);
            })!;
        }

        var compareExp =
            Expression.Call(
                new Expression<Func<TEntity, object>>[]
                {
                    yearExp,
                    monthExp,
                    dayExp,
                }.Aggregate(null as Expression, (acc, exp) =>
                {
                    Expression? fullExp;
                    switch (exp)
                    {
                        case Expression<Func<TEntity, object>> e when e == yearExp:
                            fullExp = Expression.Add(
                                GetFilledExpression(exp.Body, 4),
                                Expression.Constant("-"), MethodAccessor.String.Concat_Object);
                            break;

                        case Expression<Func<TEntity, object>> e when e == monthExp:
                            fullExp = Expression.Add(
                                GetFilledExpression(exp.Body.RebindNode(exp.Parameters[0], parameters[0]), 2),
                                Expression.Constant("-"), MethodAccessor.String.Concat_Object);
                            break;

                        case Expression<Func<TEntity, object>> e when e == dayExp:
                            fullExp = GetFilledExpression(exp.Body.RebindNode(exp.Parameters[0], parameters[0]), 2);
                            break;

                        default: throw new NotSupportedException();
                    }

                    if (acc is null)
                        return fullExp;
                    else return Expression.Add(acc, fullExp, MethodAccessor.String.Concat_Object);
                }),
                MethodAccessor.String.CompareTo,
                Expression.Constant(before.ToString("yyyy-MM-dd")));

        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(includePoint ?
            Expression.GreaterThanOrEqual(compareExp, Expression.Constant(0))
            : Expression.GreaterThan(compareExp, Expression.Constant(0)), parameters);
    }

}
