// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.Strategies;

public class QuerySearchStrategy<TEntity> : QueryStringStrategy<TEntity>
{
    private static readonly MethodInfo _Method_Enumerable_op_Any = typeof(Enumerable)
        .GetMethodViaQualifiedName("Boolean Any[TSource](System.Collections.Generic.IEnumerable`1[TSource], System.Func`2[TSource,System.Boolean])")
        .MakeGenericMethod(typeof(string));
    private static readonly MethodInfo _Method_String_op_Contains = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
    private static readonly MethodInfo _Method_String_op_Equals1 = typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(string) });
    private static readonly MethodInfo _Method_String_op_Equals2 = typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(string), typeof(string) });
    //private static readonly MethodInfo _Method_String_op_Inequality = typeof(string).GetMethod("op_Inequality");

    public QuerySearchStrategy(string searchString, Expression<Func<TEntity, object>> searchMembers, SearchOption option)
    {
        Func<Expression, Expression, Expression> compareExp;
        MethodInfo stringMethod;

        switch (option)
        {
            case SearchOption.Contains:
            case SearchOption.NotContains: stringMethod = _Method_String_op_Contains; break;

            case SearchOption.Equals:
            case SearchOption.NotEquals: stringMethod = _Method_String_op_Equals1; break;

            default: throw new NotSupportedException();
        }

        compareExp = (selectorExp, secharStringExp) =>
        {
            if (selectorExp.Type == typeof(string))
            {
                switch (option)
                {
                    case SearchOption.Contains:
                    case SearchOption.Equals:
                        return
                            Expression.AndAlso(
                                Expression.Not(Expression.Call(_Method_String_op_Equals2, selectorExp, Expression.Constant(null, typeof(string)))),
                                Expression.Call(selectorExp, stringMethod, secharStringExp));

                    case SearchOption.NotContains:
                    case SearchOption.NotEquals:
                        return
                            Expression.AndAlso(
                                Expression.Not(Expression.Call(_Method_String_op_Equals2, selectorExp, Expression.Constant(null, typeof(string)))),
                                Expression.Not(Expression.Call(selectorExp, stringMethod, secharStringExp)));

                    default: throw new NotImplementedException();
                }
            }
            else if (selectorExp.Type.GetInterface(typeof(IEnumerable).FullName) is not null)
            {
                var parameter = Expression.Parameter(typeof(string));
                Expression<Func<string, bool>> lambda;

                switch (option)
                {
                    case SearchOption.Contains:
                    case SearchOption.Equals:
                        lambda = Expression.Lambda<Func<string, bool>>(
                            Expression.AndAlso(
                                Expression.Not(Expression.Call(_Method_String_op_Equals2, parameter, Expression.Constant(null, typeof(string)))),
                                Expression.Call(parameter, stringMethod, secharStringExp)),
                            parameter);
                        break;

                    case SearchOption.NotContains:
                    case SearchOption.NotEquals:
                        lambda = Expression.Lambda<Func<string, bool>>(
                            Expression.AndAlso(
                                Expression.Not(Expression.Call(_Method_String_op_Equals2, parameter, Expression.Constant(null, typeof(string)))),
                                Expression.Not(Expression.Call(parameter, stringMethod, secharStringExp))),
                            parameter);
                        break;

                    default: throw new NotImplementedException();
                }

                return Expression.Call(_Method_Enumerable_op_Any, selectorExp, lambda);
            }
            else throw new NotSupportedException();
        };

        Init(searchMembers, compareExp, searchString ?? "");
    }

}
