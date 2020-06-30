using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp
{
    public class PropertyUnit<TSource>
    {
        public readonly WhereExpBuilder<TSource> Builder;
        public readonly string PropertyName;

        public PropertyUnit(WhereExpBuilder<TSource> builder, string propertyName)
        {
            Builder = builder;
            PropertyName = propertyName;
        }

        public WhereExpBuilder<TSource> Invoke(MethodInfo method, params object[] parameters)
        {
            Builder.Expression = Expression.Call(Expression.Property(Builder.Expression, PropertyName), method, parameters.Select(x => Expression.Constant(x)));
            return Builder;
        }

        public WhereExpBuilder<TSource> Invoke(MethodInfo method, params Expression[] parameters)
        {
            Builder.Expression = Expression.Call(Expression.Property(Builder.Expression, PropertyName), method, parameters);
            return Builder;
        }

    }
}
