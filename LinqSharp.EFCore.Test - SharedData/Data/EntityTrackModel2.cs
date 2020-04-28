using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinqSharp.EFCore.Data.Test
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

        public void OnCompleting(ApplicationDbContext context, EntityState state, IEnumerable<PropertyEntry> entries)
        {
        }

        public void OnDeleting(ApplicationDbContext context, IEnumerable<PropertyEntry> entries)
        {
            var super = context.EntityTrackModel1s.Find(Super);
            super.TotalQuantity -= GroupQuantity;
        }

        public void OnInserting(ApplicationDbContext context, IEnumerable<PropertyEntry> entries)
        {
        }

        public void OnUpdating(ApplicationDbContext context, EntityTrackModel2 origin, IEnumerable<PropertyEntry> entries)
        {
            var super = context.EntityTrackModel1s.Find(Super);
            super.TotalQuantity += GroupQuantity - origin.GroupQuantity;
        }

    }
}
