// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp.EFCore
{
    public static class EntityAccessor
    {
        public static EntityAccessor<TDbContext, TKvEntity> Create<TDbContext, TKvEntity>(TDbContext context, DbSet<TKvEntity> dbSet)
            where TDbContext : DbContext
            where TKvEntity : KvEntity, new()
        {
            return new EntityAccessor<TDbContext, TKvEntity>(context, dbSet);
        }
    }

    public class EntityAccessor<TDbContext, TKvEntity>
        where TDbContext : DbContext
        where TKvEntity : KvEntity, new()
    {
        private readonly TDbContext Context;
        private readonly DbSet<TKvEntity> DbSet;
        private readonly List<TKvEntity> Rows = new();

        public EntityAccessor(TDbContext context, DbSet<TKvEntity> dbSet)
        {
            Context = context;
            DbSet = dbSet;
        }

        public void Ensure<TEntityAgent>(string[] items) where TEntityAgent : EntityAgent, new()
        {
            var defaultAgent = typeof(TEntityAgent).CreateInstance();
            var props = typeof(TEntityAgent).GetProperties().Where(x => x.GetMethod.IsVirtual).ToArray();
            var entities = (from item in items
                            from prop in props
                            select new TKvEntity
                            {
                                Item = item,
                                Key = prop.Name,
                                Value = props.First(x => x.Name == x.Name).GetValue(defaultAgent)?.ToString(),
                            }).ToArray();

            if (entities.Length == 0) throw new InvalidOperationException($"No virtual properties could be found in `{typeof(TEntityAgent).FullName}`.");

            DbSet.AddOrUpdateRange(x => new { x.Item, x.Key }, entities);
            Context.SaveChanges();
        }

        public void Load<TEntityAgent>(params string[] items) where TEntityAgent : EntityAgent, new()
        {
            Ensure<TEntityAgent>(items);
            var rows = DbSet.Where(x => items.Contains(x.Item)).ToArray();
            Rows.AddRange(rows);
        }

        public void UnloadAll()
        {
            Rows.Clear();
        }

        public TEntityAgent Get<TEntityAgent>(string item) where TEntityAgent : EntityAgent, new()
        {
            var registry = new TEntityAgent();
            var registryProxy = new EntityAgentProxy<TEntityAgent>();
            var proxy = new ProxyGenerator().CreateClassProxyWithTarget(registry, registryProxy);

            if (Rows.Any()) proxy.Load(Rows, item);
            else proxy.Load(DbSet, item);
            return proxy;
        }
    }

}
