using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace NLinq.Test
{
    public class EntityTrackModel2 : IEntityTracker<ApplicationDbContext, EntityTrackModel2>
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(SuperLink))]
        public Guid Super { get; set; }

        [ConcurrencyCheck]
        public int GroupQuantity { get; set; }

        public EntityTrackModel1 SuperLink { get; set; }

        public virtual ICollection<EntityTrackModel3> EntityTrackModel3s { get; set; }

        public void OnCompleting(ApplicationDbContext context, EntityState state)
        {
        }

        public void OnDeleting(ApplicationDbContext context, EntityTracker tracker)
        {
            var super = context.EntityTrackModel1s.Find(Super);
            super.TotalQuantity -= GroupQuantity;
        }

        public void OnInserting(ApplicationDbContext context, EntityTracker tracker)
        {
        }

        public void OnUpdating(ApplicationDbContext context, EntityTracker tracker, EntityTrackModel2 origin)
        {
            var super = context.EntityTrackModel1s.Find(Super);
            super.TotalQuantity += GroupQuantity - origin.GroupQuantity;
        }

    }
}
