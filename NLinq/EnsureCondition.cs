using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NLinq
{
    public class EnsureCondition<TEntity> where TEntity : new()
    {
        public readonly Expression<Func<TEntity, object>> Expression;
        public readonly object ExpectedValue;

        public EnsureCondition(Expression<Func<TEntity, object>> expression, object expectedValue)
        {
            Expression = expression;
            ExpectedValue = expectedValue;
        }
    }
}
