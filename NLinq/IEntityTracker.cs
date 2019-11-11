using Microsoft.EntityFrameworkCore;
using System;

namespace NLinq
{
    public class DefEntityTracker : IEntityTracker<DbContext, DefEntityTracker>
    {
        public void OnCompleting(DbContext context, EntityState state) => throw new NotImplementedException();
        public void OnInserting(DbContext context, EntityTracker tracker) => throw new NotImplementedException();
        public void OnUpdating(DbContext context, EntityTracker tracker, DefEntityTracker origin) => throw new NotImplementedException();
        public void OnDeleting(DbContext context, EntityTracker tracker) => throw new NotImplementedException();
    }

    public interface IEntityTracker<TDbContext, TSelf> : IEntity
        where TSelf : class, IEntityTracker<TDbContext, TSelf>, new()
    {
        void OnCompleting(TDbContext context, EntityState state);
        void OnInserting(TDbContext context, EntityTracker tracker);
        void OnUpdating(TDbContext context, EntityTracker tracker, TSelf origin);
        void OnDeleting(TDbContext context, EntityTracker tracker);
    }

}
