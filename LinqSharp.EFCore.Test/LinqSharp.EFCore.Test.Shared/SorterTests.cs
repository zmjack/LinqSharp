using LinqSharp.Design;
using LinqSharp.EFCore.Data.Test;
using Microsoft.EntityFrameworkCore;
using Northwnd.Data;
using NStandard;
using System.Linq.Expressions;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class SorterTests
{
    public class QuerySorter : IQuerySorter<Employee>
    {
        public Expression<Func<Employee, object>> Sort()
        {
            return x => x.Address;
        }
    }

    public class CoQuerySorter : ICoQuerySorter<Employee>
    {
        public IEnumerable<Expression<Func<Employee, object>>> Sort()
        {
            yield return x => x.Address;
            yield return x => x.FirstName;
            yield return x => x.LastName;
        }
    }

    [Fact]
    public void QuerySorterTest()
    {
        using var mysql = ApplicationDbContext.UseMySql();
        var sql = mysql.Employees.Sort(new QuerySorter()).Select(x => x.Address).ToQueryString();
        Assert.Equal(
"""
SELECT `@`.`Address`
FROM `@n.Employees` AS `@`
ORDER BY `@`.`Address`
"""
        , sql);
    }

    [Fact]
    public void CoQuerySorterTest()
    {
        using var mysql = ApplicationDbContext.UseMySql();
        var sql = mysql.Employees.Sort(new CoQuerySorter()).Select(x => x.Address).ToQueryString();
        Assert.Equal(
"""
SELECT `@`.`Address`
FROM `@n.Employees` AS `@`
ORDER BY `@`.`Address`, `@`.`FirstName`, `@`.`LastName`
"""
        , sql);
    }
}
