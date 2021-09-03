// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp.EFCore
{
    public static partial class XDbSet
    {
        [Obsolete("Use AddOrUpdate instead. This method is slower than AddOrUpdate method, and maybe removed in the future.")]
        public static TEntity Ensure<TEntity>(this DbSet<TEntity> @this, QueryCondition<TEntity> condition) where TEntity : class, new()
        {
            return Ensure(@this, new[] { condition }, null).First();
        }

        [Obsolete("Use AddOrUpdate instead. This method is slower than AddOrUpdate method, and maybe removed in the future.")]
        public static TEntity Ensure<TEntity>(this DbSet<TEntity> @this, QueryCondition<TEntity> condition, Action<QueryOptions<TEntity>> initOptions) where TEntity : class, new()
        {
            return Ensure(@this, new[] { condition }, initOptions).First();
        }

        [Obsolete("Use AddOrUpdate instead. This method is slower than AddOrUpdateRange method, and maybe removed in the future.")]
        public static TEntity[] Ensure<TEntity>(this DbSet<TEntity> @this, QueryCondition<TEntity>[] conditions) where TEntity : class, new()
        {
            return Ensure(@this, conditions, null);
        }

        [Obsolete("Use AddOrUpdate instead. This method is slower than AddOrUpdateRange method, and maybe removed in the future.")]
        public static TEntity[] Ensure<TEntity>(this DbSet<TEntity> @this, QueryCondition<TEntity>[] conditions, Action<QueryOptions<TEntity>> initOptions) where TEntity : class, new()
        {
            if (conditions.Length == 0) return new TEntity[0];

            var options = new QueryOptions<TEntity>();
            initOptions?.Invoke(options);

            var context = @this.GetDbContext();

            Expression<Func<TEntity, bool>> predicate;
            if (options.Predicate is null)
            {
                var expressions = conditions.Select(x => x.GetExpression()).ToArray();
                var parameter = expressions[0].Parameters[0];
                foreach (var exp in expressions) exp.RebindParameter(exp.Parameters[0], parameter);
                predicate = expressions.LambdaJoin(Expression.OrElse);
            }
            else predicate = options.Predicate;

            var exsists = @this.Where(predicate).ToArray();
            if (options.Condition?.Invoke(exsists) ?? false) return exsists;

            var pairs = conditions.Select(c => new { Condition = c, Predicate = c.GetExpression().Compile() });
            var pairsForAdd = pairs.Where(pair => !exsists.Any(pair.Predicate)).ToArray();

            foreach (var pair in pairsForAdd)
            {
                var item = new TEntity();
                foreach (var unit in pair.Condition.UnitList)
                {
                    unit.Property.SetValue(item, unit.ExpectedValue);
                }
                options.Set?.Invoke(item);
                context.Add(item);
            }
            context.SaveChanges();

            return @this.Where(predicate).ToArray();
        }

    }
}
