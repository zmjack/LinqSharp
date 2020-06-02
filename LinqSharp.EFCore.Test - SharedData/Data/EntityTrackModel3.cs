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
    [EntityAudit(typeof(EntityTrackModel3Auditor))]
    public class EntityTrackModel3
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(SuperLink))]
        public Guid Super { get; set; }

        [ConcurrencyCheck]
        public int Quantity { get; set; }

        public EntityTrackModel2 SuperLink { get; set; }
    }

    public class EntityTrackModel3Auditor : IEntityAuditor<ApplicationDbContext, EntityTrackModel3>
    {
        public void OnAudited(ApplicationDbContext context, EntityAuditContainer container)
        {
            var supers = container.OfType<EntityTrackModel3>().Select(x => x.Current.Super).Distinct();

            foreach (var super in supers)
            {
                var predict = container.Predict(context.EntityTrackModel3s, x => x.Super == super);
            }
        }

        public void OnAuditing(ApplicationDbContext context, EntityAuditUnit<EntityTrackModel3>[] units)
        {
            var superCaches = new CacheContainer<Guid, EntityTrackModel2>
            {
                CacheMethod = superId => () => context.EntityTrackModel2s.Include(x => x.SuperLink).First(x => x.Id == superId),
            };

            foreach (var unit in units)
            {
                var super = superCaches[unit.Current.Super].Value;
                switch (unit.State)
                {
                    case EntityState.Added:
                        super.GroupQuantity += unit.Current.Quantity;
                        super.SuperLink.TotalQuantity += unit.Current.Quantity;
                        break;
                    case EntityState.Modified:
                        super.GroupQuantity += unit.Current.Quantity - unit.Origin.Quantity;
                        super.SuperLink.TotalQuantity += unit.Current.Quantity - unit.Origin.Quantity;
                        break;
                    case EntityState.Deleted:
                        super.GroupQuantity -= unit.Current.Quantity;
                        super.SuperLink.TotalQuantity -= unit.Current.Quantity;
                        break;
                }
            }
        }
    }

}
