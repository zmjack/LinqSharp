using LinqSharp.EFCore.Data.Test;
using NStandard;
using System;
using System.Threading;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class AutoTests
    {
        [Fact]
        public void Test1()
        {
            using var context = ApplicationDbContext.UseMySql();
            var now = DateTime.Now;

            var model = new TrackModel
            {
                ForTrim = "   127.0.0.1 ",
                ForLower = "LinqSharp",
                ForUpper = "LinqSharp",
                ForCondensed = "  Welcome to  use   LinqSharp  ",
            };
            context.TrackModels.Add(model);
            context.SaveChanges();

            Assert.Equal("127.0.0.1", model.ForTrim);
            Assert.Equal("linqsharp", model.ForLower);
            Assert.Equal("LINQSHARP", model.ForUpper);
            Assert.Equal("Welcome to use LinqSharp", model.ForCondensed);
            Assert.Equal(now.StartOfDay(), model.CreationTime.StartOfDay());
            Assert.Equal(now.StartOfDay(), model.LastWriteTime.StartOfDay());

            Thread.Sleep(10);
            context.TrackModels.Update(model);
            context.SaveChanges();
            Assert.True(model.LastWriteTime > model.CreationTime);

            context.TrackModels.Remove(model);
            context.SaveChanges();
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
}
