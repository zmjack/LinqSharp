using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        /// <summary>
        /// Groups the elements of a sequence according to the count of group capacity.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="this"></param>
        /// <param name="groupCount"></param>
        /// <returns></returns>
        [Obsolete("This function does not generate SQL and runs locally.")]
        public static IEnumerable<IGrouping<int, TSource>> GroupByCount<TSource>(this IQueryable<TSource> @this, int groupCount)
        {
            return @this.AsEnumerable().GroupByCount(groupCount, PadDirection.Default);
        }

        /// <summary>
        /// Groups the elements of a sequence according to the count of group capacity.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="this"></param>
        /// <param name="groupCount"></param>
        /// <returns></returns>
        [Obsolete("This function does not generate SQL and runs locally.")]
        public static IEnumerable<IGrouping<int, TSource>> GroupByCount<TSource>(this IQueryable<TSource> @this, int groupCount, PadDirection padDirection)
        {
            return @this.AsEnumerable().GroupByCount(groupCount, padDirection);
        }

    }

}
