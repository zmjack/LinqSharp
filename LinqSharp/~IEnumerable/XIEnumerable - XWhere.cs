// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp
{
    public static partial class XIEnumerable
    {
        public static IEnumerable<TSource> XWhere<TSource>(this IEnumerable<TSource> @this, Func<WhereHelper<TSource>, WhereExp<TSource>> build)
        {
            var helper = new WhereHelper<TSource>();
            var whereExp = build(helper);

            if (whereExp.Expression is not null)
            {
                var predicate = whereExp.Expression.Compile();
                return @this.Where(predicate);
            }
            else return @this;
        }

    }
}