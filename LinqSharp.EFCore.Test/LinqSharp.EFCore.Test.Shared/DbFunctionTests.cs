using LinqSharp.EFCore.Data.Test;
using LinqSharp.EFCore.Translators;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class DbFunctionTests
{
    [Fact]
    public void RandomTest()
    {
        using var mysql = ApplicationDbContext.UseMySql();
        var query = mysql.SimpleModels.Random(2);
        var sql = query.ToQueryString();
        string expectedSql;

#if EFCORE5_0_OR_GREATER
        expectedSql = @"
SELECT `s`.`Id`, `s`.`Age`, `s`.`Birthday`, `s`.`Name`, `s`.`State`
FROM `SimpleModels` AS `s`
ORDER BY RAND()
LIMIT 2";

#elif EFCORE3_1_OR_GREATER
        expectedSql = @"SELECT `s`.`Id`, `s`.`Age`, `s`.`Birthday`, `s`.`Name`, `s`.`State`
FROM `SimpleModels` AS `s`
ORDER BY RAND()
LIMIT @__p_0;
";

#elif EFCORE2_1_OR_GREATER
        expectedSql = @"SELECT `x`.`Id`, `x`.`Age`, `x`.`Birthday`, `x`.`Name`, `x`.`State`
FROM `SimpleModels` AS `x`
ORDER BY RAND()
LIMIT 2;
";
#endif
        Assert.Equal(expectedSql, sql);
    }

    [Fact]
    public void RowNumberTest()
    {
        using var mysql = ApplicationDbContext.UseMySql();
        var query =
        (
            from x in mysql.Orders
            let rn = DbRowNumber.RowNumber(
                x.EmployeeID,  // partition by
                x.OrderID,  // order by
                true        // desc
            )
            select new
            {
                rn,
                x.OrderID,
                x.EmployeeID,
            }
        );
        var sql = query.ToQueryString();

        var results = query.ToArray();

        string expectedSql;
    }

}
