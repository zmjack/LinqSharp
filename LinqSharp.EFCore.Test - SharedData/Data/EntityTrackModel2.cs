using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NStandard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LinqSharp.EFCore.Data.Test
{
    [EntityAudit(typeof(EntityTrackModel2Audit))]
    public class EntityTrackModel2
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(SuperLink))]
        public Guid Super { get; set; }

        [ConcurrencyCheck]
        public int GroupQuantity { get; set; }

        public EntityTrackModel1 SuperLink { get; set; }

        public virtual ICollection<EntityTrackModel3> EntityTrackModel3s { get; set; }
    }

    public class EntityTrackModel2Audit : IEntityAuditor<ApplicationDbContext, EntityTrackModel2>
    {
        public void OnAudited(ApplicationDbContext context, EntityAuditPredictor container)
        {
            var supers = container.Pick<EntityTrackModel2>().Select(x => x.Current.Super).Distinct();

            foreach (var super in supers)
            {
                var predict = container.Predict(context.EntityTrackModel2s, x => x.Super == super);
            }
        }

        public void OnAuditing(ApplicationDbContext context, EntityAudit<EntityTrackModel2>[] audits)
        {
            var superCaches = new CacheContainer<Guid, EntityTrackModel1>
            {
                CacheMethod = superId => () => context.EntityTrackModel1s.Find(superId),
            };

            foreach (var audit in audits)
            {
                var super = superCaches[audit.Current.Super].Value;
                switch (audit.State)
                {
                    case EntityState.Added: super.TotalQuantity += audit.Current.GroupQuantity; break;
                    case EntityState.Modified: super.TotalQuantity += audit.Current.GroupQuantity - audit.Origin.GroupQuantity; break;
                    case EntityState.Deleted: super.TotalQuantity -= audit.Current.GroupQuantity; break;
                }
            }
        }
    }

}
