using LinqSharp.EFCore.Data.Test;
using NStandard;
using System;
using System.Threading;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class AutoTests
{
    [Fact]
    public void Test1()
    {
        Guid id;
        using (var context = ApplicationDbContext.UseMySql())
        {
            context.CurrentUser = "User A";

            var now = DateTime.Now;
            var model = new TrackModel
            {
                ForTrim = "   127.0.0.1 ",
                ForLower = "LinqSharp",
                ForUpper = "LinqSharp",
                ForCondensed = "  Welcome to  use   LinqSharp  ",
                ForEven = 101,
            };
            context.TrackModels.Add(model);
            context.SaveChanges();

            id = model.Id;

            Assert.Equal("127.0.0.1", model.ForTrim);
            Assert.Equal("linqsharp", model.ForLower);
            Assert.Equal("LINQSHARP", model.ForUpper);
            Assert.Equal("Welcome to use LinqSharp", model.ForCondensed);
            Assert.Equal(202, model.ForEven);
            Assert.Equal(now.StartOfDay(), model.CreationTime.StartOfDay());
            Assert.Equal(now.StartOfDay(), model.LastWriteTime.StartOfDay());

            Assert.Equal("User A", model.CreatedBy);
            Assert.Equal("User A", model.UpdatedBy);
        }

        Thread.Sleep(10);

        using (var context = ApplicationDbContext.UseMySql())
        {
            context.CurrentUser = "User B";
            var model = new TrackModel
            {
                Id = id,
                ForTrim = "   127.0.0.1 ",
                ForLower = "LinqSharp",
                ForUpper = "LinqSharp",
                ForCondensed = "  Welcome to  use   LinqSharp  ",
                ForEven = 101,
            };
            context.TrackModels.Update(model);
            context.SaveChanges();
            Assert.True(model.LastWriteTime > model.CreationTime);
            Assert.Equal("User A", model.CreatedBy);
            Assert.Equal("User B", model.UpdatedBy);

            context.TrackModels.Remove(model);
            context.SaveChanges();
        }
    }

    [Fact]
    public void Test2()
    {
        using var context = ApplicationDbContext.UseMySql();

        var model = new TrackModel
        {
            ForTrim = null,
            ForLower = null,
            ForUpper = null,
            ForCondensed = null,
        };
        context.TrackModels.Add(model);
        context.SaveChanges();

        Assert.Null(model.ForTrim);
        Assert.Null(model.ForLower);
        Assert.Null(model.ForUpper);
        Assert.Equal("", model.ForCondensed);

        context.TrackModels.Remove(model);
        context.SaveChanges();
    }

}
