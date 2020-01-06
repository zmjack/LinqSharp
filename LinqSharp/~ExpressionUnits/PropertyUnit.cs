using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp
{
    public class PropertyUnit<TSource>
    {
        public readonly DynamicExpressionBuilder<TSource> Builder;
        public readonly string PropertyName;

        public PropertyUnit(DynamicExpressionBuilder<TSource> builder, string propertyName)
        {
            Builder = builder;
            PropertyName = propertyName;
        }

        public DynamicExpressionBuilder<TSource> Invoke(MethodInfo method, params object[] parameters)
        {
            Builder.Expression = Expression.Call(Expression.Property(Builder.Expression, PropertyName), method, parameters.Select(x => Expression.Constant(x)));
            return Builder;
        }

        public DynamicExpressionBuilder<TSource> Invoke(MethodInfo method, params Expression[] parameters)
        {
            Builder.Expression = Expression.Call(Expression.Property(Builder.Expression, PropertyName), method, parameters);
            return Builder;
        }

    }
}
