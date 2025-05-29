// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Navigation;
using System.ComponentModel;
using System.Linq.Expressions;

namespace LinqSharp.EFCore;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class IIncludableExtensions
{
    public static IIncludable<TEntity, TProperty> Include<TEntity, TPreviousProperty, TProperty>(this IIncludable<TEntity, TPreviousProperty> @this, Expression<Func<TEntity, TProperty>> target)
        where TEntity : class
        where TProperty : class
    {
        return @this.Owner.Include(target);
    }

    public static IIncludable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(this IIncludable<TEntity, IEnumerable<TPreviousProperty>> @this, Expression<Func<TPreviousProperty, TProperty>> target)
        where TEntity : class
        where TProperty : class
    {
        var navigation = new IncludeNavigation<TEntity, TProperty>(@this.Owner, @this.TargetPath);
        @this.TargetPath.Add(new QueryTarget
        {
            PreviousProperty = typeof(IEnumerable<TPreviousProperty>),
            PreviousPropertyElement = typeof(TPreviousProperty),
            Property = typeof(TProperty),
            Expression = target,
        });
        return navigation;
    }

    public static IIncludable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(this IIncludable<TEntity, TPreviousProperty> @this, Expression<Func<TPreviousProperty, TProperty>> target)
        where TEntity : class
        where TPreviousProperty : class
        where TProperty : class
    {
        var navigation = new IncludeNavigation<TEntity, TProperty>(@this.Owner, @this.TargetPath);
        @this.TargetPath.Add(new QueryTarget
        {
            PreviousProperty = typeof(TPreviousProperty),
            Property = typeof(TProperty),
            Expression = target,
        });
        return navigation;
    }

}
