// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Translators;

namespace LinqSharp.EFCore;

public static partial class IQueryableExtensions
{
    /// <summary>
    /// Select the specified number of random record from a source set.
    /// <para>[Warning] Before calling this function, you need to open the provider functions.</para>
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="this"></param>
    /// <param name="takeCount"></param>
    /// <returns></returns>
    public static IQueryable<TSource> Random<TSource>(this IQueryable<TSource> @this, int takeCount)
        where TSource : class
    {
        return @this.OrderBy(x => DbRandom.NextDouble()).Take(takeCount);
    }

    /// <summary>
    /// Get a random record from a source set.
    /// <para> Before calling this function, you need to enable DbRandom functions. </para>
    /// <para> Use LinqSharpEF.UseTranslator&lt;DbRandom&gt;(this, modelBuilder) on ModelCreating. </para>
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="this"></param>
    /// <returns></returns>
    public static TSource Random<TSource>(this IQueryable<TSource> @this)
        where TSource : class
    {
        return @this.OrderBy(x => DbRandom.NextDouble()).Take(1).First();
    }

    /// <summary>
    /// Get a random record from a source set.
    /// <para> Before calling this function, you need to enable DbRandom functions. </para>
    /// <para> Use LinqSharpEF.UseTranslator&lt;DbRandom&gt;(this, modelBuilder) on ModelCreating. </para>
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="this"></param>
    /// <returns></returns>
    public static TSource RandomOrDefault<TSource>(this IQueryable<TSource> @this)
        where TSource : class
    {
        return @this.OrderBy(x => DbRandom.NextDouble()).Take(1).FirstOrDefault();
    }

}
