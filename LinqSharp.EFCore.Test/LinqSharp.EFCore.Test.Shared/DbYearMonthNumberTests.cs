using LinqSharp.EFCore.Data.Test;
using LinqSharp.EFCore.Test.DbFuncProviders;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class DbYearMonthNumberTests
{
    [Fact]
    public void Test()
    {
        using var context = ApplicationDbContext.UseMySql();
        var sql = context.YearMonthModels.Where(x => DbYearMonthNumber.Combine(x.Year, x.Month) > 201204).ToQueryString();

#if EFCORE5_0_OR_GREATER
        Assert.Equal(@"SELECT `y`.`Id`, `y`.`Date`, `y`.`Day`, `y`.`Month`, `y`.`Year`
FROM `YearMonthModels` AS `y`
WHERE ((`y`.`Year` * 100) + `y`.`Month`) > 201204", sql);

#elif EFCORE3_1_OR_GREATER
        Assert.Equal(@"SELECT `y`.`Id`, `y`.`Date`, `y`.`Day`, `y`.`Month`, `y`.`Year`
FROM `YearMonthModels` AS `y`
WHERE ((`y`.`Year` * 100) + `y`.`Month`) > 201204;
", sql);

#else
        Assert.Equal(@"SELECT `x`.`Id`, `x`.`Date`, `x`.`Day`, `x`.`Month`, `x`.`Year`
FROM `YearMonthModels` AS `x`
WHERE ((`x`.`Year` * 100) + `x`.`Month`) > 201204;
", sql);
#endif
    }
}
