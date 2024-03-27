// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using LinqSharp.EFCore.Query;
using LinqSharp.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Reflection;

namespace LinqSharp.EFCore;

public static partial class DbSetExtensions
{
    private static Expression<Func<TEntity, bool>> GetAbsoluteAddOrUpdateLambda<TEntity>(string[] propNames, TEntity entity)
    {
        var record = Expression.Parameter(typeof(TEntity));
        var entityExp = Expression.Constant(entity);

        var parts = propNames.Select(name =>
        {
            var left = Expression.Property(record, name);

            var rightProperty = Expression.Property(entityExp, name);
            Expression right;
            if (rightProperty.Member is PropertyInfo prop)
            {
                right = Expression.Property(entityExp, name);
            }
            else throw new InvalidOperationException("Right property must be PropertyInfo.");

            var lambda = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(left, right), record);
            return lambda;
        }).ToArray();

        var predicate = parts.LambdaJoin(Expression.AndAlso);
        return predicate;
    }

    private static Expression<Func<TEntity, TEntity, bool>> GetAddOrUpdateLambda<TEntity>(string[] propNames)
    {
        var record = Expression.Parameter(typeof(TEntity), "record");
        var entity = Expression.Parameter(typeof(TEntity), "entity");

        var parts = propNames.Select(name =>
        {
            var left = Expression.Property(record, name);

            var rightProperty = Expression.Property(entity, name);
            Expression right;
            if (rightProperty.Member is PropertyInfo prop)
            {
                right = Expression.Property(entity, name);
            }
            else throw new InvalidOperationException("Right property must be PropertyInfo.");

            var lambda = Expression.Lambda<Func<TEntity, TEntity, bool>>(Expression.Equal(left, right), record, entity);
            return lambda;
        }).ToArray();

        var predicate = parts.LambdaJoin(Expression.AndAlso);
        return predicate;
    }

    /// <summary>
    /// Add an entity if not exsist, or update the exsist record.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="this"></param>
    /// <param name="keys">[Member or NewSelector]</param>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static EntityEntry<TEntity> AddOrUpdate<TEntity>(this DbSet<TEntity> @this, Expression<Func<TEntity, object>> keys, ref TEntity entity)
        where TEntity : class
    {
        return AddOrUpdate(@this, keys, ref entity, null);
    }

    /// <summary>
    /// Add an entity if not exsist, or update the exsist record.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="this"></param>
    /// <param name="keys">[Member or NewSelector]</param>
    /// <param name="entity"></param>
    /// <param name="initOptions"></param>
    /// <returns></returns>
    public static EntityEntry<TEntity> AddOrUpdate<TEntity>(this DbSet<TEntity> @this, Expression<Func<TEntity, object>> keys, ref TEntity entity, Action<UpdateOptions<TEntity>> initOptions)
        where TEntity : class
    {
        var options = new UpdateOptions<TEntity>();
        initOptions?.Invoke(options);

        var propNames = IncludesExpression.GetPropertyNames(keys);
        var predicate = GetAbsoluteAddOrUpdateLambda(propNames, entity);

        var record = @this.FirstOrDefault(predicate);
        if (record is not null)
        {
            options.Update?.Invoke(record, entity);
            entity = record;
            return @this.GetDbContext().Entry(record);
        }
        else return @this.Add(entity);
    }

    /// <summary>
    /// Add many entities if not exsist, or update the exsist records.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="this"></param>
    /// <param name="keys">[Member or NewSelector]</param>
    /// <param name="entities"></param>
    public static void AddOrUpdateRange<TEntity>(this DbSet<TEntity> @this, Expression<Func<TEntity, object>> keys, TEntity[] entities)
        where TEntity : class
    {
        AddOrUpdateRange(@this, keys, entities, null);
    }

    /// <summary>
    /// Add many entities if not exsist, or update the exsist records.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="this"></param>
    /// <param name="keys">[Member or NewSelector]</param>
    /// <param name="entities"></param>
    /// <param name="initOptions"></param>
    public static void AddOrUpdateRange<TEntity>(this DbSet<TEntity> @this, Expression<Func<TEntity, object>> keys, TEntity[] entities, Action<UpdateOptions<TEntity>> initOptions)
        where TEntity : class
    {
        if (!entities.Any()) return;

        var options = new UpdateOptions<TEntity>();
        initOptions?.Invoke(options);

        var propNames = IncludesExpression.GetPropertyNames(keys);
        var predicateBuilder = GetAddOrUpdateLambda<TEntity>(propNames).Compile();

        Expression<Func<TEntity, bool>> predicate;
        if (options.Predicate is null)
        {
            var parts = entities.Select(x => new
            {
                Entity = x,
                Predicate = GetAbsoluteAddOrUpdateLambda(propNames, x),
            }).ToArray();
            var lambdas = parts.Select(x => x.Predicate).ToArray();
            predicate = lambdas.LambdaJoin(Expression.OrElse);
        }
        else predicate = options.Predicate;

        var update = options.Update;
        var recordlist = @this.Where(predicate).ToList();
        foreach (ref var entity in entities.AsSpan())
        {
            var _entity = entity;
            var record = recordlist.FirstOrDefault(x => predicateBuilder(x, _entity));
            if (record is null) @this.Add(entity);
            else
            {
                update?.Invoke(record, entity);
                entity = record;
                recordlist.Remove(record);
            }
        }
    }

}
