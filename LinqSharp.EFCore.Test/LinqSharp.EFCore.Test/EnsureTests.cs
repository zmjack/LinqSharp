using LinqSharp.EFCore.Data.Test;
using NStandard;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class EnsureTests
    {
        [Fact]
        public void EnsureFirstTest()
        {
            using var context = ApplicationDbContext.UseMySql();
            using var trans = context.Database.BeginTransaction();
            var create = context.AuditRoots.Ensure(new QueryCondition<AuditRoot>
            {
                [x => x.LimitQuantity] = 1,
            });

            var found = context.AuditRoots.Ensure(new QueryCondition<AuditRoot>
            {
                [x => x.LimitQuantity] = 1,
            });

            Assert.Equal(create, found);

            trans.Rollback();
        }

        [Fact]
        public void EnsureManyTest1()
        {
            using var context = ApplicationDbContext.UseMySql();
            using var trans = context.Database.BeginTransaction();
            var created1 = context.AuditRoots.Ensure(new QueryCondition<AuditRoot>
            {
                [x => x.LimitQuantity] = 1,
            });

            var created2 = context.AuditRoots.Ensure(new[]
            {
                    new QueryCondition<AuditRoot>
                    {
                        [x => x.LimitQuantity] = 1,
                    },
                    new QueryCondition<AuditRoot>
                    {
                        [x => x.LimitQuantity] = 2,
                    },
                }, options =>
                {
                    options.Predicate = x => new[] { 1, 2 }.Contains(x.LimitQuantity);
                });
            Assert.Equal(created1, created2[0]);

            var found = context.AuditRoots.Ensure(new[]
            {
                    new QueryCondition<AuditRoot>
                    {
                        [x => x.LimitQuantity] = 1,
                    },
                    new QueryCondition<AuditRoot>
                    {
                        [x => x.LimitQuantity] = 2,
                    },
                });
            Assert.Equal(created2, found);

            trans.Rollback();
        }

        [Fact]
        public void EnsureManyTest2()
        {
            using var context = ApplicationDbContext.UseMySql();
            using var trans = context.Database.BeginTransaction();
            var conditions = new int[1000].Let(i => i).Select(i => new QueryCondition<AuditRoot>
            {
                [x => x.LimitQuantity] = i,
            }).ToArray();

            var created = context.AuditRoots.Ensure(conditions);
            Assert.Equal(1000, created.Length);

            trans.Rollback();
        }

    }
}
