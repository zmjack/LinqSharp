using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LinqSharp.EFCore
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class KvEntityAgent
    {
        private string _Item;
        private KvEntity[] _ColumnStores;
        private bool _ProxyLoaded;

        public string GetItemString() => _Item;
        public KvEntity[] GetColumnStores() => _ColumnStores;
        public bool IsProxyLoaded() => _ProxyLoaded;

        public void Load<TKvEntity>(IEnumerable<TKvEntity> columnStores, string item)
            where TKvEntity : KvEntity
        {
            if (GetType().Namespace != "Castle.Proxies")
                throw new InvalidOperationException("This method can only be called in a proxy instance.");

            _Item = item;
            _ColumnStores = columnStores.Where(x => x.Item == item).ToArray();
            _ProxyLoaded = true;
        }
    }

    /// <summary>
    /// Hint: Each custom properties must be virtual(public).
    /// </summary>
    /// <typeparam name="TSelf"></typeparam>
    public abstract class KvEntityAgent<TSelf, TKvEntity> : KvEntityAgent
        where TSelf : KvEntityAgent<TSelf, TKvEntity>, new()
        where TKvEntity : KvEntity, new()
    {
        public static TSelf Connect(IEnumerable<TKvEntity> columnStores, string item)
        {
            var proxy = new ProxyGenerator().CreateClassProxyWithTarget(new TSelf(), new KvEntityAgentProxy<TSelf>());
            proxy.Load(columnStores, item);
            return proxy;
        }
    }

}
