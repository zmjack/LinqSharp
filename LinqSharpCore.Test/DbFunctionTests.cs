using LinqSharp.Data.Test;
using Xunit;

namespace LinqSharp.Test
{
    public class DbFunctionTests
    {
        [Fact]
        public void RandomTest()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query = mysql.FreeModels.Random(2);
                var sql = query.ToSql();
                string expectedSql;

                if (EFVersion.AtLeast(3, 0))
                {
                    expectedSql = @"SELECT `f`.`Id`, `f`.`Age`, `f`.`Name`, `f`.`State`
FROM `FreeModels` AS `f`
ORDER BY `RAND`()
LIMIT @__p_0;
";
                }
                else if (EFVersion.AtLeast(2, 0))
                {
                    expectedSql = @"SELECT `x`.`Id`, `x`.`Age`, `x`.`Name`, `x`.`State`
FROM `FreeModels` AS `x`
ORDER BY RAND()
LIMIT 2;
";
                }
                else throw EFVersion.NotSupportedException;

                Assert.Equal(expectedSql, sql);
            }
        }

    }
}
