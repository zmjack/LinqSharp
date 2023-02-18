// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp
{
    public static partial class IQueryableExtensions
    {
        public static TResult MaxOrDefault<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, TResult @default = default) => source.Any() ? source.Max(selector) : @default;
        public static TSource MaxOrDefault<TSource>(this IQueryable<TSource> source, TSource @default = default) => source.Any() ? source.Max() : @default;
    }
}
