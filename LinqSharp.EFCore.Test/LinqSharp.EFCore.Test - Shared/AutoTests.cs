using LinqSharp.EFCore.Data.Test;
using System;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class AutoTests
    {
        [Fact]
        public void Test1()
        {
            using var context = ApplicationDbContext.UseMySql();

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

        [Fact]
        public void Test3()
        {
            using (var context = ApplicationDbContext.UseMySql())
            {
                var model = new SimpleModel
                {
                    Name = "zmjack",
                    Age = 18,
                };
                context.Add(model);
                context.SaveChanges();
            }

            Guid id;
            using (var context = ApplicationDbContext.UseMySql())
            {
                var result = context.SimpleModels.First();
                id = result.Id;
                Assert.Equal(18, result.Age);
            }

            using (var context = ApplicationDbContext.UseMySql())
            {
                var item = new SimpleModel
                {
                    Id = id,
                    Name = "zmjack",
                    Age = 27,
                };
                context.SimpleModels.Update(item);
                context.SaveChanges();
            }

            using (var context = ApplicationDbContext.UseMySql())
            {
                var result = context.SimpleModels.First();
                Assert.Equal(27, result.Age);
            }


            using (var context = ApplicationDbContext.UseMySql())
            {
                var result = context.SimpleModels;
                context.RemoveRange(result);
                context.SaveChanges();
            }

        }

    }
}
