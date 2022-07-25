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
        /// Pad the collection to ensure that the specified key exists in the collection.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="this"></param>
        /// <param name="takeCount"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> Pad<TSource, TKey>(this IEnumerable<TSource> @this, Func<TSource, TKey> keySelector, TKey[] keys, Func<TKey, TSource> initItem)
        {
            var exsistKeys = @this.Select(x => keySelector(x)).ToArray();
            var fillItems = keys.Where(x => !exsistKeys.Contains(x)).Select(x => initItem(x)).ToArray();
            return @this.Concat(fillItems);
        }

        public static IEnumerable<TSource> PadFirst<TSource>(this IEnumerable<TSource> @this, int totalWidth)
        {
            return PadFirst(@this, totalWidth, () => default);
        }
        public static IEnumerable<TSource> PadFirst<TSource>(this IEnumerable<TSource> @this, int totalWidth, Func<TSource> createPaddingItem)
        {
            if (totalWidth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(totalWidth), "Non-negative number required.");
            }
            int count = @this.Count();
            int num = totalWidth - count;
            if (num <= 0)
            {
                return @this;
            }

            return new TSource[num].Let(createPaddingItem).Concat(@this);
        }

        public static IEnumerable<TSource> PadLast<TSource>(this IEnumerable<TSource> @this, int totalWidth)
        {
            return PadLast(@this, totalWidth, () => default);
        }
        public static IEnumerable<TSource> PadLast<TSource>(this IEnumerable<TSource> @this, int totalWidth, Func<TSource> createPaddingItem)
        {
            if (totalWidth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(totalWidth), "Non-negative number required.");
            }
            int count = @this.Count();
            int num = totalWidth - count;
            if (num <= 0)
            {
                return @this;
            }

            return @this.Concat(new TSource[num].Let(createPaddingItem));
        }

    }
}
