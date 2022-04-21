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
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class EntityAgent
    {
        private string _item;
        private KvEntity[] _stores;
        private bool _loaded;

        public string GetItemString() => _item;
        public KvEntity[] GetColumnStores() => _stores;
        public bool IsProxyLoaded() => _loaded;

        public void Load<TKvEntity>(IEnumerable<TKvEntity> stores, string item)
            where TKvEntity : KvEntity
        {
            if (GetType().Namespace != "Castle.Proxies") throw new InvalidOperationException("This method can only be called in a proxy instance.");
            _item = item;
            _stores = stores.Where(x => x.Item == item).ToArray();
            _loaded = true;
        }
    }

    /// <summary>
    /// Hint: Each custom properties must be virtual(public).
    /// </summary>
    /// <typeparam name="TSelf"></typeparam>
    public abstract class KvEntityAgent<TSelf, TKvEntity> : EntityAgent
        where TSelf : KvEntityAgent<TSelf, TKvEntity>, new()
        where TKvEntity : KvEntity, new()
    {
        public static TSelf Connect(IEnumerable<TKvEntity> columnStores, string item)
        {
            var proxy = new ProxyGenerator().CreateClassProxyWithTarget(new TSelf(), new EntityAgentProxy<TSelf>());
            proxy.Load(columnStores, item);
            return proxy;
        }
    }

}
