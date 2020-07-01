using System;
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
        [Obsolete("This function does not support generating SQL.", true)]
        public static IQueryable<IGrouping<int, TSource>> GroupByCount<TSource>(this IQueryable<TSource> @this, int groupCount)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Groups the elements of a sequence according to the count of group capacity.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="this"></param>
        /// <param name="groupCount"></param>
        /// <returns></returns>
        [Obsolete("This function does not support generating SQL.", true)]
        public static IQueryable<IGrouping<int, TSource>> GroupByCount<TSource>(this IQueryable<TSource> @this, int groupCount, PadDirection padDirection)
        {
            throw new NotSupportedException();
        }
    }

}
