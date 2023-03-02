// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Castle.DynamicProxy;
using LinqSharp.EFCore.Agent;
using LinqSharp.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSharp.EFCore.Scopes
{
    public static class AgentQuery
    {
        public static InvalidOperationException NoScopeException => new("This operation needs to be contained within a AgentQuery scope.");
    }

    public class AgentQuery<TEntity> : Scope<AgentQuery<TEntity>> where TEntity : KeyValueEntity, new()
    {
        private static readonly MemoryCache _agentProperties = new(new MemoryCacheOptions());

        public DbContext Context { get; }
        public DbSet<TEntity> DbSet { get; }

        private readonly List<string> _loadedItemList = new();
        private readonly List<TEntity> _loadedEntities = new();

        private readonly HashSet<string> _uncreatedItemList = new();

        internal AgentQuery(DbContext context, DbSet<TEntity> dbSet)
        {
            Context = context;
            DbSet = dbSet;
        }

        public TAgent GetAgent<TAgent>(string itemName) where TAgent : KeyValueAgent<TEntity>, new()
        {
            if (!_loadedItemList.Contains(itemName))
            {
                _uncreatedItemList.Add(itemName);
            }

            var agent = new TAgent();
            var agentProxy = new KeyValueAgentProxy<TAgent, TEntity>();
            var proxy = new ProxyGenerator().CreateClassProxyWithTarget(agent, agentProxy);
            proxy.ItemName = itemName;
            return proxy;
        }

        internal TEntity[] GetEntities<TAgent>(string item) where TAgent : KeyValueAgent<TEntity>, new()
        {
            if (!_loadedItemList.Contains(item)) Sync<TAgent>();
            return _loadedEntities.Where(x => x.Item == item).ToArray();
        }

        private void Sync<TAgent>() where TAgent : KeyValueAgent<TEntity>, new()
        {
            if (_uncreatedItemList.Any())
            {
                var agentType = typeof(TAgent);
                var defaultAgent = agentType.CreateInstance();

                var props = _agentProperties.GetOrCreate(agentType, entry =>
                {
                    return agentType.GetProperties().Where(x => x.GetMethod.IsVirtual).ToArray();
                });

                var entities = (
                    from item in _uncreatedItemList
                    from prop in props
                    select new TEntity
                    {
                        Item = item,
                        Key = prop.Name,
                        Value = props.First(x => x.Name == prop.Name).GetValue(defaultAgent)?.ToString(),
                    }
                ).ToArray();

                if (entities.Length == 0) throw new InvalidOperationException($"No virtual properties could be found in `{agentType.FullName}`.");

                DbSet.AddOrUpdateRange(x => new { x.Item, x.Key }, entities, options =>
                {
                    var items = _uncreatedItemList.ToArray();
                    options.Predicate = x => items.Contains(x.Item);
                });

                _loadedItemList.AddRange(_uncreatedItemList);
                _loadedEntities.AddRange(entities);
                _uncreatedItemList.Clear();
            }
        }

    }
}
