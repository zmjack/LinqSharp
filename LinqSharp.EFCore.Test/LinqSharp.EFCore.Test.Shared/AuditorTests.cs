using LinqSharp.EFCore.Data.Test;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class AuditorTests
{
    [Fact]
    public void Test1()
    {
        using (var context = ApplicationDbContext.UseMySql())
        using (var trans = context.Database.BeginTransaction())
        {
            var root = new AuditRoot { LimitQuantity = 20 };
            context.AuditRoots.Add(root);
            context.SaveChanges();

            var level1 = new AuditLevel { Root = root.Id };
            var level2 = new AuditLevel { Root = root.Id };
            context.AuditLevels.AddRange(level1, level2);
            context.SaveChanges();

            var value11 = new AuditValue { Level = level1.Id, Quantity = 5 };
            var value12 = new AuditValue { Level = level1.Id, Quantity = 5 };
            var value21 = new AuditValue { Level = level2.Id, Quantity = 4 };
            var value22 = new AuditValue { Level = level2.Id, Quantity = 6 };
            context.AuditValues.AddRange(value11, value12, value21, value22);
            context.SaveChanges();

            Assert.Equal(20, root.TotalQuantity);

            value11.Quantity = 8;
            context.AuditValues.Remove(value12);
            context.AuditValues.Add(new AuditValue { Level = level1.Id, Quantity = 8 });
            context.AuditLevels.Remove(level2);
            context.AuditValues.Remove(value21);
            context.SaveChanges();

            Assert.Equal(16, root.TotalQuantity);

            trans.Rollback();
        }
    }

}
