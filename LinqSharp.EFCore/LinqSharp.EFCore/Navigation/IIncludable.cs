// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinqSharp.EFCore.Navigation
{
    public interface IIncludable<TDbContext, TEntity, out TProperty>
        where TDbContext : DbContext
        where TEntity : class
    {
        PreQuery<TDbContext, TEntity> Owner { get; }
        List<QueryTarget> TargetPath { get; }
    }

    public static class IncludeNavigationExtensions
    {
        public static PreQuery<TDbContext, TEntity> Where<TDbContext, TEntity, TPreviousProperty>(this IIncludable<TDbContext, TEntity, TPreviousProperty> @this, Expression<Func<TEntity, bool>> predicate)
            where TDbContext : DbContext
            where TEntity : class
        {
            return @this.Owner.Where(predicate);
        }

        public static PreQuery<TDbContext, TEntity> Filter<TDbContext, TEntity, TPreviousProperty>(this IIncludable<TDbContext, TEntity, TPreviousProperty> @this, Func<QueryHelper<TEntity>, QueryExpression<TEntity>> filter)
            where TDbContext : DbContext
            where TEntity : class
        {
            return @this.Owner.Filter(filter);
        }

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
