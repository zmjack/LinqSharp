// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq.Expressions;

namespace LinqSharp
{
    public class WhereExpBuilder<TSource>
    {
        public ParameterExpression Parameter { get; set; }
        public Expression Expression { get; set; }

        public WhereExpBuilder()
        {
            Parameter = Expression.Parameter(typeof(TSource));
            Expression = Parameter;
        }

        public WhereExpBuilder(ParameterExpression parameter, Expression expression)
        {
            Parameter = parameter;
            Expression = expression;
        }

        public PropertyUnit<TSource> Property(string property) => new PropertyUnit<TSource>(this, property);

        public Expression<Func<TSource, bool>> Lambda => Expression.Lambda<Func<TSource, bool>>(Expression, Parameter);

    }
}
