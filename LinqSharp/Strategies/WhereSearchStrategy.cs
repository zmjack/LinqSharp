using NStandard;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.Strategies
{
    public class WhereSearchStrategy<TEntity> : WhereStringStrategy<TEntity>
    {
        private static readonly MethodInfo _Method_Enumerable_op_Any = typeof(Enumerable)
            .GetMethodViaQualifiedName("Boolean Any[TSource](System.Collections.Generic.IEnumerable`1[TSource], System.Func`2[TSource,System.Boolean])")
            .MakeGenericMethod(typeof(string));
        private static readonly MethodInfo _Method_String_op_Contains = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
        private static readonly MethodInfo _Method_String_op_Equals = typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(string) });

        public WhereSearchStrategy(string searchString, Expression<Func<TEntity, object>> searchMembers, SearchOption option)
        {
            Func<Expression, Expression, Expression> compareExp;
            MethodInfo stringMethod;

            switch (option)
            {
                case SearchOption.Contains:
                case SearchOption.NotContains: stringMethod = _Method_String_op_Contains; break;

                case SearchOption.Equal:
                case SearchOption.NotEqual: stringMethod = _Method_String_op_Equals; break;

                default: throw new NotSupportedException();
            }

            compareExp = (singlePartOfInExp, secharStringExp) => singlePartOfInExp.For(exp =>
            {
                if (exp.Type == typeof(string))
                    return Expression.Call(singlePartOfInExp, typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) }), secharStringExp);
                else if (exp.Type.GetInterface(typeof(IEnumerable).FullName) != null)
                {
                    var parameter = Expression.Parameter(typeof(string));
                    Expression<Func<string, bool>> lambda;

                    switch (option)
                    {
                        case SearchOption.Contains:
                        case SearchOption.Equal:
                            lambda = Expression.Lambda<Func<string, bool>>(
                                Expression.Call(parameter, stringMethod, secharStringExp),
                                parameter);
                            break;

                        case SearchOption.NotContains:
                        case SearchOption.NotEqual:
                            lambda = Expression.Lambda<Func<string, bool>>(
                                Expression.Not(Expression.Call(parameter, stringMethod, secharStringExp)),
                                parameter);
                            break;

                        default: throw new NotSupportedException();
                    }

                    return Expression.Call(_Method_Enumerable_op_Any, singlePartOfInExp, lambda);
                }
                else throw new NotSupportedException();
            });

            Init(searchMembers, compareExp, searchString ?? "");
        }

    }
}
