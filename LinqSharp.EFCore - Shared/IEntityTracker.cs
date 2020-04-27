using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;

namespace LinqSharp
{
    public class EntityTrackerClass : IEntityTracker<DbContext, EntityTrackerClass>
    {
        public void OnCompleting(DbContext context, EntityState state, IEnumerable<PropertyEntry> entries) => throw new NotImplementedException();
        public void OnInserting(DbContext context, IEnumerable<PropertyEntry> entries) => throw new NotImplementedException();
        public void OnUpdating(DbContext context, EntityTrackerClass origin, IEnumerable<PropertyEntry> entries) => throw new NotImplementedException();
        public void OnDeleting(DbContext context, IEnumerable<PropertyEntry> entries) => throw new NotImplementedException();
    }

    public interface IEntityTracker<TDbContext, TSelf> : IEntity
        where TSelf : class, IEntityTracker<TDbContext, TSelf>, new()
    {
        void OnCompleting(TDbContext context, EntityState state, IEnumerable<PropertyEntry> entries);
        void OnInserting(TDbContext context, IEnumerable<PropertyEntry> entries);
        void OnUpdating(TDbContext context, TSelf origin, IEnumerable<PropertyEntry> entries);
        void OnDeleting(TDbContext context, IEnumerable<PropertyEntry> entries);
    }

}
