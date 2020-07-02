// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp.Dev
{
    public static partial class XIEnumerable
    {
        /// <summary>
        /// Returns distinct elements from a sequence by using a specified properties to compare values.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="compares_MemberOrNewExp"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource>(this IEnumerable<TSource> source, Expression<Func<TSource, object>> compares_MemberOrNewExp)
        {
            return Enumerable.Distinct(source, new ExactEqualityComparer<TSource>(compares_MemberOrNewExp));
        }
    }
}
