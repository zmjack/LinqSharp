using LinqSharp.EFCore.Data.Test;
using NStandard;
using NStandard.Diagnostics;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class ZipperTests
{
    [Fact]
    public void ConcurrencyTest()
    {
        using var context = ApplicationDbContext.UseMySql();
        using (context.BeginUnsafeQuery())
        {
            context.ZipperModels.Truncate();
        }

        Concurrency.Run(4, 4, id =>
        {
            InnerTest();
        });
        DeleteTest();
    }

    private void InnerTest()
    {
        using var context = ApplicationDbContext.UseMySql();

        using (var zipper = context.BeginZipperScope(x => x.ZipperModels, x => x.KeyName, x => x))
        {
            var a = zipper.GetAgent("A");
            var b = zipper.GetAgent("B");
            var c = zipper.GetAgent("C");
            var d = zipper.GetAgent("D");

            a.AddOrUpdate(new(2000, 1, 1), new() { Price = 10 });
            b.AddOrUpdate(new(2000, 1, 1), new() { Price = 20 });
            c.AddOrUpdate(new(2000, 1, 1), new() { Price = 30 });
            b.AddOrUpdate(new(2000, 3, 1), new() { Price = null });
            d.AddOrUpdate(new(2000, 3, 1), new() { Price = 40 });
            c.AddOrUpdate(new(2000, 3, 1), new() { Price = 35 });
            c.AddOrUpdate(new(2000, 5, 1), new() { Price = 50 });
            c.AddOrUpdate(new(2000, 2, 1), new() { Price = 20 });
            context.SaveChanges();
        }

        using (var zipper = context.BeginZipperScope(x => x.ZipperModels, x => x.KeyName, x => x))
        {
            var dict = new Dictionary<DateOnly, (string, decimal?)[]>()
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
                    ("C", 20),
                ],
                [new(2000, 3, 1)] =
                [
                    ("A", 10),
                    ("C", 35),
                    ("D", 40),
                ],
                [new(2000, 4, 1)] =
                [
                    ("A", 10),
                    ("C", 35),
                    ("D", 40),
                ],
                [new(2000, 5, 1)] =
                [
                    ("A", 10),
                    ("C", 50),
                    ("D", 40),
                ],
            };

            foreach (var pair in dict)
            {
                var date = pair.Key;
                var source = zipper.View(date);
                var actual = (
                    from x in source
                    where x.Price is not null
                    orderby x.KeyName
                    select (x.KeyName, x.Price)
                ).ToArray();
                Assert.Equal(pair.Value, actual);
            }
        }
    }

    private void DeleteTest()
    {
        using var context = ApplicationDbContext.UseMySql();

        using (var zipper = context.BeginZipperScope(x => x.ZipperModels, x => x.KeyName, x => x))
        {
            var a = zipper.GetAgent("A");
            Assert.ThrowsAny<Exception>(() => a.Remove(new(2000, 1, 2)));
            a.Remove(new(2000, 1, 1));
            context.SaveChanges();
        }

        using (var zipper = context.BeginZipperScope(x => x.ZipperModels, x => x.KeyName, x => x))
        {
            var dict = new Dictionary<DateOnly, (string, decimal?)[]>()
            {
                [new(2000, 1, 1)] =
                [
                    ("B", 20),
                    ("C", 30),
                ],
                [new(2000, 2, 1)] =
                [
                    ("B", 20),
                    ("C", 20),
                ],
                [new(2000, 3, 1)] =
                [
                    ("C", 35),
                    ("D", 40),
                ],
                [new(2000, 4, 1)] =
                [
                    ("C", 35),
                    ("D", 40),
                ],
                [new(2000, 5, 1)] =
                [
                    ("C", 50),
                    ("D", 40),
                ],
            };

            foreach (var pair in dict)
            {
                var date = pair.Key;
                var source = zipper.View(date);
                var actual = (
                    from x in source
                    where x.Price is not null
                    orderby x.KeyName
                    select (x.KeyName, x.Price)
                ).ToArray();
                Assert.Equal(pair.Value, actual);
            }
            context.SaveChanges();
        }
    }
}
