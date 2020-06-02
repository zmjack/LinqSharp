using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;

namespace LinqSharp.EFCore
{
    public interface IEntityAuditor<TDbContext, TEntity> : IEntity
        where TDbContext : DbContext
        where TEntity : class, new()
    {
        //void OnCompleting(TDbContext context, EntityState state, IEnumerable<PropertyEntry> entries);
        //void OnInserting(TDbContext context, IEnumerable<PropertyEntry> entries);
        //void OnUpdating(TDbContext context, TSelf origin, IEnumerable<PropertyEntry> entries);
        //void OnDeleting(TDbContext context, IEnumerable<PropertyEntry> entries);

        void BeforeAudit(TDbContext context, EntityAudit<TEntity>[] audits);
        void OnAuditing(TDbContext context, EntityAudit<TEntity>[] audits);
        void OnAudited(TDbContext context, AuditPredictor predictor);
    }

}
