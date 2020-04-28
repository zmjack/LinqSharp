using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LinqSharp.EFCore.Data.Test
{
    public class EntityTrackModel3 : IEntityTracker<ApplicationDbContext, EntityTrackModel3>
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(SuperLink))]
        public Guid Super { get; set; }

        [ConcurrencyCheck]
        public int Quantity { get; set; }

        public EntityTrackModel2 SuperLink { get; set; }

        public void OnCompleting(ApplicationDbContext context, EntityState state, IEnumerable<PropertyEntry> entries)
        {
            context.EntityMonitorModels.Add(new EntityMonitorModel
            {
                CreationTime = DateTime.Now,
                TypeName = typeof(EntityTrackModel3).Name,
                Key = new[] { Id.ToString() },
                Event = state switch
                {
                    EntityState.Added => nameof(EntityState.Added),
                    EntityState.Modified => nameof(EntityState.Modified),
                    EntityState.Deleted => nameof(EntityState.Deleted),
                    _ => "",
                },
                ChangeValues = new RowChangeInfo(entries),
            });
        }

        public void OnDeleting(ApplicationDbContext context, IEnumerable<PropertyEntry> entries)
        {
            var super = context.EntityTrackModel2s
                .Include(x => x.SuperLink)
                .First(x => x.Id == Super);

            super.GroupQuantity -= Quantity;
            super.SuperLink.TotalQuantity -= Quantity;
        }

        public void OnInserting(ApplicationDbContext context, IEnumerable<PropertyEntry> entries)
        {
            var super = context.EntityTrackModel2s
                .Include(x => x.SuperLink)
                .First(x => x.Id == Super);

            super.GroupQuantity += Quantity;
            super.SuperLink.TotalQuantity += Quantity;
        }

        public void OnUpdating(ApplicationDbContext context, EntityTrackModel3 origin, IEnumerable<PropertyEntry> entries)
        {
            var super = context.EntityTrackModel2s
                .Include(x => x.SuperLink)
                .First(x => x.Id == Super);

            super.GroupQuantity += Quantity - origin.Quantity;
            super.SuperLink.TotalQuantity += Quantity - origin.Quantity;
        }

    }
}
