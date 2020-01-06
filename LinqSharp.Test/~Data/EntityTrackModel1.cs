using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinqSharp.Test
{
    public class EntityTrackModel1 : IEntityTracker<ApplicationDbContext, EntityTrackModel1>
    {
        [Key]
        public Guid Id { get; set; }

        [ConcurrencyCheck]
        public int TotalQuantity { get; set; }

        public virtual ICollection<EntityTrackModel2> EntityTrackModel2s { get; set; }

        public void OnCompleting(ApplicationDbContext context, EntityState state)
        {
        }

        public void OnDeleting(ApplicationDbContext context)
        {
        }

        public void OnInserting(ApplicationDbContext context)
        {
        }

        public void OnUpdating(ApplicationDbContext context, EntityTrackModel1 origin)
        {
        }

    }
}
