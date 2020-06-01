using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace LinqSharp.EFCore
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class KvEntityAccessor
    {
        private string _Item;
        private KvEntity[] _ColumnStores;
        private bool _ProxyLoaded;

        public string GetItemString() => _Item;
        public KvEntity[] GetColumnStores() => _ColumnStores;
        public bool IsProxyLoaded() => _ProxyLoaded;

        public void Load<TRegistryStore>(IEnumerable<TRegistryStore> columnStores, string item)
            where TRegistryStore : KvEntity
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
    public abstract class KvEntityAccessor<TSelf, TKvEntity> : KvEntityAccessor
        where TSelf : KvEntityAccessor<TSelf, TKvEntity>, new()
        where TKvEntity : KvEntity
    {
        public static TSelf Connect(IEnumerable<TKvEntity> columnStores, string item)
        {
            var proxy = new ProxyGenerator().CreateClassProxyWithTarget(new TSelf(), new KvEntityAccessorProxy<TSelf>());
            proxy.Load(columnStores, item);
            return proxy;
        }
    }

}
