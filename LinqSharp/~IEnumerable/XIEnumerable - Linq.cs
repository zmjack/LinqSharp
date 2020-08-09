// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
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
