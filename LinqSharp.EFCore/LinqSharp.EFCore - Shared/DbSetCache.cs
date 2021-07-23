// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqSharp.EFCore
{
    public class DbSetCache<TEntity> where TEntity : class, new()
    {
        public DbSet<TEntity> DbSet { get; }
        public TEntity DefaultValue { get; }
        public readonly Dictionary<QueryCondition<TEntity>, TEntity> RecordCache = new();

        public DbSetCache(DbSet<TEntity> dbSet)
        {
            DbSet = dbSet;
        }

        public DbSetCache(DbSet<TEntity> dbSet, TEntity defaultValue) : this(dbSet)
        {
            DefaultValue = defaultValue;
        }

        public TEntity Query(QueryCondition<TEntity> condition)
        {
            if (RecordCache.ContainsKey(condition)) return RecordCache[condition];

            var props = typeof(TEntity).GetProperties().Where(x => x.CanRead && x.CanWrite);
            var parameter = Expression.Parameter(typeof(TEntity));
            var body = condition.UnitList
                .Select(u =>
                {
                    var property = Expression.Property(parameter, props.First(p => p.Name == u.PropName));
                    return (Expression)Expression.Equal(property, Expression.Constant(u.ExpectedValue));
                })
                .Aggregate((a, b) => Expression.AndAlso(a, b));
            var exp = Expression.Lambda<Func<TEntity, bool>>(body, parameter);
            var func = exp.Compile();

            var value = DbSet.Local.FirstOrDefault(func) ?? DbSet.FirstOrDefault(exp) ?? DefaultValue;
            RecordCache[condition] = value;
            return value;
        }

        public void Track(Expression<Func<TEntity, bool>> predicate)
        {
            DbSet.Where(predicate).ToArray();
        }

    }
}
