using LinqSharp.Data.Test;
using System;
using System.Linq;
using Xunit;

namespace LinqSharp.Test
{
    public class TrackTests
    {
        [Fact]
        public void Test1()
        {
            using (var context = ApplicationDbContext.UseMySql())
            {
                var origin = new TrackModel
                {
                    ForTrim = "   127.0.0.* ",
                    ForLower = "Dawnx",
                    ForUpper = "Dawnx",
                    ForCondensed = "  Welcome to   Dawnx  ",
                };
                var model = new TrackModel().Accept(origin);
                context.TrackModels.Add(model);
                context.SaveChanges();

                Assert.Equal("127.0.0.*", model.ForTrim);
                Assert.Equal("dawnx", model.ForLower);
                Assert.Equal("DAWNX", model.ForUpper);
                Assert.Equal("Welcome to Dawnx", model.ForCondensed);

                model = context.TrackModels.First();
                model.Accept(origin, x => new { x.ForTrim, x.ForLower, x.ForUpper, x.ForCondensed });

                Assert.Equal(origin.ForTrim, model.ForTrim);
                Assert.Equal(origin.ForLower, model.ForLower);
                Assert.Equal(origin.ForUpper, model.ForUpper);
                Assert.Equal(origin.ForCondensed, model.ForCondensed);

                context.TrackModels.Remove(model);
                context.SaveChanges();
            }
        }

        [Fact]
        public void Test2()
        {
            using (var context = ApplicationDbContext.UseMySql())
            {
                var model = new SimpleModel
                {
                    NickName = "zmjack",
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
                    NickName = "zmjack",
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
