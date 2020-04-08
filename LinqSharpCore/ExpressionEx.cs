using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp
{
    public static class ExpressionEx
    {
        public static string[] GetPropertyNames<TEntity>(Expression<Func<TEntity, object>> memberOrNewExp)
        {
            string[] propNames;
            switch (memberOrNewExp.Body)
            {
                case MemberExpression exp:
                    propNames = new[] { exp.Member.Name };
                    break;

                case NewExpression exp:
                    propNames = exp.Members.Select(x => x.Name).ToArray();
                    break;

                case UnaryExpression exp:
                    if (exp.NodeType == ExpressionType.Convert)
                    {
                        var mexp = (exp.Operand as MemberExpression);
                        if (mexp != null)
                        {
                            propNames = new[] { mexp.Member.Name };
                            break;
                        }
                        else goto default;
                    }
                    else goto default;

                default:
                    throw new NotSupportedException("This argument must be MemberExpression or NewExpression.");
            }

            return propNames;
        }

        public static IEnumerable<PropertyInfo> GetProperties<TEntity>(Expression<Func<TEntity, object>> memberOrNewExp)
        {
            var propNames = GetPropertyNames(memberOrNewExp);
            var type = typeof(TEntity);
            var props = type.GetProperties().Where(x => propNames.Contains(x.Name));
            return props;
        }

    }
}
