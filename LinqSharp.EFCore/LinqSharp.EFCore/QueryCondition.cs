// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LinqSharp.EFCore
{
    public class QueryCondition<TEntity> : IEquatable<QueryCondition<TEntity>> where TEntity : class, new()
    {
        public readonly List<QueryConditionUnit<TEntity>> UnitList = new();

        public object this[string propName]
        {
            set
            {
                var parameter = Expression.Parameter(typeof(TEntity));
                var body = Expression.Property(parameter, propName);

                var expression = Expression.Lambda<Func<TEntity, object>>(body, parameter);
                UnitList.Add(new QueryConditionUnit<TEntity>(expression, value));
            }
        }

        public object this[Expression<Func<TEntity, object>> expression]
        {
            set => UnitList.Add(new QueryConditionUnit<TEntity>(expression, value));
        }

        public override int GetHashCode() => ToString().GetHashCode();

        public bool Equals(QueryCondition<TEntity> other)
        {
            if (UnitList.Count != other.UnitList.Count) return false;
            foreach (var pair in Zipper.Create(UnitList, other.UnitList))
            {
                if (pair.Item1.PropName != pair.Item2.PropName) return false;
                if (!pair.Item1.ExpectedValue.Equals(pair.Item2.ExpectedValue)) return false;
            }
            return true;
        }

        public Expression<Func<TEntity, bool>> GetExpression()
        {
            if (!UnitList.Any()) throw new InvalidOperationException("Missing conditional unit.");

            var units = UnitList.ToArray();
            var parameter = units[0].UnitExpression.Parameters[0];
            var predicate = units.Select(x => Expression.Lambda<Func<TEntity, bool>>(
                Expression.Equal(
                    x.UnitExpression.RebindParameter(x.UnitExpression.Parameters[0], parameter).Body.For(body => (body as UnaryExpression)?.Operand ?? body),
                    Expression.Constant(x.ExpectedValue)),
                parameter)).ToArray().LambdaJoin(Expression.AndAlso);
            return predicate;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var unit in UnitList)
            {
                var part = $"x.{unit.PropName} == {(unit.ExpectedValue is string ? $"\"{unit.ExpectedValue}\"" : unit.ToString())}";
                if (sb.Length == 0) sb.Append($"x => {part}");
                else sb.Append($"&& {part}");
            }
            return sb.ToString();
        }

    }
}
