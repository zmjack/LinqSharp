using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        /// <summary>
        /// Projects page elements of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="this"></param>
        /// <param name="pageNumber">'pageNumber' starts at 1</param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedEnumerable<TSource> SelectPage<TSource>(this IEnumerable<TSource> @this, int pageNumber, int pageSize)
        {
            return new PagedEnumerable<TSource>(@this, pageNumber, pageSize);
        }

        /// <summary>
        /// Calculates the max page number through the specified page size.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="this"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int PageCount<TSource>(this IEnumerable<TSource> @this, int pageSize, out int count)
        {
            count = @this switch
            {
                TSource[] array => array.Length,
                ICollection<TSource> collection => collection.Count,
                IQueryable<TSource> querable => querable.Count(),
                _ => @this.Count(),
            };
            return (int)Math.Ceiling((double)count / pageSize);
        }
        /// <summary>
        /// Calculates the max page number through the specified page size.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="this"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static int PageCount<TSource>(this IEnumerable<TSource> @this, int pageSize) => PageCount(@this, pageSize, out _);

        /// <summary>
        /// Produces the set difference of two sequences by using the specified properties to compare values.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> ExceptBy<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Expression<Func<TSource, object>> compare)
            => Enumerable.Except(first, second, new ExactEqualityComparer<TSource>(compare));

        /// <summary>
        /// Produces the set union of two sequences by using a specified properties.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> UnionBy<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Expression<Func<TSource, object>> compare)
            => Enumerable.Union(first, second, new ExactEqualityComparer<TSource>(compare));

        /// <summary>
        /// Produces the set intersection of two sequences by using the specified properties to compare values.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> IntersectBy<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Expression<Func<TSource, object>> compare)
            => Enumerable.Intersect(first, second, new ExactEqualityComparer<TSource>(compare));

    }
}
