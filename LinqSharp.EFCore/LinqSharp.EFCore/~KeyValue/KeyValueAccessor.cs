// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Castle.DynamicProxy;
using LinqSharp.EFCore.Agent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private static readonly MemoryCache _agentProperties = new(new MemoryCacheOptions());
        private readonly TDbContext _context;
        private readonly DbSet<TKeyValueEntity> _dbSet;
        private readonly Dictionary<string, TKeyValueEntity[]> _itemDictionary = new();

        public KeyValueAccessor(TDbContext context, DbSet<TKeyValueEntity> dbSet)
        {
            _context = context;
            _dbSet = dbSet;
        }

        public void Load<TEntityAgent>(params string[] items) where TEntityAgent : KeyValueAgent<TEntityAgent, TKeyValueEntity>, new()
        {
            if (!items.Any()) throw new ArgumentException("Items can not be empty.", nameof(items));

            var agentType = typeof(TEntityAgent);
            var defaultAgent = agentType.CreateInstance();

            var props = _agentProperties.GetOrCreate(agentType, entry =>
            {
                return agentType.GetProperties().Where(x => x.GetMethod.IsVirtual).ToArray();
            });
            var entities = (
                from item in items
                from prop in props
                select new TKeyValueEntity
                {
                    Item = item,
                    Key = prop.Name,
                    Value = props.First(x => x.Name == prop.Name).GetValue(defaultAgent)?.ToString(),
                }
            ).ToArray();

            if (entities.Length == 0) throw new InvalidOperationException($"No virtual properties could be found in `{agentType.FullName}`.");

            _dbSet.AddOrUpdateRange(x => new { x.Item, x.Key }, entities, options =>
            {
                options.Predicate = x => items.Contains(x.Item);
            });
            _context.SaveChanges();

            var groups = entities.GroupBy(x => x.Item);
            foreach (var group in groups)
            {
                _itemDictionary[group.Key] = group.ToArray();
            }
        }

        public TKeyValueAgent GetItem<TKeyValueAgent>(string item) where TKeyValueAgent : KeyValueAgent<TKeyValueAgent, TKeyValueEntity>, new()
        {
            var registry = new TKeyValueAgent();
            var registryProxy = new KeyValueAgentProxy<TKeyValueAgent, TKeyValueEntity>();
            var proxy = new ProxyGenerator().CreateClassProxyWithTarget(registry, registryProxy);

            if (!_itemDictionary.ContainsKey(item))
            {
                Load<TKeyValueAgent>(item);
            }

            proxy.SetEntities(_itemDictionary[item], item);
            return proxy;
        }
    }

}
