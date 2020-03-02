using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LinqSharp
{
    public abstract class KvEntityAgent<TKvEntityAccessor>
        where TKvEntityAccessor : KvEntityAccessor, new()
    {
        public static KvEntityAgent<TKvEntityAccessor, TKvEntity> Create<TKvEntity>(DbSet<TKvEntity> dbSet)
            where TKvEntity : KvEntity, new()
        {
            return new KvEntityAgent<TKvEntityAccessor, TKvEntity>(dbSet);
        }

        public abstract void EnsureItem(string item);
        public abstract TKvEntityAccessor Get(string item);
        public TKvEntityAccessor this[string item] => Get(item);
    }

    public class KvEntityAgent<TKvEntityAccessor, TKvEntity> : KvEntityAgent<TKvEntityAccessor>
        where TKvEntityAccessor : KvEntityAccessor, new()
        where TKvEntity : KvEntity, new()
    {
        public DbSet<TKvEntity> DbSet;

        public KvEntityAgent(DbSet<TKvEntity> dbSet)
        {
            DbSet = dbSet;
        }

        public override void EnsureItem(string item)
        {
            var ensureItems = typeof(TKvEntityAccessor).GetProperties()
                .Where(x => x.GetMethod.IsVirtual)
                .Select(x => new EnsureCondition<TKvEntity>
                {
                    [c => c.Item] = item,
                    [c => c.Key] = x.Name,
                }).ToArray();

            foreach (var ensureItem in ensureItems)
            {
                DbSet.EnsureFirst(ensureItem);
            }
        }

        public override TKvEntityAccessor Get(string item)
        {
            EnsureItem(item);

            var registry = new TKvEntityAccessor();
            var registryProxy = Activator.CreateInstance(typeof(KvEntityAccessorProxy<TKvEntityAccessor>));

            var proxy = new ProxyGenerator().CreateClassProxyWithTarget(registry, registryProxy as IInterceptor);
            proxy.Load(DbSet, item);
            return proxy;
        }

    }
}
