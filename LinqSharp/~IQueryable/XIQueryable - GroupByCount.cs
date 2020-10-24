// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

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
        [Obsolete("This function does not support generating.", true)]
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
        [Obsolete("This function does not support generating.", true)]
        public static IQueryable<IGrouping<int, TSource>> GroupByCount<TSource>(this IQueryable<TSource> @this, int groupCount, PadDirection padDirection)
        {
            throw new NotSupportedException();
        }
    }

}
