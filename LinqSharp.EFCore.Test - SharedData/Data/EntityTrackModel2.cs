using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

    public class EntityTrackModel2Audit : IEntityAudit<ApplicationDbContext, EntityTrackModel2>
    {
        public void OnAudited(ApplicationDbContext context, EntityAuditUnitContainer container)
        {
        }

        public void OnAuditing(ApplicationDbContext context, EntityAuditUnit<EntityTrackModel2>[] units)
        {
            var unitsBySuper = units.GroupBy(x => x.Current.Super);
            foreach (var group in unitsBySuper)
            {
                var super = context.EntityTrackModel1s.Find(group.Key);
                foreach (var unit in group)
                {
                    switch (unit.State)
                    {
                        case EntityState.Added: super.TotalQuantity += unit.Current.GroupQuantity; break;
                        case EntityState.Modified: super.TotalQuantity += unit.Current.GroupQuantity - unit.Origin.GroupQuantity; break;
                        case EntityState.Deleted: super.TotalQuantity -= unit.Current.GroupQuantity; break;
                    }
                }
            }
        }
    }

}
