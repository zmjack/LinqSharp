using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinqSharp.EFCore.Data.Test
{
    public class EntityTrackModel1
    {
        [Key]
        public Guid Id { get; set; }

        [ConcurrencyCheck]
        public int TotalQuantity { get; set; }

        public virtual ICollection<EntityTrackModel2> EntityTrackModel2s { get; set; }

    }
}
