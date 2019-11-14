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
        private HashSet<object> HasUpdatedEntityList;

        public EntityTracker(ChangeTracker changeTracker, EntityEntry[] defaultEntityEntries)
        {
            ChangeTracker = changeTracker;
            DefaultEntityEntries = defaultEntityEntries;
            UpdateEntityList = new HashSet<object>();
            HasUpdatedEntityList = new HashSet<object>();
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
                foreach (var entry in EntityEntries)
                    HasUpdatedEntityList.Add(entry.Entity);

                EntityEntries = ChangeTracker.Entries()
                    .Where(x => !HasUpdatedEntityList.Contains(x.Entity))
                    .Where(x => UpdateEntityList.Contains(x.Entity)).ToArray();
                UpdateEntityList = new HashSet<object>();
            }

            return EntityEntries.Length > 0;
        }

    }
}
