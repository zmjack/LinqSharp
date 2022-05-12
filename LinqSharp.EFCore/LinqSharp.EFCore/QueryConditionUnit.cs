// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Caching.Memory;
using NStandard;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.EFCore
{
    public class QueryConditionUnit<TEntity>
    {
        public readonly string PropName;
        public readonly object ExpectedValue;
        public readonly Expression<Func<TEntity, object>> UnitExpression;

        public static MemoryCache PropAccessCache = new(new MemoryCacheOptions());
        public PropertyInfo Property
        {
            get
            {
                return PropAccessCache.GetOrCreate(PropName, entry =>
                {
                    return typeof(TEntity).GetProperty(PropName);
                });
            }
        }

        public QueryConditionUnit(Expression<Func<TEntity, object>> expression, object expectedValue)
        {
            UnitExpression = expression;
            ExpectedValue = expectedValue;
            PropName = (expression.Body.For(body => (body as UnaryExpression)?.Operand ?? body) as MemberExpression).Member.Name;
        }

        public QueryConditionUnit(string propName, object expectedValue)
        {
            var parameter = Expression.Parameter(typeof(TEntity));
            var body = Expression.Property(parameter, propName);
            UnitExpression = Expression.Lambda<Func<TEntity, object>>(body, parameter);
            ExpectedValue = expectedValue;
            PropName = propName;
        }

    }

}
