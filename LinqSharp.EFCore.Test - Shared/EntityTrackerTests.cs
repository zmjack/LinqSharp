using LinqSharp.Data.Test;
using System.Linq;
using Xunit;

namespace LinqSharp.Test
{
    public class EntityTrackerTests
    {
        [Fact]
        public void Test1()
        {
            using (var context = ApplicationDbContext.UseMySql())
            {
                var model1 = new EntityTrackModel1();
                context.EntityTrackModel1s.Add(model1);
                context.SaveChanges();

                var model2 = new EntityTrackModel2 { Super = model1.Id };
                context.EntityTrackModel2s.Add(model2);
                context.SaveChanges();

                var model3s = new[]
                {
                    new EntityTrackModel3 { Super = model2.Id, Quantity = 10 },
                    new EntityTrackModel3 { Super = model2.Id, Quantity = 20 },
                };
                context.EntityTrackModel3s.AddRange(model3s);
                context.SaveChanges();

                Assert.Equal(30, model1.TotalQuantity);
                Assert.Equal(30, model2.GroupQuantity);

                var m3 = context.EntityTrackModel3s.Where(x => x.Quantity == 10).First();
                m3.Quantity = 50;
                context.SaveChanges();

                Assert.Equal(70, model1.TotalQuantity);
                Assert.Equal(70, model2.GroupQuantity);

                context.EntityTrackModel3s.Remove(m3);
                context.SaveChanges();

                Assert.Equal(20, model1.TotalQuantity);
                Assert.Equal(20, model2.GroupQuantity);

                context.Remove(model1);
                context.SaveChanges();
            }
        }

    }
}
