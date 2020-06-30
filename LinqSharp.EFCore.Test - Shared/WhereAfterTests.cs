using LinqSharp.EFCore.Data.Test;
using LinqSharp.EFCore.ProviderFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class WhereAfterTests
    {
        [Fact]
        public void Test1()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                // Worst
                {
                    var query = mysql.YearMonthModels.WhereAfter(x => x.Year, x => x.Month, x => x.Day, new DateTime(2000, 1, 1));
                    var sql = query.ToSql();
                }
                // Better
                {
                    var query = mysql.YearMonthModels.Where(x => PMySql.StrToDate(x.Year.ToString() + '-' + x.Month.ToString() + '-' + x.Day.ToString(), "%Y-%m-%d") > new DateTime(2000, 1, 1));
                    var sql = query.ToSql();
                }
            }
        }

    }
}
