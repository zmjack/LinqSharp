// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace LinqSharp
{
    public static partial class XIQueryable
    {
        /// <summary>
        /// Pad the collection to ensure that the specified key exists in the collection.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="this"></param>
        /// <param name="keySelector"></param>
        /// <param name="keys"></param>
        /// <param name="initItem"></param>
        /// <returns></returns>
        /// 
        [Obsolete("This function does not support generating.", true)]
        public static IQueryable<TSource> Pad<TSource, TKey>(this IQueryable<TSource> @this, Func<TSource, TKey> keySelector, TKey[] keys, Func<TKey, TSource> initItem)
        {
            throw new NotSupportedException();
        }

    }
}
