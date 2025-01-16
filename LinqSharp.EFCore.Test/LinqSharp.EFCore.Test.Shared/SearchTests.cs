using LinqSharp.EFCore.Data.Test;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class SearchTests
{
    [Fact]
    public void Test()
    {
        using var context = ApplicationDbContext.UseMySql();
        var query =
            from e in context.Employees.Search(SearchMode.Contains, "London", e => new()
            {
                e.City,

                from x in e.Orders.SelectMany(o => o.OrderDetails)
                select new
                {
                    x.ProductLink.ProductName,
                    x.ProductLink.SupplierLink.CompanyName,
                }
            })
            select new
            {
                e.FirstName
            };
        var sql = query.ToQueryString();
    }

    [Fact]
    public void SearchIntTest()
    {
        using var context = ApplicationDbContext.UseMySql();
        var query = context.Employees
            .Search(SearchMode.NotEquals, "London", e => new()
            {
                e.City,
                e.Orders
                    .SelectMany(o => o.OrderDetails)
                    .Select(x => x.ProductLink.ProductID),
            })
            .Select(x => x.FirstName);
        var sql = query.ToQueryString();
    }

    [Fact]
    public void SearchStringTest()
    {
        using var context = ApplicationDbContext.UseMySql();

        var sql = (
            from c in context.Categories.Search(SearchMode.Equals,
            [
                "Tofu",
                "England",
            ], c => new()
            {
                from p in c.Products select new
                {
                    p.ProductID,
                    p.ProductName,
                    p.SupplierLink.CompanyName,
                }
            })
            select new
            {
                c.CategoryName,
            }
        ).ToQueryString();
    }

    public class SimpleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SimpleModel[] Subs { get; set; }
    }

    [Fact]
    public void ContainsTest()
    {
        using var mysql = ApplicationDbContext.UseMySql();
        var query =
            from e in mysql.Employees.Search(SearchMode.Contains, "London", e => new()
            {
                from x in e.Orders.SelectMany(o => o.OrderDetails)
                select x.ProductLink.ProductName,

                from o in e.Orders select o.ShipCountry,
                from o in e.Orders select o.ShipRegion,
                from o in e.Orders select o.ShipCity,
                from o in e.Orders select o.ShipAddress,
            })
            select e.EmployeeID;
        var sql = query.ToQueryString();
    }

    [Fact]
    public void ContainsTest_Opt()
    {
        using var mysql = ApplicationDbContext.UseMySql();
        var query =
            from e in mysql.Employees.Search(SearchMode.Contains, "London", e => new()
            {
                from x in e.Orders.SelectMany(o => o.OrderDetails)
                select x.ProductLink.ProductName,

                from x in e.Orders
                select new
                {
                    x.ShipCountry,
                    x.ShipRegion,
                    x.ShipCity,
                    x.ShipAddress,
                }
            })
            select e.EmployeeID;
        var sql = query.ToQueryString();
    }

    [Fact]
    public void NotContainsTest()
    {
        using var mysql = ApplicationDbContext.UseMySql();
        var query =
            from e in mysql.Employees.Search(SearchMode.NotContains, "London", e => new()
            {
                from x in e.Orders.SelectMany(o => o.OrderDetails)
                select x.ProductLink.ProductName,

                from o in e.Orders select o.ShipCountry,
                from o in e.Orders select o.ShipRegion,
                from o in e.Orders select o.ShipCity,
                from o in e.Orders select o.ShipAddress,
            })
            select e.EmployeeID;
        var sql = query.ToQueryString();
    }

    [Fact]
    public void NotContainsTest_Opt()
    {
        using var mysql = ApplicationDbContext.UseMySql();
        var query =
            from e in mysql.Employees.Search(SearchMode.NotContains, "London", e => new()
            {
                from x in e.Orders.SelectMany(o => o.OrderDetails)
                select x.ProductLink.ProductName,

                from x in e.Orders
                select new
                {
                    x.ShipCountry,
                    x.ShipRegion,
                    x.ShipCity,
                    x.ShipAddress,
                }
            })
            select e.EmployeeID;
        var sql = query.ToQueryString();
    }
}
