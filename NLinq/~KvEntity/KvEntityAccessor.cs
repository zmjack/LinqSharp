using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NLinq
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

    public static class KvEntityAgent<TKvEntityAccessor>
        where TKvEntityAccessor : KvEntityAccessor, new()
    {
        public static KvEntityAgent<TDbContext, TKvEntityAccessor, TKvEntity> Create<TDbContext, TKvEntity>(TDbContext context, Func<TDbContext, DbSet<TKvEntity>> getEntities)
            where TDbContext : DbContext
            where TKvEntity : KvEntity, new()
        {
            return new KvEntityAgent<TDbContext, TKvEntityAccessor, TKvEntity>(context, getEntities);
        }
    }

    public class KvEntityAgent<TDbContext, TKvEntityAccessor, TKvEntity>
        where TDbContext : DbContext
        where TKvEntityAccessor : KvEntityAccessor, new()
        where TKvEntity : KvEntity, new()
    {
        private TDbContext DbContext;
        public Func<TDbContext, IQueryable<TKvEntity>> GetEntities;

        public KvEntityAgent(TDbContext context, Func<TDbContext, IQueryable<TKvEntity>> getEntities)
        {
            DbContext = context;
            GetEntities = getEntities;
        }

        public void EnsureItem(string item)
        {
            var ensureItems = typeof(TKvEntityAccessor).GetProperties()
                .Where(x => x.GetMethod.IsVirtual)
                .Select(x => new[]
                {
                    new EnsureCondition<TKvEntity>(c => c.Item, item),
                    new EnsureCondition<TKvEntity>(c => c.Key, x.Name),
                })
                .ToArray();

            var queryable = GetEntities(DbContext);
            foreach (var ensureItem in ensureItems)
            {
                queryable.EnsureFirst(DbContext, ensureItem);
            }
        }

        public TKvEntityAccessor this[string item] => Get(item);
        public TKvEntityAccessor Get(string item)
        {
            EnsureItem(item);

            var registry = new TKvEntityAccessor();
            var registryProxy = Activator.CreateInstance(typeof(KvEntityAccessorProxy<TKvEntityAccessor>));

            var proxy = new ProxyGenerator().CreateClassProxyWithTarget(registry, registryProxy as IInterceptor);
            proxy.Load(GetEntities(DbContext), item);
            return proxy;
        }

    }

}
