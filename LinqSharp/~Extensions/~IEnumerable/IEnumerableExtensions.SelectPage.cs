// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Query;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class IEnumerableExtensions
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

            if (count == 0) return 0;
            else return ((count - 1) / pageSize) + 1;
        }
        /// <summary>
        /// Calculates the max page number through the specified page size.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="this"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static int PageCount<TSource>(this IEnumerable<TSource> @this, int pageSize) => PageCount(@this, pageSize, out _);

    }
}
