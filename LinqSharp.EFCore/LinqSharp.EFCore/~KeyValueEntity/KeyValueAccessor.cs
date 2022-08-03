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
    public static class KeyValueAccessor
    {
        public static KeyValueAccessor<TDbContext, TKeyValueEntity> Create<TDbContext, TKeyValueEntity>(TDbContext context, DbSet<TKeyValueEntity> dbSet)
            where TDbContext : DbContext
            where TKeyValueEntity : KeyValueEntity, new()
        {
            return new KeyValueAccessor<TDbContext, TKeyValueEntity>(context, dbSet);
        }
    }

    public class KeyValueAccessor<TDbContext, TKeyValueEntity>
        where TDbContext : DbContext
        where TKeyValueEntity : KeyValueEntity, new()
    {
        private readonly TDbContext Context;
        private readonly DbSet<TKeyValueEntity> DbSet;
        private readonly Dictionary<string, TKeyValueEntity[]> ItemDictionary = new();

        public KeyValueAccessor(TDbContext context, DbSet<TKeyValueEntity> dbSet)
        {
            Context = context;
            DbSet = dbSet;
        }

        private TKeyValueEntity[] Ensure<TEntityAgent>(string item) where TEntityAgent : KeyValueAgent<TEntityAgent, TKeyValueEntity>, new()
        {
            var defaultAgent = typeof(TEntityAgent).CreateInstance();
            var props = typeof(TEntityAgent).GetProperties().Where(x => x.GetMethod.IsVirtual).ToArray();
            var entities = (
                from prop in props
                select new TKeyValueEntity
                {
                    Item = item,
                    Key = prop.Name,
                    Value = props.First(x => x.Name == x.Name).GetValue(defaultAgent)?.ToString(),
                }
            ).ToArray();

            if (entities.Length == 0) throw new InvalidOperationException($"No virtual properties could be found in `{typeof(TEntityAgent).FullName}`.");

            DbSet.AddOrUpdateRange(x => new { x.Item, x.Key }, entities, options =>
            {
                options.Predicate = x => x.Item == item;
            });
            Context.SaveChanges();

            return entities;
        }

        public TKeyValueAgent GetItem<TKeyValueAgent>(string item) where TKeyValueAgent : KeyValueAgent<TKeyValueAgent, TKeyValueEntity>, new()
        {
            var registry = new TKeyValueAgent();
            var registryProxy = new KeyValueAgentProxy<TKeyValueAgent, TKeyValueEntity>();
            var proxy = new ProxyGenerator().CreateClassProxyWithTarget(registry, registryProxy);

            if (!ItemDictionary.ContainsKey(item))
            {
                ItemDictionary[item] = Ensure<TKeyValueAgent>(item);
            }

            proxy.Load(ItemDictionary[item], item);
            return proxy;
        }
    }

}
