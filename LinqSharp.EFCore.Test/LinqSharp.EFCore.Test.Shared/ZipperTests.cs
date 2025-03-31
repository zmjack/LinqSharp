using LinqSharp.EFCore.Data.Test;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class ZipperTests
{
    [Fact]
    public void Test()
    {
        using var context = ApplicationDbContext.UseMySql();

        using (context.BeginUnsafeQuery())
        {
            context.ZipperModels.Truncate();
        }

        using (var scope = context.BeginZipperScope(x => x.ZipperModels))
        {
            var agent = scope.GetAgent(x => x.KeyName);
            agent.AddOrUpdate("A", new(2000, 1, 1), new() { Price = 10 });
            agent.AddOrUpdate("B", new(2000, 1, 1), new() { Price = 20 });
            agent.AddOrUpdate("C", new(2000, 1, 1), new() { Price = 30 });
            agent.AddOrUpdate("B", new(2000, 3, 1), new() { Price = null });
            agent.AddOrUpdate("D", new(2000, 3, 1), new() { Price = 40 });
            agent.AddOrUpdate("C", new(2000, 3, 1), new() { Price = 35 });
        }
        context.SaveChanges();

        using (var scope = context.BeginZipperScope(x => x.ZipperModels))
        {
            var agent = scope.GetAgent(x => x.KeyName);
            var dict = new Dictionary<DateTime, (string, decimal?)[]>()
            {
                [new(2000, 1, 1)] =
                [
                    ("A", 10),
                    ("B", 20),
                    ("C", 30),
                ],
                [new(2000, 2, 1)] =
                [
                    ("A", 10),
                    ("B", 20),
                    ("C", 30),
                ],
                [new(2000, 3, 1)] =
                [
                    ("A", 10),
                    ("C", 35),
                    ("D", 40),
                ],
            };

            foreach (var pair in dict)
            {
                var source = agent.View(pair.Key);
                var actual = (
                    from x in source
                    where x.Price is not null
                    orderby x.KeyName
                    select (x.KeyName, x.Price)
                ).ToArray();
                Assert.Equal(pair.Value, actual);
            }
        }

        using (var scope = context.BeginZipperScope(x => x.ZipperModels))
        {
            var agent = scope.GetAgent(x => x.KeyName);
            Assert.ThrowsAny<Exception>(() => agent.Remove("A", new(2000, 1, 2)));
            agent.Remove("A", new(2000, 1, 1));
        }
        context.SaveChanges();

        using (var scope = context.BeginZipperScope(x => x.ZipperModels))
        {
            var agent = scope.GetAgent(x => x.KeyName);
            var dict = new Dictionary<DateTime, (string, decimal?)[]>()
            {
                [new(2000, 1, 1)] =
                [
                    ("B", 20),
                    ("C", 30),
                ],
                [new(2000, 2, 1)] =
                [
                    ("B", 20),
                    ("C", 30),
                ],
                [new(2000, 3, 1)] =
                [
                    ("C", 35),
                    ("D", 40),
                ],
            };

            foreach (var pair in dict)
            {
                var source = agent.View(pair.Key);
                var actual = (
                    from x in source
                    where x.Price is not null
                    orderby x.KeyName
                    select (x.KeyName, x.Price)
                ).ToArray();
                Assert.Equal(pair.Value, actual);
            }
        }
        context.SaveChanges();
    }
}
