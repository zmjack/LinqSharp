using System;
using System.Linq;
using System.Linq.Expressions;

namespace NLinq
{
    public static partial class XIQueryable
    {
        /// <summary>
        /// Projects page elements of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="this"></param>
        /// <param name="pageNumber">'pageNumber' starts at 1</param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedQueryable<TSource> SelectPage<TSource>(this IQueryable<TSource> @this, int pageNumber, int pageSize)
            => new PagedQueryable<TSource>(@this, pageNumber, pageSize);

        /// <summary>
        /// Calculates the max page number through the specified page size.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="this"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static int PageCount<TSource>(this IQueryable<TSource> @this, int pageSize)
            => (int)Math.Ceiling((double)@this.Count() / pageSize);

        public static IQueryable<TSource> WhereNot<TSource>(this IQueryable<TSource> @this, Expression<Func<TSource, bool>> predicate)
            => @this.Where(Expression.Lambda<Func<TSource, bool>>(Expression.Not(predicate.Body), predicate.Parameters));

        //TODO: Pending Delete

        ///// <summary>
        ///// Produces the set union of two sequences by using a specified properties.
        ///// </summary>
        ///// <typeparam name="TSource"></typeparam>
        ///// <param name="first"></param>
        ///// <param name="second"></param>
        ///// <param name="compare"></param>
        ///// <returns></returns>
        //public static IQueryable<TSource> UnionByValue<TSource>(this IQueryable<TSource> first, IQueryable<TSource> second, Func<TSource, object> compare)
        //    => Queryable.Union(first, second, new ExactEqualityComparer<TSource>(compare));

        ///// <summary>
        ///// Produces the set intersection of two sequences by using the specified properties to compare values.
        ///// </summary>
        ///// <typeparam name="TSource"></typeparam>
        ///// <param name="first"></param>
        ///// <param name="second"></param>
        ///// <param name="compare"></param>
        ///// <returns></returns>
        //public static IQueryable<TSource> IntersectByValue<TSource>(this IQueryable<TSource> first, IQueryable<TSource> second, Func<TSource, object> compare)
        //    => Queryable.Intersect(first, second, new ExactEqualityComparer<TSource>(compare));
    }

}
