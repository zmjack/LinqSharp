// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;

namespace LinqSharp.EFCore
{
    public static partial class XDbSet
    {
        public static DbSetCache<TEntity> CreateCacheQuery<TEntity>(this DbSet<TEntity> @this) where TEntity : class, new()
        {
            return new DbSetCache<TEntity>(@this);
        }

        public static DbSetCache<TEntity> CreateCacheQuery<TEntity>(this DbSet<TEntity> @this, TEntity defaultValue) where TEntity : class, new()
        {
            return new DbSetCache<TEntity>(@this, defaultValue);
        }

    }
}
