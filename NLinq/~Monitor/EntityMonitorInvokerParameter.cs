using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

namespace NLinq
{
    public interface IEntityMonitorInvokerParameter
    {
        EntityState State { get; set; }
        object Entity { get; set; }
        IEnumerable<PropertyEntry> PropertyEntries { get; set; }
    }

    public class EntityMonitorInvokerParameter<TEntity> : IEntityMonitorInvokerParameter
        where TEntity : IEntityMonitor
    {
        public EntityState State { get; set; }
        public TEntity Entity
        {
            get => (TEntity)(this as IEntityMonitorInvokerParameter).Entity;
            set => (this as IEntityMonitorInvokerParameter).Entity = value;
        }
        public IEnumerable<PropertyEntry> PropertyEntries { get; set; }
        object IEntityMonitorInvokerParameter.Entity { get; set; }
    }

}
