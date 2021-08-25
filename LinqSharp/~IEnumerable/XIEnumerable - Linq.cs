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
using System.Reflection;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        private static readonly MemoryCache typeOpMethods = new(new MemoryCacheOptions());

        private static MethodInfo GetOpMethod(Type type, string name, Type[][] parameterLists)
        {
            var opMethod = typeOpMethods.GetOrCreate($"[{type.AssemblyQualifiedName}].{name}", entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(20);
                var methods = type.GetMethods();
                var matchedMethod = methods.Where(x => x.Name == name)
                    .OrderBy(p => parameterLists.IndexOf(list => p.GetParameters().Select(x => x.ParameterType).SequenceEqual(list)).For(x => x < 0 ? int.MaxValue : x))
                    .FirstOrDefault();
                return matchedMethod;
            });
            return opMethod;
        }
        private static MethodInfo GetOpAddition<TType>()
        {
            var type = typeof(TType);
            if (type.IsNullable()) type = type.GetGenericArguments()[0];
            var op_Addition = GetOpMethod(type, "op_Addition", new[]
            {
                new[] { type, type },
            });
            if (op_Addition is null) throw new InvalidOperationException($"There is no matching op_Addition method for {type.FullName}.");
            return op_Addition;
        }
        private static MethodInfo GetOpDivision<TType>()
        {
            var type = typeof(TType);
            if (type.IsNullable()) type = type.GetGenericArguments()[0];
            var opDivision = GetOpMethod(type, "op_Division", new[]
            {
                new[] { type, typeof(long) },
                new[] { type, typeof(int) },
                new[] { type, typeof(double) },
                new[] { type, typeof(float) },
            });
            if (opDivision is null) throw new InvalidOperationException($"There is no matching op_Division method for {type.FullName}.");
            return opDivision;
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
