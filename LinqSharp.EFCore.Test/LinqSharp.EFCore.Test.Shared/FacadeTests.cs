using LinqSharp.EFCore.Data.Test;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class FacadeTests
{
    private static readonly Random _random = new Random();

    [Fact]
    public void Test()
    {
        var name = _random.Next().ToString();
        var nameV2 = $"v2:{name}";

        using (var context = ApplicationDbContext.UseMySql())
        using (var trans = context.Database.BeginTransaction())
        {
            Assert.Null(context.LastChanged);

            var item = new FacadeModel { Name = name };
            context.FacadeModels.Add(item);
            context.SaveChanges();

            trans.Commit();
            Assert.Equal($"Added or Updated: {item.Id} with {name}", context.LastChanged);
        }

        using (var context = ApplicationDbContext.UseMySql())
        using (var trans = context.Database.BeginTransaction())
        {
            Assert.Null(context.LastChanged);

            var result = context.FacadeModels.First(x => x.Name == name);
            result.Name = nameV2;
            context.SaveChanges();

            trans.Commit();
            Assert.Equal($"Added or Updated: {result.Id} with {nameV2}", context.LastChanged);
        }

        using (var context = ApplicationDbContext.UseMySql())
        {
            Assert.Null(context.LastChanged);

            var result = context.FacadeModels.First(x => x.Name == nameV2);
            context.Remove(result);
            context.SaveChanges();

            Assert.Equal($"Deleted: {result.Id} with {nameV2}", context.LastChanged);
        }

        using (var context = ApplicationDbContext.UseMySql())
        using (var trans = context.Database.BeginTransaction())
        {
            Assert.Null(context.LastChanged);

            var item = new FacadeModel { Name = name };
            context.FacadeModels.Add(item);
            context.SaveChanges();

            trans.Rollback();
            Assert.Equal("Rollbacked", context.LastChanged);
        }
    }

#if EFCORE3_1_OR_GREATER
    [Fact]
    public async void AsyncTest()
    {
        var name = _random.Next().ToString();
        var nameV2 = $"v2:{name}";

        using (var context = ApplicationDbContext.UseMySql())
        using (var trans = context.Database.BeginTransaction())
        {
            Assert.Null(context.LastChanged);

            var item = new FacadeModel { Name = name };
            context.FacadeModels.Add(item);
            await context.SaveChangesAsync();

            await trans.CommitAsync();
            Assert.Equal($"Added or Updated: {item.Id} with {name}", context.LastChanged);
        }

        using (var context = ApplicationDbContext.UseMySql())
        using (var trans = context.Database.BeginTransaction())
        {
            Assert.Null(context.LastChanged);

            var result = context.FacadeModels.First(x => x.Name == name);
            result.Name = nameV2;
            await context.SaveChangesAsync();

            await trans.CommitAsync();
            Assert.Equal($"Added or Updated: {result.Id} with {nameV2}", context.LastChanged);
        }

        using (var context = ApplicationDbContext.UseMySql())
        {
            Assert.Null(context.LastChanged);

            var result = context.FacadeModels.First(x => x.Name == nameV2);
            context.Remove(result);
            await context.SaveChangesAsync();

            Assert.Equal($"Deleted: {result.Id} with {nameV2}", context.LastChanged);
        }

        using (var context = ApplicationDbContext.UseMySql())
        using (var trans = context.Database.BeginTransaction())
        {
            Assert.Null(context.LastChanged);

            var item = new FacadeModel { Name = name };
            context.FacadeModels.Add(item);
            await context.SaveChangesAsync();

            await trans.RollbackAsync();
            Assert.Equal("Rollbacked", context.LastChanged);
        }
    }
#endif

}
