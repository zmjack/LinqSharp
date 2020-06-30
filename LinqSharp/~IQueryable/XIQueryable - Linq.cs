using System;
using System.Linq;

namespace LinqSharp
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
        {
            return new PagedQueryable<TSource>(@this, pageNumber, pageSize);
        }

        /// <summary>
        /// Calculates the max page number through the specified page size.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="this"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static int PageCount<TSource>(this IQueryable<TSource> @this, int pageSize)
        {
            return (int)Math.Ceiling((double)@this.Count() / pageSize);
        }

    }
}
