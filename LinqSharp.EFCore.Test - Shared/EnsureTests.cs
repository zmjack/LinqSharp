using LinqSharp.Data.Test;
using NStandard;
using System.Linq;
using Xunit;

namespace LinqSharp.Test
{
    public class EnsureTests
    {
        [Fact]
        public void EnsureFirstTest()
        {
            using (var context = ApplicationDbContext.UseMySql())
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
        public void EnsureManyTest1()
        {
            using (var context = ApplicationDbContext.UseMySql())
            using (var trans = context.Database.BeginTransaction())
            {
                var created1 = context.EntityTrackModel1s.EnsureFirst(new EnsureCondition<EntityTrackModel1>
                {
                    [x => x.TotalQuantity] = 1,
                });

                var created2 = context.EntityTrackModel1s.EnsureMany(new[]
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
                Assert.Equal(created1, created2[0]);

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
                Assert.Equal(created2, found);

                trans.Rollback();
            }
        }

        [Fact]
        public void EnsureManyTest2()
        {
            using (var context = ApplicationDbContext.UseMySql())
            using (var trans = context.Database.BeginTransaction())
            {
                var conditions = new int[1000].Let(i => i).Select(i => new EnsureCondition<EntityTrackModel1>
                {
                    [x => x.TotalQuantity] = i,
                }).ToArray();

                var created = context.EntityTrackModel1s.EnsureMany(conditions);
                Assert.Equal(1000, created.Length);

                trans.Rollback();
            }
        }

    }
}
