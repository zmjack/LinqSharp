// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        /// <summary>
        /// Groups the elements of a sequence according to the count of group capacity.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="this"></param>
        /// <param name="groupCount"></param>
        /// <returns></returns>
        public static IEnumerable<IGrouping<int, TSource>> GroupByCount<TSource>(this IEnumerable<TSource> @this, int groupCount)
        {
            return GroupByCount(@this, groupCount, PadDirection.Default);
        }

        /// <summary>
        /// Groups the elements of a sequence according to the count of group capacity.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="this"></param>
        /// <param name="groupCount"></param>
        /// <returns></returns>
        public static IEnumerable<IGrouping<int, TSource>> GroupByCount<TSource>(this IEnumerable<TSource> @this, int groupCount, PadDirection padDirection)
        {
            switch (padDirection)
            {
                case PadDirection.Default:
                case PadDirection.Right:
                    return @this
                        .Select((v, i) => new { Key = i, Value = v })
                        .GroupBy(x => x.Key / groupCount, x => x.Value);

                case PadDirection.Left:
                    var count = @this.Count();
                    return @this
                        .Select((v, i) => new { Key = i, Value = v })
                        .GroupBy(x => (x.Key + (groupCount - count % groupCount)) / groupCount, x => x.Value);

                default: throw new NotSupportedException();
            }
        }

    }

}
