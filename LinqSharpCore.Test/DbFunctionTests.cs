using LinqSharp.Data.Test;
using Xunit;

namespace LinqSharp.Test
{
    public class DbFunctionTests
    {
        [Fact]
        public void RandomTest()
        {
            using (var mysql = ApplicationDbContext.UseDefault())
            {
                var query = mysql.FreeModels.Random(2);
                var sql = query.ToSql();
                var expectedSql = @"SELECT `x`.`Id`, `x`.`Age`, `x`.`Name`, `x`.`State`
FROM `FreeModels` AS `x`
ORDER BY RAND()
LIMIT 2;
";
                Assert.Equal(expectedSql, sql);
            }
        }

    }
}
