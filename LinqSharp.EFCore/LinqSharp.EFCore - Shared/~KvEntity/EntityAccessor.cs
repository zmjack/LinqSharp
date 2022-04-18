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
        public static EntityAccessor<TKvEntity> Create<TKvEntity>(DbSet<TKvEntity> dbSet)
            where TKvEntity : KvEntity, new()
        {
            return new EntityAccessor<TKvEntity>(dbSet);
        }
    }

    public class EntityAccessor<TKvEntity>
        where TKvEntity : KvEntity, new()
    {
        private readonly DbSet<TKvEntity> DbSet;

        public EntityAccessor(DbSet<TKvEntity> dbSet)
        {
            DbSet = dbSet;
        }

        public EntityAccessor<TEntityAgent, TKvEntity> GetEntityAccessor<TEntityAgent>() where TEntityAgent : EntityAgent, new()
        {
            return new EntityAccessor<TEntityAgent, TKvEntity>(DbSet);
        }

        public EntityAccessor<TEntityAgent, TKvEntity> GetEntityAccessor<TEntityAgent>(params string[] items) where TEntityAgent : EntityAgent, new()
        {
            var accessor = new EntityAccessor<TEntityAgent, TKvEntity>(DbSet);
            accessor.Load(items);
            return accessor;
        }
    }

    public class EntityAccessor<TEntityAgent, TKvEntity>
        where TEntityAgent : EntityAgent, new()
        where TKvEntity : KvEntity, new()
    {
        public DbSet<TKvEntity> DbSet;
        public TKvEntity[] Rows;

        public EntityAccessor(DbSet<TKvEntity> dbSet)
        {
            DbSet = dbSet;
        }

        public void Ensure(string[] items)
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
        }

        public void Load(params string[] items)
        {
            Ensure(items);
            Rows = DbSet.Where(x => items.Contains(x.Item)).ToArray();
        }

        public TEntityAgent Get(string item)
        {
            var registry = new TEntityAgent();
            var registryProxy = new EntityAgentProxy<TEntityAgent>();
            var proxy = new ProxyGenerator().CreateClassProxyWithTarget(registry, registryProxy);

            if (Rows is not null) proxy.Load(Rows, item);
            else proxy.Load(DbSet, item);
            return proxy;
        }

    }
}
