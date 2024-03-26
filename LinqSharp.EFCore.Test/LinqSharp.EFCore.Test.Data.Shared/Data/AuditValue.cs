using LinqSharp.EFCore.Design;
using Microsoft.EntityFrameworkCore;
using NStandard.Caching;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LinqSharp.EFCore.Data.Test;

[EntityAudit(typeof(AuditValueAuditor))]
public class AuditValue
{
    [Key]
    public Guid Id { get; set; }

    [ForeignKey(nameof(LevelLink))]
    public Guid Level { get; set; }

    [ConcurrencyCheck]
    public int Quantity { get; set; }

    public AuditLevel LevelLink { get; set; }
}

public class AuditValueAuditor : IEntityAuditor<ApplicationDbContext, AuditValue>
{
    public void BeforeAudit(ApplicationDbContext context, EntityAudit<AuditValue>[] audits)
    {
    }

    public void OnAuditing(ApplicationDbContext context, EntityAudit<AuditValue>[] audits)
    {
        var levelCaches = new CacheSet<Guid, AuditLevel>(id => () => context.AuditLevels.Include(x => x.RootLink).First(x => x.Id == id));

        foreach (var audit in audits)
        {
            var level = levelCaches[audit.Current.Level].Value;
            switch (audit.State)
            {
                case EntityState.Added:
                    level.ValueCount += 1;
                    level.RootLink.TotalQuantity += audit.Current.Quantity;
                    break;

                case EntityState.Modified:
                    level.RootLink.TotalQuantity += audit.Current.Quantity - audit.Origin.Quantity;
                    break;

                case EntityState.Deleted:
                    level.ValueCount -= 1;
                    level.RootLink.TotalQuantity -= audit.Current.Quantity;
                    break;
            }
        }
    }

    public void OnAudited(ApplicationDbContext context, AuditPredictor predictor)
    {
    }
}
