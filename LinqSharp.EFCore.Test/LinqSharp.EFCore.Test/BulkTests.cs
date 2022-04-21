using LinqSharp.EFCore.Data.Test;
using LinqSharp.EFCore.MySql;
using LinqSharp.EFCore.SqlServer;
using NStandard;
using NStandard.Flows;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class BulkTests
    {
        static BulkTests()
        {
            LinqSharpEFRegister.RegisterBulkCopyEngine(DatabaseProviderName.MySql, new MySqlBulkCopyEngine());
            LinqSharpEFRegister.RegisterBulkCopyEngine(DatabaseProviderName.SqlServer, new SqlServerBulkCopyEngine());
        }

        [Fact]
        public void BulkInsertTest()
        {
            using var context = ApplicationDbContext.UseSqlServer();
            var count = 1000;

            var models = new BulkTestModel[count].Let(i => new BulkTestModel
            {
                Id = Guid.NewGuid(),
                Code = Path.GetRandomFileName(),
                Name = Path.GetRandomFileName().Bytes().For(BytesFlow.Base58),
            });

            using (context.BeginDirectScope())
            {
                context.BulkTestModels.Truncate();      // Clear table
                context.BulkTestModels.BulkInsert(models);
                Assert.Equal(count, context.BulkTestModels.Count());
            }

        }

    }
}
