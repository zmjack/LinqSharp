// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using System;

namespace LinqSharp.EFCore
{
    public static partial class XDbSet
    {
        [Obsolete("Use DbContext.CreatePreQuery instead. This method is slower than AddOrUpdate method, and maybe removed in the future.")]
        public static DbSetCache<TEntity> CreateCacheQuery<TEntity>(this DbSet<TEntity> @this) where TEntity : class, new()
        {
            return new DbSetCache<TEntity>(@this);
        }

        [Obsolete("Use DbContext.CreatePreQuery instead. This method is slower than AddOrUpdate method, and maybe removed in the future.")]
        public static DbSetCache<TEntity> CreateCacheQuery<TEntity>(this DbSet<TEntity> @this, TEntity defaultValue) where TEntity : class, new()
        {
            return new DbSetCache<TEntity>(@this, defaultValue);
        }

    }
}
