using Microsoft.EntityFrameworkCore;
using NStandard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LinqSharp.EFCore
{
    public class EntityAuditContainer
    {
        private List<object> List { get; } = new List<object>();
        internal void Add(object auditUnit) => List.Add(auditUnit);

        public IEnumerable<EntityAuditUnit<TEntity>> OfType<TEntity>() where TEntity : class, new()
        {
            return List.OfType<EntityAuditUnit<TEntity>>();
        }

        public TEntity[] Predict<TEntity>(DbSet<TEntity> dbSet, Func<TEntity, bool> predicate)
            where TEntity : class, new()
        {
            var locals = OfType<TEntity>();
            var localsOfAdded = locals.Where(x => x.State == EntityState.Added).ToArray();
            var localsOfModified = locals.Where(x => x.State == EntityState.Modified).ToArray();
            var localsOfDeleted = locals.Where(x => x.State == EntityState.Deleted).ToArray();
            var stores = dbSet.Where(predicate).ToArray();

            var keyProps = typeof(TEntity).GetProperties().Where(x => x.HasAttribute<KeyAttribute>());
            var ret = stores.Where(store =>
            {
                var ret = !localsOfModified.Any(x => keyProps.All(keyProp => keyProp.GetValue(x.Current).Equals(keyProp.GetValue(store))))
                       && !localsOfDeleted.Any(x => keyProps.All(keyProp => keyProp.GetValue(x.Current).Equals(keyProp.GetValue(store))));
                return ret;
            })
                .Concat(localsOfAdded.Select(x => x.Current))
                .Concat(localsOfModified.Select(x => x.Current))
                .ToArray();

            return ret;
        }

    }
}
