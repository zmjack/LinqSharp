using LinqSharp.EFCore.Data.Test;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class SearchTests
    {
        private class SimpleModel
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
                new SimpleModel { Id = 1, Subs = new SimpleModel[0] },
                new SimpleModel
                {
                    Id = 2,
                    Subs = new[]
                    {
                        //new SimpleModel { Id = 3 },
                        new SimpleModel { Id = 3, Name = "a" },
                    }
                },
            };

            var aa = models.Search("a", model => new
            {
                model.Name,
                SubNames = model.Subs.Select(x => x.Name),
            });
        }

        [Fact]
        public void QueryableTest1()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query = mysql.Employees
                    .Search("London", e => new
                    {
                        e.City,
                        ProductName = e.Orders
                            .SelectMany(o => o.OrderDetails)
                            .Select(x => x.ProductLink.ProductName),
                    });
                var sql = query.ToSql();
            }

            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query = mysql.Employees
                    .Search("London", e => new
                    {
                        ProductName = e.Orders
                            .SelectMany(o => o.OrderDetails)
                            .Select(x => x.ProductLink.ProductName),
                        ShipCountry = e.Orders.Select(x => x.ShipCountry),
                        ShipRegion = e.Orders.Select(x => x.ShipRegion),
                        ShipCity = e.Orders.Select(x => x.ShipCity),
                        ShipAddress = e.Orders.Select(x => x.ShipAddress),
                    });
                var sql = query.ToSql();
            }

            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var employees_WhoSelled_AllKindsOfTofu = mysql.Employees
                    .Search("Tofu", e => new
                    {
                        ProductName = e.Orders
                            .SelectMany(o => o.OrderDetails)
                            .Select(x => x.ProductLink.ProductName)
                    });
                var sql1 = employees_WhoSelled_AllKindsOfTofu.ToSql();

                var employees_WhoSelled_Tofu = mysql.Employees
                     .Search("Tofu", e => new
                     {
                         ProductName = e.Orders
                             .SelectMany(o => o.OrderDetails)
                             .Select(x => x.ProductLink.ProductName)
                     }, SearchOption.Equals);
                var sql2 = employees_WhoSelled_Tofu.ToSql();

                var employees_WhoSelled_LongLifeTofu = mysql.Employees
                     .Search("Longlife Tofu", e => new
                     {
                         ProductName = e.Orders
                             .SelectMany(o => o.OrderDetails)
                             .Select(x => x.ProductLink.ProductName)
                     }, SearchOption.Equals);
                var sql3 = employees_WhoSelled_LongLifeTofu.ToSql();
            }
            return;
        }

    }
}
