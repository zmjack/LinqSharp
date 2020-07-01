using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp.Dev
{
    public static partial class XIQueryable
    {
        public static IQueryable<TSource> WhereNot<TSource>(this IQueryable<TSource> @this, Expression<Func<TSource, bool>> predicate)
        {
            return @this.Where(Expression.Lambda<Func<TSource, bool>>(Expression.Not(predicate.Body), predicate.Parameters));
        }

        /// <summary>
        /// Produces the set union of two sequences by using a specified properties.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static IQueryable<TSource> UnionByValue<TSource>(this IQueryable<TSource> first, IQueryable<TSource> second, Expression<Func<TSource, object>> compare)
        {
            return Queryable.Union(first, second, new ExactEqualityComparer<TSource>(compare));
        }

        /// <summary>
        /// Produces the set intersection of two sequences by using the specified properties to compare values.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static IQueryable<TSource> IntersectByValue<TSource>(this IQueryable<TSource> first, IQueryable<TSource> second, Expression<Func<TSource, object>> compare)
        {
            return Queryable.Intersect(first, second, new ExactEqualityComparer<TSource>(compare));
        }
    }

}
