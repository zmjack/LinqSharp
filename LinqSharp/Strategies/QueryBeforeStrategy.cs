// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.Strategies;

[Obsolete]
public class QueryBeforeStrategy<TEntity> : IQueryStrategy<TEntity, bool>
{
    public Expression<Func<TEntity, bool>> StrategyExpression { get; }

    private static readonly MethodInfo _Method_DateTime_op_LessThanOrEqual = typeof(DateTime).GetMethodViaQualifiedName("Boolean op_LessThanOrEqual(System.DateTime, System.DateTime)");
    private static readonly MethodInfo _Method_DateTime_op_LessThan = typeof(DateTime).GetMethodViaQualifiedName("Boolean op_LessThan(System.DateTime, System.DateTime)");
    private static readonly PropertyInfo _Property_DateTime_HasValue = typeof(DateTime?).GetProperty(nameof(Nullable<DateTime>.HasValue))!;
    private static readonly PropertyInfo _Property_DateTime_Value = typeof(DateTime?).GetProperty(nameof(Nullable<DateTime>.Value))!;

    public QueryBeforeStrategy(
        Expression<Func<TEntity, DateTime>> memberExp,
        Expression<Func<TEntity, DateTime>> beforeExp,
        bool includePoint)
    {
        var left = memberExp.Body;
        var right = beforeExp.Body.RebindNode(beforeExp.Parameters[0], memberExp.Parameters[0]);

        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(includePoint ?
            Expression.LessThanOrEqual(left, right, false, _Method_DateTime_op_LessThanOrEqual)
            : Expression.LessThan(left, right, false, _Method_DateTime_op_LessThan), memberExp.Parameters);
    }

    public QueryBeforeStrategy(
        Expression<Func<TEntity, DateTime>> memberExp,
        DateTime before,
        bool includePoint)
    {
        var left = memberExp.Body;
        var right = Expression.Constant(before);

        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(includePoint ?
            Expression.LessThanOrEqual(left, right, false, _Method_DateTime_op_LessThanOrEqual)
            : Expression.LessThan(left, right, false, _Method_DateTime_op_LessThan), memberExp.Parameters);
    }

    public QueryBeforeStrategy(
        Expression<Func<TEntity, DateTime?>> memberExp,
        Expression<Func<TEntity, DateTime>> beforeExp,
        bool liftNullToTrue, bool includePoint)
    {
        var left = memberExp.Body;
        var right = beforeExp.Body.RebindNode(beforeExp.Parameters[0], memberExp.Parameters[0]);

        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(
            Expression.Condition(
                Expression.Not(Expression.Property(memberExp.Body, _Property_DateTime_HasValue)),
                    Expression.Constant(liftNullToTrue),
                    includePoint
                        ? Expression.LessThanOrEqual(Expression.Property(left, _Property_DateTime_Value), right, false, _Method_DateTime_op_LessThanOrEqual)
                        : Expression.LessThan(left, right, false, _Method_DateTime_op_LessThan)),
            memberExp.Parameters);
    }

    public QueryBeforeStrategy(
        Expression<Func<TEntity, DateTime?>> memberExp,
        DateTime before,
        bool liftNullToTrue, bool includePoint)
    {
        var left = memberExp.Body;
        var right = Expression.Constant(before);

        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(
            Expression.Condition(
                Expression.Not(Expression.Property(memberExp.Body, _Property_DateTime_HasValue)),
                    Expression.Constant(liftNullToTrue),
                    includePoint
                        ? Expression.LessThanOrEqual(Expression.Property(left, _Property_DateTime_Value), right, false, _Method_DateTime_op_LessThanOrEqual)
                        : Expression.LessThan(left, right, false, _Method_DateTime_op_LessThan)),
            memberExp.Parameters);
    }

    public QueryBeforeStrategy(
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
                    Expression fullExp;
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
            Expression.LessThanOrEqual(compareExp, Expression.Constant(0))
            : Expression.LessThan(compareExp, Expression.Constant(0)), parameters);
    }

}
