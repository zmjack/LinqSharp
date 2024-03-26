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
            var model = new AutoModel
            {
                Trim = "   127.0.0.1 ",
                Lower = "LinqSharp",
                Upper = "LinqSharp",
                Condensed = "  Welcome to  use   LinqSharp  ",
                Even = 101,
                Month_DateTime = now,
                Month_DateTimeOffset = now,
                Month_NullableDateTime = now,
                Month_NullableDateTimeOffset = now,
            };
            context.AutoModels.Add(model);
            context.SaveChanges();

            id = model.Id;

            Assert.Equal("127.0.0.1", model.Trim);
            Assert.Equal("linqsharp", model.Lower);
            Assert.Equal("LINQSHARP", model.Upper);
            Assert.Equal("Welcome to use LinqSharp", model.Condensed);
            Assert.Equal(202, model.Even);
            Assert.Equal(now.StartOfDay(), model.CreationTime.StartOfDay());
            Assert.Equal(now.StartOfDay(), model.LastWriteTime.StartOfDay());

            Assert.Equal(now.StartOfMonth(), model.Month_DateTime.StartOfMonth());
            Assert.Equal(now.StartOfMonth(), model.Month_DateTimeOffset.StartOfMonth());
            Assert.Equal(now.StartOfMonth(), model.Month_NullableDateTime?.StartOfMonth());
            Assert.Equal(now.StartOfMonth(), model.Month_NullableDateTimeOffset?.StartOfMonth());

            Assert.Equal("User A", model.CreatedBy);
            Assert.Equal("User A", model.UpdatedBy);
        }

        Thread.Sleep(10);

        using (var context = ApplicationDbContext.UseMySql())
        {
            context.CurrentUser = "User B";
            var model = new AutoModel
            {
                Id = id,
                Trim = "   127.0.0.1 ",
                Lower = "LinqSharp",
                Upper = "LinqSharp",
                Condensed = "  Welcome to  use   LinqSharp  ",
                Even = 101,
            };
            context.AutoModels.Update(model);
            context.SaveChanges();
            Assert.True(model.LastWriteTime > model.CreationTime);
            Assert.Equal("User A", model.CreatedBy);
            Assert.Equal("User B", model.UpdatedBy);

            context.AutoModels.Remove(model);
            context.SaveChanges();
        }
    }

    [Fact]
    public void Test2()
    {
        using var context = ApplicationDbContext.UseMySql();

        var model = new AutoModel
        {
            Trim = null,
            Lower = null,
            Upper = null,
            Condensed = null,
        };
        context.AutoModels.Add(model);
        context.SaveChanges();

        Assert.Null(model.Trim);
        Assert.Null(model.Lower);
        Assert.Null(model.Upper);
        Assert.Equal("", model.Condensed);

        context.AutoModels.Remove(model);
        context.SaveChanges();
    }

}
