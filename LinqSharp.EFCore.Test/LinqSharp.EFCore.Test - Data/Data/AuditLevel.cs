using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LinqSharp.EFCore.Data.Test
{
    [EntityAudit(typeof(AuditLevelAudit))]
    public class AuditLevel
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(RootLink))]
        public Guid Root { get; set; }

        [ConcurrencyCheck]
        public int ValueCount { get; set; }

        public AuditRoot RootLink { get; set; }

        public virtual ICollection<AuditValue> Values { get; set; }
    }

    public class AuditLevelAudit : IEntityAuditor<ApplicationDbContext, AuditLevel>
    {
        public void BeforeAudit(ApplicationDbContext context, EntityAudit<AuditLevel>[] audits)
        {
            foreach (var audit in audits)
            {
                if (audit.State == EntityState.Deleted)
                {
                    var values = context.AuditValues.Where(x => x.Level == audit.Current.Id);
                    context.AuditValues.RemoveRange(values);
                }
            }
        }

        public void OnAudited(ApplicationDbContext context, AuditPredictor predictor)
        {
        }

        public void OnAuditing(ApplicationDbContext context, EntityAudit<AuditLevel>[] audits)
        {
        }
    }

}
