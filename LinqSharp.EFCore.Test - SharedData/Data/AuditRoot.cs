using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LinqSharp.EFCore.Data.Test
{
    [EntityAudit(typeof(AuditRootAudit))]
    public class AuditRoot
    {
        [Key]
        public Guid Id { get; set; }

        [ConcurrencyCheck]
        public int TotalQuantity { get; set; }

        public int LimitQuantity { get; set; }

        public virtual ICollection<AuditLevel> Levels { get; set; }
    }

    public class AuditRootAudit : IEntityAuditor<ApplicationDbContext, AuditRoot>
    {
        public void BeforeAudit(ApplicationDbContext context, EntityAudit<AuditRoot>[] audits)
        {
        }

        public void OnAudited(ApplicationDbContext context, AuditPredictor predictor)
        {
            var roots = predictor.Pick<AuditRoot>().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var root in roots)
            {
                if (root.Current.TotalQuantity > root.Current.LimitQuantity)
                    throw new InvalidOperationException("Invalid TotalQuantity.");
            }
        }

        public void OnAuditing(ApplicationDbContext context, EntityAudit<AuditRoot>[] audits)
        {
        }
    }

}
