using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NLinq.Test
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

        public void OnCompleting(ApplicationDbContext context, EntityState state)
        {
        }

        public void OnDeleting(ApplicationDbContext context)
        {
            var super = context.EntityTrackModel2s
                .Include(x => x.SuperLink)
                .First(x => x.Id == Super);

            super.GroupQuantity -= Quantity;
            super.SuperLink.TotalQuantity -= Quantity;
        }

        public void OnInserting(ApplicationDbContext context)
        {
            var super = context.EntityTrackModel2s
                .Include(x => x.SuperLink)
                .First(x => x.Id == Super);

            super.GroupQuantity += Quantity;
            super.SuperLink.TotalQuantity += Quantity;
        }

        public void OnUpdating(ApplicationDbContext context, EntityTrackModel3 origin)
        {
            var super = context.EntityTrackModel2s
                .Include(x => x.SuperLink)
                .First(x => x.Id == Super);

            super.GroupQuantity += Quantity - origin.Quantity;
            super.SuperLink.TotalQuantity += Quantity - origin.Quantity;
        }

    }
}
