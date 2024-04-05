// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    /// <summary>
    /// Select the specified number of random record from a source set.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="this"></param>
    /// <param name="takeCount"></param>
    /// <returns></returns>
    public static IEnumerable<TSource> Random<TSource>(this IEnumerable<TSource> @this, int takeCount)
    {
        return @this.OrderBy(x => Guid.NewGuid()).Take(takeCount);
    }

    /// <summary>
    /// Get a random record from a source set.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="this"></param>
    /// <returns></returns>
    public static TSource Random<TSource>(this IEnumerable<TSource> @this)
    {
        return @this.OrderBy(x => Guid.NewGuid()).Take(1).First();
    }

    /// <summary>
    /// Get a random record from a source set.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="this"></param>
    /// <returns></returns>
    public static TSource? RandomOrDefault<TSource>(this IEnumerable<TSource> @this)
    {
        return @this.OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();
    }

}
