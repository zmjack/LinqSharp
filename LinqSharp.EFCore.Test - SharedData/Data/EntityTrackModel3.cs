using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LinqSharp.EFCore.Data.Test
{
    [EntityAudit(typeof(EntityTrackModel3Audit))]
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

    public class EntityTrackModel3Audit : IEntityAudit<ApplicationDbContext, EntityTrackModel3>
    {
        public void OnAudited(ApplicationDbContext context, EntityAuditUnitContainer container)
        {
        }

        public void OnAuditing(ApplicationDbContext context, EntityAuditUnit<EntityTrackModel3>[] units)
        {
            var unitsBySuper = units.GroupBy(x => x.Current.Super);
            foreach (var group in unitsBySuper)
            {
                var super = context.EntityTrackModel2s.Include(x => x.SuperLink).First(x => x.Id == group.Key);
                foreach (var unit in group)
                {
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

}
