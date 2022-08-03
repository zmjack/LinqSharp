// Copyright 2020 zmjack
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// See the LICENSE file in the project root for more information.

using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LinqSharp.EFCore
{
    /// <summary>
    /// [ Each custom properties must be virtual(public). ]
    /// </summary>
    /// <typeparam name="TSelf"></typeparam>
    /// <typeparam name="TKeyValueEntity"></typeparam>
    public abstract class KeyValueAgent<TSelf, TKeyValueEntity>
        where TSelf : KeyValueAgent<TSelf, TKeyValueEntity>, new()
        where TKeyValueEntity : KeyValueEntity, new()
    {
        internal string _item;
        internal KeyValueEntity[] _entities;

        internal void SetEntities(TKeyValueEntity[] keyValueEntities, string item)
        {
            if (GetType().Namespace != "Castle.Proxies") throw new InvalidOperationException("This method can only be called in a proxy instance.");
            _item = item;
            _entities = keyValueEntities;
        }

        public string GetItemName() => _item;

        public static TSelf Attach(IEnumerable<TKeyValueEntity> keyValueEntities, string item)
        {
            var proxy = new ProxyGenerator().CreateClassProxyWithTarget(new TSelf(), new KeyValueAgentProxy<TSelf, TKeyValueEntity>());
            proxy.SetEntities(keyValueEntities.Where(x => x.Item == item).ToArray(), item);
            return proxy;
        }
    }

}
