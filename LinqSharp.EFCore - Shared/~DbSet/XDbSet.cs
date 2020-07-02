// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp.EFCore
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class XDbSet
    {
        public static DbContext GetDbContext<TEntity>(this DbSet<TEntity> @this)
            where TEntity : class
        {
            var provider = (@this as IInfrastructure<IServiceProvider>).Instance;
            var context = (provider.GetService(typeof(ICurrentDbContext)) as ICurrentDbContext).Context;
            return context;
        }

        [Obsolete("The API is part of the experimental feature and may be modified or removed in the future.")]
        public static UpdateWrapper<TEntity> TryUpdate<TEntity>(this DbSet<TEntity> @this, Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return new UpdateWrapper<TEntity>(new WhereWrapper<TEntity>(@this, expression));
        }

        [Obsolete("The API is part of the experimental feature and may be modified or removed in the future.")]
        public static DeleteWrapper<TEntity> TryDelete<TEntity>(this DbSet<TEntity> @this, Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return new DeleteWrapper<TEntity>(new WhereWrapper<TEntity>(@this, expression));
        }

        public static string ToSql<TEntity>(this DbSet<TEntity> @this) where TEntity : class => LinqSharp.XIQueryable.ToSql(@this.Where(x => true));

    }
}
