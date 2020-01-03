using System;
using System.Linq.Expressions;

namespace NLinq
{
    public class DynamicExpressionBuilder<TSource>
    {
        public ParameterExpression Parameter { get; set; }
        public Expression Expression { get; set; }

        public DynamicExpressionBuilder()
        {
            Parameter = Expression.Parameter(typeof(TSource));
            Expression = Parameter;
        }

        public DynamicExpressionBuilder(ParameterExpression parameter, Expression expression)
        {
            Parameter = parameter;
            Expression = expression;
        }

        public PropertyUnit<TSource> Property(string property) => new PropertyUnit<TSource>(this, property);

        public Expression<Func<TSource, bool>> Lambda => Expression.Lambda<Func<TSource, bool>>(Expression, Parameter);

    }
}
