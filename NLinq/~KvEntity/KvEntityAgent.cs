using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLinq._KvEntity
{
    public abstract class KvEntityAgent<TKvEntityAccessor>
        where TKvEntityAccessor : KvEntityAccessor, new()
    {
        public static KvEntityAgent<TKvEntityAccessor> Create<TDbContext, TKvEntity>(TDbContext context, Func<TDbContext, DbSet<TKvEntity>> getEntities)
            where TDbContext : DbContext
            where TKvEntity : KvEntity, new()
        {
            return new KvEntityAgent<TDbContext, TKvEntityAccessor, TKvEntity>(context, getEntities);
        }

        public abstract void EnsureItem(string item);
        public abstract TKvEntityAccessor Get(string item);
        public TKvEntityAccessor this[string item] => Get(item);
    }

    public class KvEntityAgent<TDbContext, TKvEntityAccessor, TKvEntity> : KvEntityAgent<TKvEntityAccessor>
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

        public override void EnsureItem(string item)
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

        public override TKvEntityAccessor Get(string item)
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
