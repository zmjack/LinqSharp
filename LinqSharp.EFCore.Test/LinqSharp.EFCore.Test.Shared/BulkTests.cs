using LinqSharp.EFCore.Data.Test;
using LinqSharp.EFCore.MySql;
using LinqSharp.EFCore.SqlServer;
using NStandard;
using NStandard.Flows;
using System;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class BulkTests
{
    static BulkTests()
    {
        LinqSharpEFRegister.RegisterBulkCopyEngine(ProviderName.MySql, new MySqlBulkCopyEngine());
        LinqSharpEFRegister.RegisterBulkCopyEngine(ProviderName.SqlServer, new SqlServerBulkCopyEngine());
    }

    private static void BulkInsertTest(ApplicationDbContext context)
    {
        var count = 100;
        var guid = Guid.NewGuid();

        var models = new BulkTestModel[count].Let(i =>
        {
            var guid = Guid.NewGuid();
            return new BulkTestModel
            {
                Id = guid,
                Code = $"{guid} code",
                Name = guid.ToString().Bytes().Pipe(BytesFlow.Base58),
            };
        });

        using (context.BeginDirectQuery())
        {
            context.BulkTestModels.Truncate();
            context.BulkTestModels.BulkInsert(models);
            Assert.Equal(count, context.BulkTestModels.Count());
        }
    }

    [Fact]
    public void BulkInsert_SqlServerTest()
    {
        using var context = ApplicationDbContext.UseSqlServer();
        BulkInsertTest(context);
    }

    [Fact]
    public void BulkInsert_MySqlTest()
    {
        using var context = ApplicationDbContext.UseMySql();
        BulkInsertTest(context);
    }

}
