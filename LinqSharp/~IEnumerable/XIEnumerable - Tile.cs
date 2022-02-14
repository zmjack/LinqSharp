// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static IEnumerable<TSource> Tile<TSource>(this IEnumerable<TSource> source, int repeats)
        {
            for (int i = 0; i < repeats; i++)
            {
                foreach (var item in source)
                {
                    yield return item;
                }
            }
        }

    }

}
