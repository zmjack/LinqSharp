// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using NStandard;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp.EFCore
{
    public static partial class XIQueryable
    {
        public static TEntity[] EnsureMany<TEntity>(this DbSet<TEntity> @this, EnsureCondition<TEntity>[] conditions)
            where TEntity : class, new()
        {
            return EnsureMany(@this, conditions, null);
        }

        public static TEntity[] EnsureMany<TEntity>(this DbSet<TEntity> @this, EnsureCondition<TEntity>[] conditions, Action<EnsureOptions<TEntity>> initOptions)
            where TEntity : class, new()
        {
            if (conditions.Length == 0) return new TEntity[0];

            var options = new EnsureOptions<TEntity>();
            initOptions?.Invoke(options);

            var context = @this.GetDbContext();
            var expressions = conditions.Select(x => x.GetExpression()).ToArray();

            Expression<Func<TEntity, bool>> predicate;
            if (options.Predicate is null)
            {
                var parameter = expressions[0].Parameters[0];
                foreach (var exp in expressions) exp.RebindParameter(exp.Parameters[0], parameter);
                predicate = expressions.LambdaJoin(Expression.OrElse);
            }
            else predicate = options.Predicate;

            var exsists = @this.Where(predicate).ToArray();
            var conditionsForAdd = conditions.Where(condition => !exsists.Any(condition.GetExpression().Compile()));

            foreach (var conditionForAdd in conditionsForAdd)
            {
                var item = new TEntity();
                foreach (var unit in conditionForAdd.UnitList)
                {
                    var prop = typeof(TEntity).GetProperty(unit.PropName);
                    prop.SetValue(item, unit.ExpectedValue);
                }
                options.Set?.Invoke(item);
                context.Add(item);
            }
            context.SaveChanges();

            return @this.Where(predicate).ToArray();
        }

    }
}
