using LinqSharp.Design;
using LinqSharp.EFCore.Data.Test;
using Microsoft.EntityFrameworkCore;
using Northwnd.Data;
using NStandard;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class SorterTests
{
    public class QuerySorter(bool descending = false) : IQuerySorter<Employee>
    {
        public QuerySortRule<Employee>? Sort()
        {
            return new(x => x.Address, descending);
        }
    }

    public class CoQuerySorter(bool descending = false) : ICoQuerySorter<Employee>
    {
        public IEnumerable<QuerySortRule<Employee>> Sort()
        {
            yield return new(x => x.Address, descending);
            yield return new(x => x.FirstName, descending);
            yield return new(x => x.LastName, descending);
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

    [Fact]
    public void QuerySorterDescendingTest()
    {
        using var mysql = ApplicationDbContext.UseMySql();
        var sql = mysql.Employees.Sort(new QuerySorter(true)).Select(x => x.Address).ToQueryString();
        Assert.Equal(
"""
SELECT `@`.`Address`
FROM `@n.Employees` AS `@`
ORDER BY `@`.`Address` DESC
"""
        , sql);
    }

    [Fact]
    public void CoQuerySorterDescendingTest()
    {
        using var mysql = ApplicationDbContext.UseMySql();
        var sql = mysql.Employees.Sort(new CoQuerySorter(true)).Select(x => x.Address).ToQueryString();
        Assert.Equal(
"""
SELECT `@`.`Address`
FROM `@n.Employees` AS `@`
ORDER BY `@`.`Address` DESC, `@`.`FirstName` DESC, `@`.`LastName` DESC
"""
        , sql);
    }
}
