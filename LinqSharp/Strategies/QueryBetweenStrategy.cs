// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.Strategies;

[Obsolete]
public class QueryBetweenStrategy<TEntity> : IQueryStrategy<TEntity, bool>
{
    public Expression<Func<TEntity, bool>> StrategyExpression { get; }

    private static readonly MethodInfo _Method_op_LessThanOrEqual = typeof(DateTime).GetMethodViaQualifiedName("Boolean op_LessThanOrEqual(System.DateTime, System.DateTime)");
    private static readonly PropertyInfo _Property_DateTime_HasValue = typeof(DateTime?).GetProperty(nameof(Nullable<DateTime>.HasValue))!;
    private static readonly PropertyInfo _Property_DateTime_Value = typeof(DateTime?).GetProperty(nameof(Nullable<DateTime>.Value))!;

    #region Return DateTime
    public QueryBetweenStrategy(
        Expression<Func<TEntity, DateTime>> memberExp,
        Expression<Func<TEntity, DateTime>> startExp,
        Expression<Func<TEntity, DateTime>> endExp)
    {
        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(
            Expression.AndAlso(
                Expression.LessThanOrEqual(
                    startExp.Body.RebindNode(startExp.Parameters[0], memberExp.Parameters[0]),
                    memberExp.Body,
                    false, _Method_op_LessThanOrEqual),
                Expression.LessThanOrEqual(
                    memberExp.Body,
                    endExp.Body.RebindNode(endExp.Parameters[0], memberExp.Parameters[0]),
                    false, _Method_op_LessThanOrEqual)), memberExp.Parameters);
    }

    public QueryBetweenStrategy(
        Expression<Func<TEntity, DateTime>> memberExp,
        DateTime start,
        Expression<Func<TEntity, DateTime>> endExp)
    {
        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(
            Expression.AndAlso(
                Expression.LessThanOrEqual(
                    Expression.Constant(start),
                    memberExp.Body,
                    false, _Method_op_LessThanOrEqual),
                Expression.LessThanOrEqual(
                    memberExp.Body,
                    endExp.Body.RebindNode(endExp.Parameters[0], memberExp.Parameters[0]),
                    false, _Method_op_LessThanOrEqual)), memberExp.Parameters);
    }

    public QueryBetweenStrategy(
        Expression<Func<TEntity, DateTime>> memberExp,
        Expression<Func<TEntity, DateTime>> startExp,
        DateTime end)
    {
        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(
            Expression.AndAlso(
                Expression.LessThanOrEqual(
                    startExp.Body.RebindNode(startExp.Parameters[0], memberExp.Parameters[0]),
                    memberExp.Body,
                    false, _Method_op_LessThanOrEqual),
                Expression.LessThanOrEqual(
                    memberExp.Body,
                    Expression.Constant(end),
                    false, _Method_op_LessThanOrEqual)), memberExp.Parameters);
    }

    public QueryBetweenStrategy(
        Expression<Func<TEntity, DateTime>> memberExp,
        DateTime start,
        DateTime end)
    {
        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(
            Expression.AndAlso(
                Expression.LessThanOrEqual(
                    Expression.Constant(start),
                    memberExp.Body,
                    false, _Method_op_LessThanOrEqual),
                Expression.LessThanOrEqual(
                    memberExp.Body,
                    Expression.Constant(end),
                    false, _Method_op_LessThanOrEqual)), memberExp.Parameters);
    }
    #endregion

    #region Return DateTime?
    public QueryBetweenStrategy(
        Expression<Func<TEntity, DateTime?>> memberExp,
        Expression<Func<TEntity, DateTime>> startExp,
        Expression<Func<TEntity, DateTime>> endExp)
    {
        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(
            Expression.AndAlso(
                Expression.LessThanOrEqual(
                    startExp.Body.RebindNode(startExp.Parameters[0], memberExp.Parameters[0]),
                    memberExp.Body,
                    false, _Method_op_LessThanOrEqual),
                Expression.LessThanOrEqual(
                    memberExp.Body,
                    endExp.Body.RebindNode(endExp.Parameters[0], memberExp.Parameters[0]),
                    false, _Method_op_LessThanOrEqual)), memberExp.Parameters);
    }

    public QueryBetweenStrategy(
        Expression<Func<TEntity, DateTime?>> memberExp,
        DateTime start,
        Expression<Func<TEntity, DateTime>> endExp)
    {
        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(
            Expression.Condition(
                Expression.Property(memberExp.Body, _Property_DateTime_HasValue),
                Expression.AndAlso(
                    Expression.LessThanOrEqual(
                        Expression.Constant(start),
                        memberExp.Body,
                        false, _Method_op_LessThanOrEqual),
                    Expression.LessThanOrEqual(
                        memberExp.Body,
                        endExp.Body.RebindNode(endExp.Parameters[0], memberExp.Parameters[0]),
                        false, _Method_op_LessThanOrEqual)),
                Expression.Constant(false)),
                memberExp.Parameters);
    }

    public QueryBetweenStrategy(
        Expression<Func<TEntity, DateTime?>> memberExp,
        Expression<Func<TEntity, DateTime>> startExp,
        DateTime end)
    {
        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(
            Expression.Condition(
                Expression.Property(memberExp.Body, _Property_DateTime_HasValue),
                Expression.AndAlso(
                    Expression.LessThanOrEqual(
                        startExp.Body.RebindNode(startExp.Parameters[0], memberExp.Parameters[0]),
                        memberExp.Body,
                        false, _Method_op_LessThanOrEqual),
                    Expression.LessThanOrEqual(
                        memberExp.Body,
                        Expression.Constant(end),
                        false, _Method_op_LessThanOrEqual)),
                Expression.Constant(false)),
                memberExp.Parameters);
    }

    public QueryBetweenStrategy(
        Expression<Func<TEntity, DateTime?>> memberExp,
        DateTime start,
        DateTime end)
    {
        StrategyExpression = Expression.Lambda<Func<TEntity, bool>>(
            Expression.Condition(
                Expression.Property(memberExp.Body, _Property_DateTime_HasValue),
                Expression.AndAlso(
                    Expression.LessThanOrEqual(
                        Expression.Constant(start),
                        Expression.Property(memberExp.Body, _Property_DateTime_Value),
                        false, _Method_op_LessThanOrEqual),
                    Expression.LessThanOrEqual(
                        Expression.Property(memberExp.Body, _Property_DateTime_Value),
                        Expression.Constant(end),
                        false, _Method_op_LessThanOrEqual)),
                Expression.Constant(false)),
                memberExp.Parameters);
    }
    #endregion

}
