﻿// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Castle.DynamicProxy;
using LinqSharp.EFCore.Agent;
using LinqSharp.EFCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NStandard;
using System.Runtime.CompilerServices;

namespace LinqSharp.EFCore.Scopes;

public static class AgentQueryScope
{
    public static InvalidOperationException NoScopeException => new("This operation needs to be contained within a AgentQuery scope.");
}

public sealed class AgentQueryScope<T> : Scope<AgentQueryScope<T>> where T : KeyValueEntity, new()
{
    private static readonly MemoryCache _agentProperties = new(new MemoryCacheOptions());

    public DbContext Context { get; }
    public DbSet<T> DbSet { get; }

    private readonly Dictionary<string, KeyValueAgent<T>> _agentList = new();
    private readonly List<string> _uncreatedNameList = new();

    internal AgentQueryScope(DbContext context, DbSet<T> dbSet)
    {
        Context = context;
        DbSet = dbSet;
    }

    public TAgent GetAgent<TAgent>(string itemName) where TAgent : KeyValueAgent<T>, new()
    {
        if (!_agentList.ContainsKey(itemName))
        {
            _uncreatedNameList.Add(itemName);
        }

        var proxy = new KeyValueAgentProxy<TAgent, T>();
        var agent = new ProxyGenerator().CreateClassProxyWithTarget(new TAgent(), proxy);
        agent._itemName = itemName;

        _agentList[itemName] = agent;

        return agent;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Execute()
    {
        if (_uncreatedNameList.Any())
        {
            var itemNamesByType = _uncreatedNameList.GroupBy(name => _agentList[name].GetType().BaseType!);
            foreach (var itemNames in itemNamesByType)
            {
                var agentType = itemNames.Key;
                var defaultAgent = agentType.CreateInstance();

                var props = _agentProperties.GetOrCreate(agentType, entry =>
                {
                    return agentType.GetProperties().Where(x => x.GetMethod!.IsVirtual).ToArray();
                })!;

                var entities = (
                    from name in itemNames
                    from prop in props
                    select new T
                    {
                        Item = name,
                        Key = prop.Name,
                        Value = props.First(x => x.Name == prop.Name).GetValue(defaultAgent)?.ToString(),
                    }
                ).ToArray();

                if (entities.Length == 0) throw new InvalidOperationException($"No virtual properties could be found in `{agentType.FullName}`.");

                DbSet.AddOrUpdateRange(x => new { x.Item, x.Key }, entities, options =>
                {
                    var items = itemNames.ToArray();
                    options.Predicate = x => items.Contains(x.Item);
                });

                foreach (var name in itemNames)
                {
                    var agent = _agentList[name];
                    agent._executed = true;
                    agent._entities = entities.Where(x => x.Item == name).ToArray();
                }
            }

            _uncreatedNameList.Clear();
        }
    }

    public override void Disposing()
    {
        Execute();
    }
}
