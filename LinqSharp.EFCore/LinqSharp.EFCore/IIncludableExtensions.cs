// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Navigation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace LinqSharp.EFCore
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class IIncludableExtensions
    {
        public static IIncludable<TDbContext, TEntity, TProperty> Include<TDbContext, TEntity, TPreviousProperty, TProperty>(this IIncludable<TDbContext, TEntity, TPreviousProperty> @this, Expression<Func<TEntity, TProperty>> target)
            where TDbContext : DbContext
            where TEntity : class
            where TProperty : class
        {
            return @this.Owner.Include(target);
        }

        public static IIncludable<TDbContext, TEntity, TProperty> ThenInclude<TDbContext, TEntity, TPreviousProperty, TProperty>(this IIncludable<TDbContext, TEntity, IEnumerable<TPreviousProperty>> @this, Expression<Func<TPreviousProperty, TProperty>> target)
            where TDbContext : DbContext
            where TEntity : class
            where TProperty : class
        {
            var navigation = new IncludeNavigation<TDbContext, TEntity, TProperty>(@this.Owner, @this.TargetPath);
            @this.TargetPath.Add(new QueryTarget
            {
                PreviousProperty = typeof(IEnumerable<TPreviousProperty>),
                PreviousPropertyElement = typeof(TPreviousProperty),
                Property = typeof(TProperty),
                Expression = target,
            });
            return navigation;
        }

        public static IIncludable<TDbContext, TEntity, TProperty> ThenInclude<TDbContext, TEntity, TPreviousProperty, TProperty>(this IIncludable<TDbContext, TEntity, TPreviousProperty> @this, Expression<Func<TPreviousProperty, TProperty>> target)
            where TDbContext : DbContext
            where TEntity : class
            where TPreviousProperty : class
            where TProperty : class
        {
            var navigation = new IncludeNavigation<TDbContext, TEntity, TProperty>(@this.Owner, @this.TargetPath);
            @this.TargetPath.Add(new QueryTarget
            {
                PreviousProperty = typeof(TPreviousProperty),
                Property = typeof(TProperty),
                Expression = target,
            });
            return navigation;
        }

    }
}
