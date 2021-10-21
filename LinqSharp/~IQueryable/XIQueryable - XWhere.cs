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
        public static IQueryable<TSource> XWhere<TSource>(this IQueryable<TSource> @this, Func<WhereHelperQ<TSource>, WhereExp<TSource>> build)
        {
            var helper = new WhereHelperQ<TSource>(@this);
            var whereExp = build(helper);

            if (whereExp?.Expression != null)
                return @this.Where(whereExp.Expression);
            else return @this;
        }

    }
}