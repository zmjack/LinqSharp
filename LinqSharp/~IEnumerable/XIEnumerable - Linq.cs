// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Caching.Memory;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        private static readonly MemoryCache typeOpMethods = new(new MemoryCacheOptions());

        private static Func<TObj, TRight, TResult> GetOpMethod<TObj, TRight, TResult>(string name, Type[][] paramLists)
        {
            var objType = typeof(TObj);
            var rightType = typeof(TRight);

            var opMethod = typeOpMethods.GetOrCreate($"[{objType.FullName}].{name}({objType.FullName}, {rightType.FullName})", entry =>
            {
                var objTypeFinal = objType;
                if (objTypeFinal.IsNullable())
                {
                    objTypeFinal = objTypeFinal.GetGenericArguments()[0];
                }
                var rightTypeFinal = rightType;
                if (rightTypeFinal.IsNullable())
                {
                    rightTypeFinal = rightTypeFinal.GetGenericArguments()[0];
                }

                var methods = objTypeFinal.GetMethods().Where(x => x.Name == name);
                var method = methods.FirstOrDefault(m =>
                {
                    var methodParams = m.GetParameters().Select(x => x.ParameterType).ToArray();
                    return methodParams[0] == objTypeFinal && methodParams[1] == rightTypeFinal;
                });

                if (method is null && (paramLists?.Any() ?? false))
                {
                    method = methods.OrderBy(p =>
                    {
                        var methodParams = p.GetParameters().Select(x => x.ParameterType).ToArray();
                        var index = paramLists.IndexOf(list => methodParams.SequenceEqual(list));
                        return index < 0 ? int.MaxValue : index;
                    }).FirstOrDefault();
                }

                if (method is null) return null;

                var methodParams = method.GetParameters().Select(x => x.ParameterType).ToArray();

                var resultType = typeof(TResult);
                var p0 = Expression.Parameter(objType);
                var p1 = Expression.Parameter(rightType);
                var arg0 = methodParams[0] == objType ? p0 : (Expression)Expression.Convert(p0, methodParams[0]);
                var arg1 = methodParams[1] == rightType ? p1 : (Expression)Expression.Convert(p1, methodParams[1]);
                var body = method.ReturnType == resultType
                    ? Expression.Call(method, arg0, arg1)
                    : (Expression)Expression.Convert(Expression.Call(method, arg0, arg1), resultType);

                var func = Expression.Lambda<Func<TObj, TRight, TResult>>(body, p0, p1).Compile();
                return func;
            });
            return opMethod;
        }
        private static Func<TType, TType, TType> GetOpAddition<TType>()
        {
            var type = typeof(TType);
            if (type.IsNullable()) type = type.GetGenericArguments()[0];

            var op_Addition = GetOpMethod<TType, TType, TType>("op_Addition", new[]
            {
                new[] { type, type },
            });

            if (op_Addition is null) throw new InvalidOperationException($"There is no matching op_Addition method for {type.FullName}.");
            return op_Addition;
        }
        private static Func<TType, long, TType> GetOpDivision<TType>()
        {
            var type = typeof(TType);
            if (type.IsNullable()) type = type.GetGenericArguments()[0];

            var op_Division = GetOpMethod<TType, long, TType>("op_Division", new[]
            {
                new[] { type, typeof(long) },
                new[] { type, typeof(int) },
                new[] { type, typeof(double) },
                new[] { type, typeof(float) },
            });

            if (op_Division is null) throw new InvalidOperationException($"There is no matching op_Division method for {type.FullName}.");
            return op_Division;
        }

        /// <summary>
        /// Produces the set difference of two sequences by using the specified properties to compare values.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> ExceptBy<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Expression<Func<TSource, object>> compare)
        {
            return Enumerable.Except(first, second, new ExactEqualityComparer<TSource>(compare));
        }

        /// <summary>
        /// Produces the set union of two sequences by using a specified properties.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> UnionBy<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Expression<Func<TSource, object>> compare)
        {
            return Enumerable.Union(first, second, new ExactEqualityComparer<TSource>(compare));
        }

        /// <summary>
        /// Produces the set intersection of two sequences by using the specified properties to compare values.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> IntersectBy<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Expression<Func<TSource, object>> compare)
        {
            return Enumerable.Intersect(first, second, new ExactEqualityComparer<TSource>(compare));
        }
    }
}
