// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.EFCore
{
    public static partial class XDbSet
    {
        private static Expression<Func<TEntity, bool>> GetAddOrUpdateLambda<TEntity>(string[] propNames, TEntity entity)
        {
            var param = Expression.Parameter(typeof(TEntity));
            var entityExp = Expression.Constant(entity);
            var parts = propNames.Select(name =>
            {
                var left = Expression.Property(param, name);

                var rightProperty = Expression.Property(entityExp, name);
                object rightValue;
                if (rightProperty.Member is PropertyInfo prop)
                {
                    if (prop.PropertyType.IsValueType)
                    {
                        var body = Expression.Convert(Expression.Property(entityExp, name), typeof(object));
                        rightValue = Expression.Lambda<Func<object>>(body).Compile()();
                    }
                    else
                    {
                        var body = Expression.Property(entityExp, name);
                        rightValue = Expression.Lambda<Func<object>>(body).Compile()();
                    }
                }
                else throw new InvalidOperationException("Right property must be PropertyInfo.");

                var right = Expression.Constant(rightValue);
                var lambda = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(left, right), param);

                return lambda;
            }).ToArray();

            var predicate = parts.LambdaJoin(Expression.AndAlso);
            return predicate;
        }

        public static EntityEntry<TEntity> AddOrUpdate<TEntity>(this DbSet<TEntity> @this, Expression<Func<TEntity, object>> memberOrNewSelector, TEntity entity)
            where TEntity : class
        {
            return AddOrUpdate(@this, memberOrNewSelector, entity, null);
        }

        public static EntityEntry<TEntity> AddOrUpdate<TEntity>(this DbSet<TEntity> @this, Expression<Func<TEntity, object>> memberOrNewSelector, TEntity entity, Action<UpdateOptions<TEntity>> initOptions)
            where TEntity : class
        {
            var options = new UpdateOptions<TEntity>();
            initOptions?.Invoke(options);

            var propNames = ExpressionEx.GetPropertyNames(memberOrNewSelector);
            var predicate = GetAddOrUpdateLambda(propNames, entity);

            var record = @this.FirstOrDefault(predicate);
            if (record is not null)
            {
                options.Update(record, entity);
                return @this.GetDbContext().Entry(record);
            }
            else return @this.Add(entity);
        }

        public static void AddOrUpdateRange<TEntity>(this DbSet<TEntity> @this, Expression<Func<TEntity, object>> memberOrNewSelector, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            AddOrUpdateRange(@this, memberOrNewSelector, entities, null);
        }

        public static void AddOrUpdateRange<TEntity>(this DbSet<TEntity> @this, Expression<Func<TEntity, object>> memberOrNewSelector, params TEntity[] entities)
            where TEntity : class
        {
            AddOrUpdateRange(@this, memberOrNewSelector, entities, null);
        }

        public static void AddOrUpdateRange<TEntity>(this DbSet<TEntity> @this, Expression<Func<TEntity, object>> memberOrNewSelector, IEnumerable<TEntity> entities, Action<UpdateOptions<TEntity>> initOptions)
            where TEntity : class
        {
            var options = new UpdateOptions<TEntity>();
            initOptions?.Invoke(options);

            var propNames = ExpressionEx.GetPropertyNames(memberOrNewSelector);
            var parts = entities.Select(x => new
            {
                Entity = x,
                Predicate = GetAddOrUpdateLambda(propNames, x),
            });

            Expression<Func<TEntity, bool>> predicate;
            if (options.Predicate is null)
            {
                var lambdas = parts.Select(x => x.Predicate).ToArray();
                predicate = lambdas.LambdaJoin(Expression.OrElse);
            }
            else predicate = options.Predicate;

            var records = @this.Where(predicate).ToArray();
            foreach (var part in parts)
            {
                var partPredicate = part.Predicate.Compile();
                var entity = part.Entity;
                var record = records.FirstOrDefault(partPredicate);

                if (record is not null)
                {
                    options.Update(record, entity);
                    @this.GetDbContext().Entry(record);
                }
                else @this.Add(entity);
            }
        }

    }
}
