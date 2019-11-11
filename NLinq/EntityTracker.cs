using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLinq
{
    public class EntityTracker
    {
        public EntityEntry[] DefaultEntityEntries;
        public EntityEntry[] EntityEntries;
        private readonly ChangeTracker ChangeTracker;
        private HashSet<object> UpdateEntityList;

        public EntityTracker(ChangeTracker changeTracker, EntityEntry[] defaultEntityEntries)
        {
            ChangeTracker = changeTracker;
            DefaultEntityEntries = defaultEntityEntries;
            UpdateEntityList = new HashSet<object>();
        }

        public void Update(object entity)
        {
            UpdateEntityList.Add(entity);
        }

        public bool Fix()
        {
            if (DefaultEntityEntries != null)
            {
                EntityEntries = DefaultEntityEntries;
                DefaultEntityEntries = null;
            }
            else
            {
                EntityEntries = ChangeTracker.Entries().Where(x => UpdateEntityList.Contains(x.Entity)).ToArray();
                UpdateEntityList = new HashSet<object>();
            }

            return EntityEntries.Length > 0;
        }

    }
}
