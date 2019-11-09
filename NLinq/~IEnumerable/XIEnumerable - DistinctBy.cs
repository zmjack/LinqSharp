using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NLinq
{
    public static partial class XIEnumerable
    {
        /// <summary>
        /// Returns distinct elements from a sequence by using a specified properties to compare values.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="compares_MemberOrNewExp"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, object>> compares_MemberOrNewExp)
            => Enumerable.Distinct(source, new ExactEqualityComparer<TSource>(compares_MemberOrNewExp));

    }
}
