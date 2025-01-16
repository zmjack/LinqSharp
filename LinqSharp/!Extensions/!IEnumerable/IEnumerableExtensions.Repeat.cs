// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

namespace LinqSharp;

public static partial class IEnumerableExtensions
{
    public static IEnumerable<TSource> Repeat<TSource>(this IEnumerable<TSource> source, int repeats)
    {
        foreach (var item in source)
        {
            for (int i = 0; i < repeats; i++)
            {
                yield return item;
            }
        }
    }

}
