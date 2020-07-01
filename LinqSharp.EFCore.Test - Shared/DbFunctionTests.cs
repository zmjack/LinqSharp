using LinqSharp.EFCore.Data.Test;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class DbFunctionTests
    {
        [Fact]
        public void RandomTest()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query = mysql.SimpleModels.Random(2);
                var sql = query.ToSql();
                string expectedSql;

                if (EFVersion.AtLeast(3, 0))
                {
                    expectedSql = @"SELECT `s`.`Id`, `s`.`Age`, `s`.`Birthday`, `s`.`Name`, `s`.`State`
FROM `SimpleModels` AS `s`
ORDER BY RAND()
LIMIT @__p_0;
";
                }
                else if (EFVersion.AtLeast(2, 0))
                {
                    expectedSql = @"SELECT `x`.`Id`, `x`.`Age`, `x`.`Birthday`, `x`.`Name`, `x`.`State`
FROM `SimpleModels` AS `x`
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
