using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqSharp.EFCore
{
    public class EntityAuditUnitContainer
    {
        private List<object> List { get; } = new List<object>();
        internal void Add(object auditUnit) => List.Add(auditUnit);

        public IEnumerable<EntityAuditUnit<TEntity>> OfType<TEntity>() where TEntity : class, new()
        {
            return List.OfType<EntityAuditUnit<TEntity>>();
        }

    }
}
