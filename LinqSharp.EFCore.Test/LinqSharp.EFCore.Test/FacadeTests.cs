using LinqSharp.EFCore.Data.Test;
using System;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class FacadeTests
    {
        [Fact]
        public void Test()
        {
            var name = DateTime.Now.ToString();

            using (var context = ApplicationDbContext.UseMySql())
            using (var trans = context.Database.BeginTransaction())
            {
                Assert.Null(context.LastChanged);

                var item = new FacadeModel { Name = name };
                context.FacadeModels.Add(item);
                context.SaveChanges();

                trans.Commit();

                Assert.Equal(item.Id, context.LastChanged);
            }

            using (var context = ApplicationDbContext.UseMySql())
            {
                Assert.Null(context.LastChanged);

                var results = context.FacadeModels.Where(x => x.Name == name).ToArray();
                context.RemoveRange(results);
                context.SaveChanges();

                Assert.Null(context.LastChanged);
            }

            using (var context = ApplicationDbContext.UseMySql())
            using (var trans = context.Database.BeginTransaction())
            {
                Assert.Null(context.LastChanged);

                var item = new FacadeModel { Name = name };
                context.FacadeModels.Add(item);
                context.SaveChanges();

                trans.Rollback();

                Assert.Equal(Guid.Empty, context.LastChanged);
            }

        }

    }
}
