using Microsoft.EntityFrameworkCore;
using System;

namespace LinqSharp
{
    public class DefEntityTracker : IEntityTracker<DbContext, DefEntityTracker>
    {
        public void OnCompleting(DbContext context, EntityState state) => throw new NotImplementedException();
        public void OnInserting(DbContext context) => throw new NotImplementedException();
        public void OnUpdating(DbContext context, DefEntityTracker origin) => throw new NotImplementedException();
        public void OnDeleting(DbContext context) => throw new NotImplementedException();
    }

    public interface IEntityTracker<TDbContext, TSelf> : IEntity
        where TSelf : class, IEntityTracker<TDbContext, TSelf>, new()
    {
        void OnCompleting(TDbContext context, EntityState state);
        void OnInserting(TDbContext context);
        void OnUpdating(TDbContext context, TSelf origin);
        void OnDeleting(TDbContext context);
    }

}
