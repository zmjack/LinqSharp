using Xunit;

namespace LinqSharp.Test
{
    public class EnsureTests
    {
        [Fact]
        public void Test1()
        {
            using (var context = new ApplicationDbContext())
            using (var tx = context.Database.BeginTransaction())
            {
                var create = context.EntityTrackModel1s.EnsureFirst(context, new[]
                {
                    new EnsureCondition<EntityTrackModel1>(x => x.TotalQuantity, 1),
                });

                var found = context.EntityTrackModel1s.EnsureFirst(context, new[]
                {
                    new EnsureCondition<EntityTrackModel1>(x => x.TotalQuantity, 1),
                });

                Assert.Equal(create, found);

                tx.Rollback();
            }
        }
    }
}
