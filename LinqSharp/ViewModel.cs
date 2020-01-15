using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp
{
    public static class ViewModel<TModel>
        where TModel : new()
    {
        public static readonly Type InstanceType = typeof(TModel);

        public static string DisplayName<TRet>(Expression<Func<TModel, TRet>> expression)
        {
            var exp = expression.Body as MemberExpression;
            if (exp is null)
                throw new NotSupportedException("This argument 'expression' must be MemberExpression.");

            return DataAnnotationEx.GetDisplayName(exp.Member);
        }

        public static string DisplayShortName<TRet>(Expression<Func<TModel, TRet>> expression)
        {
            var exp = expression.Body as MemberExpression;
            if (exp is null)
                throw new NotSupportedException("This argument 'expression' must be MemberExpression.");

            return exp.Member.GetCustomAttribute<DisplayAttribute>()?.ShortName ?? exp.Member.Name;
        }

    }
}
