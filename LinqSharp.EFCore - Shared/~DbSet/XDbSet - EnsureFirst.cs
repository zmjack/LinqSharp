// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LinqSharp.EFCore
{
    public static partial class XIQueryable
    {
        public static TEntity EnsureFirst<TEntity>(this DbSet<TEntity> @this, EnsureCondition<TEntity> condition)
            where TEntity : class, new()
        {
            return EnsureMany(@this, new[] { condition }, null).First();
        }

        public static TEntity EnsureFirst<TEntity>(this DbSet<TEntity> @this, EnsureCondition<TEntity> condition, Action<EnsureOptions<TEntity>> initOptions)
            where TEntity : class, new()
        {
            return EnsureMany(@this, new[] { condition }, initOptions).First();
        }

    }
}
