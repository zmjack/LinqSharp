using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

namespace LinqSharp.EFCore
{
    public class EntityAuditUnit<TEntity>
        where TEntity : class, new()
    {
        public EntityState State { get; set; }
        public TEntity Origin { get; set; }
        public TEntity Current { get; set; }
        public IEnumerable<PropertyEntry> PropertyEntries { get; set; }
    }
}
