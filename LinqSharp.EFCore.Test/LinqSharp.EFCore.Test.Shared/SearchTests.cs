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
    public void Test1()
    {
        var models = new[]
        {
            new SimpleModel { Id = 1, Subs = [] },
            new SimpleModel
            {
                Id = 2,
                Subs =
                [
                    //new SimpleModel { Id = 3 },
                    new SimpleModel { Id = 3, Name = "a" },
                ]
            },
        };

        var aa = models.Search(SearchMode.Contains, "a", m => new()
        {
            m.Name,
            m.Subs.Select(x => x.Name),
        }).ToArray();
        Assert.Single(aa);
    }

    [Fact]
    public void QueryableTest1()
    {
        using (var mysql = ApplicationDbContext.UseMySql())
        {
            var query = mysql.Employees
                .Search(SearchMode.Contains, "London", e => new()
                {
                    e.City,
                    e.Orders
                        .SelectMany(o => o.OrderDetails)
                        .Select(x => x.ProductLink.ProductName),
                });
            var sql = query.ToQueryString();
        }

        using (var mysql = ApplicationDbContext.UseMySql())
        {
            var query = mysql.Employees
                .Search(SearchMode.Contains, "London", e => new()
                {
                    e.Orders
                        .SelectMany(o => o.OrderDetails)
                        .Select(x => x.ProductLink.ProductName),
                    e.Orders.Select(x => x.ShipCountry),
                    e.Orders.Select(x => x.ShipRegion),
                    e.Orders.Select(x => x.ShipCity),
                    e.Orders.Select(x => x.ShipAddress),
                });
            var sql = query.ToQueryString();
        }

        using (var mysql = ApplicationDbContext.UseMySql())
        {
            var employees_WhoSelled_AllKindsOfTofu = mysql.Employees
                .Search(SearchMode.Contains, "Tofu", e => new()
                {
                    e.Orders
                        .SelectMany(o => o.OrderDetails)
                        .Select(x => x.ProductLink.ProductName)
                });
            var sql1 = employees_WhoSelled_AllKindsOfTofu.ToQueryString();

            var employees_WhoSelled_Tofu = mysql.Employees
                 .Search(SearchMode.Equals, "Tofu", e => new()
                 {
                     e.Orders
                         .SelectMany(o => o.OrderDetails)
                         .Select(x => x.ProductLink.ProductName)
                 });
            var sql2 = employees_WhoSelled_Tofu.ToQueryString();

            var employees_WhoSelled_LongLifeTofu = mysql.Employees
                 .Search(SearchMode.Equals, "Longlife Tofu", e => new()
                 {
                     e.Orders
                         .SelectMany(o => o.OrderDetails)
                         .Select(x => x.ProductLink.ProductName)
                 });
            var sql3 = employees_WhoSelled_LongLifeTofu.ToQueryString();
        }
        return;
    }
}
