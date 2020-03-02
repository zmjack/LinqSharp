using LinqSharp.Data.Test;
using Xunit;

namespace LinqSharp.Test
{
    public class EnsureTests
    {
        [Fact]
        public void Test1()
        {
            using (var context = ApplicationDbContext.UseDefault())
            using (var trans = context.Database.BeginTransaction())
            {
                var create = context.EntityTrackModel1s.EnsureFirst(new EnsureCondition<EntityTrackModel1>
                {
                    [x => x.TotalQuantity] = 1,
                });

                var found = context.EntityTrackModel1s.EnsureFirst(new EnsureCondition<EntityTrackModel1>
                {
                    [x => x.TotalQuantity] = 1,
                });

                Assert.Equal(create, found);

                trans.Rollback();
            }
        }

        [Fact]
        public void Test2()
        {
            using (var context = ApplicationDbContext.UseDefault())
            using (var trans = context.Database.BeginTransaction())
            {
                var create1 = context.EntityTrackModel1s.EnsureFirst(new EnsureCondition<EntityTrackModel1>
                {
                    [x => x.TotalQuantity] = 1,
                });

                var create2 = context.EntityTrackModel1s.EnsureMany(new[]
                {
                    new EnsureCondition<EntityTrackModel1>
                    {
                        [x => x.TotalQuantity] = 1,
                    },
                    new EnsureCondition<EntityTrackModel1>
                    {
                        [x => x.TotalQuantity] = 2,
                    },
                });
                Assert.Equal(create1, create2[0]);

                var found = context.EntityTrackModel1s.EnsureMany(new[]
                {
                    new EnsureCondition<EntityTrackModel1>
                    {
                        [x => x.TotalQuantity] = 1,
                    },
                    new EnsureCondition<EntityTrackModel1>
                    {
                        [x => x.TotalQuantity] = 2,
                    },
                });
                Assert.Equal(create2, found);

                trans.Rollback();
            }
        }

    }
}
