using Microsoft.EntityFrameworkCore;
using Northwnd;
using System;
using System.Linq;
using Xunit;

namespace LinqSharp.Test
{
    public class ToSqlTests
    {
        private readonly DbContextOptions MySqlOptions = new DbContextOptionsBuilder().UseMySql("Server=127.0.0.1").Options;

        [Fact]
        public void Test()
        {
            string sql;
            using (var sqlite = NorthwndContext.UseSqliteResource())
            {
                sql = sqlite.Employees
                    .WhereSearch(new[] { "Tofu", "123" }, e => new
                    {
                        ProductName = e.Orders
                            .SelectMany(o => o.Order_Details)
                            .Select(x => x.Product.ProductName)
                    }).ToSql();
            }
        }

        [Fact]
        public void WhereBeforeTest()
        {
            using (var sqlite = NorthwndContext.UseSqliteResource())
            {
                var query = sqlite.Employees
                    .WhereBefore(x => x.BirthDate, new DateTime(1960, 5, 31), true);

                var sql = query.ToSql();

                var result = query.ToArray();
                Assert.Equal(6, result.Length);
            }
        }

        [Fact]
        public void WhereBetweenTest()
        {
            using (var sqlite = NorthwndContext.UseSqliteResource())
            {
                var query = sqlite.Employees
                    .WhereBetween(x => x.BirthDate, new DateTime(1960, 5, 1), new DateTime(1960, 5, 31));

                var query1 = sqlite.Employees
                    .Where(x => Convert.ToDateTime("1960-05-01") <= x.BirthDate && x.BirthDate <= new DateTime(1960, 5, 31));
                var sql1 = query1.ToSql();

                var result = query.ToArray();
                Assert.Single(result);
            }
        }

        [Fact]
        public void WhereMinTest()
        {
            using (var sqlite = NorthwndContext.UseSqliteResource())
            {
                var query = sqlite.Products.WhereMin(x => x.UnitPrice);
                var records = query.ToArray();
                Assert.Single(records);
                Assert.Equal(33, records[0].ProductID);
            }
        }

        [Fact]
        public void WhereMaxTest()
        {
            using (var sqlite = NorthwndContext.UseSqliteResource())
            {
                var query = sqlite.Products.WhereMax(x => x.UnitPrice);
                var records = query.ToArray();
                Assert.Single(records);
                Assert.Equal(38, records[0].ProductID);
            }
        }

        [Fact]
        public void WhereTest()
        {
            using (var sqlite = NorthwndContext.UseSqliteResource())
            {
                var query = sqlite.Employees
                    .WhereAfter(
                        yearExp: x => x.Country,
                        monthExp: x => x.EmployeeID,
                        dayExp: x => x.EmployeeID,
                        after: DateTime.Now);
                var sql = query.ToSql();

                //var result = query.ToArray();
                //Assert.Single(result);
            }
        }

        [Fact]
        public void TryDeleteTest()
        {
            using (var sqlite = NorthwndContext.UseSqliteResource())
            {
                var query = sqlite.Employees.TryDelete(x => x.Country == "China");
                var sql = query.ToSql();

                //var result = query.ToArray();
                //Assert.Single(result);
            }
        }

        [Fact]
        public void OrderByCaseTest1()
        {
            //  RegionID    RegionDescription
            //  1   Eastern
            //  2   Western
            //  3   Northern
            //  4   Southern

            using (var sqlite = NorthwndContext.UseSqliteResource())
            {
                var originResult = sqlite.Regions;
                var orderedResult =
                    sqlite.Regions.OrderByCase(x => x.RegionDescription, new[] { "Northern", "Eastern", "Western", "Southern" });

                Assert.Equal(new[] { 1, 2, 3, 4 }, originResult.Select(x => x.RegionID));
                Assert.Equal(new[] { 3, 1, 2, 4 }, orderedResult.Select(x => x.RegionID));
            }
        }

        [Fact]
        public void OrderByCaseTest2()
        {
            //  RegionID    RegionDescription
            //  1   Eastern
            //  2   Western
            //  3   Northern
            //  4   Southern

            using (var sqlite = NorthwndContext.UseSqliteResource())
            {
                var originResult = sqlite.Regions;
                var orderedResult =
                    sqlite.Regions.OrderByCase(x => x.RegionID, new[] { 3, 1, 2, 4 });

                Assert.Equal(new[] { 1, 2, 3, 4 }, originResult.Select(x => x.RegionID));
                Assert.Equal(new[] { 3, 1, 2, 4 }, orderedResult.Select(x => x.RegionID));
            }
        }

        [Fact]
        public void Test1()
        {
            using (var sqlite = NorthwndContext.UseSqliteResource())
            {
                var query = sqlite.Employees
                    .WhereSearch("London", e => new
                    {
                        ProductName = e.Orders
                            .SelectMany(o => o.Order_Details)
                            .Select(x => x.Product.ProductName),
                        ShipCountry = e.Orders.Select(x => x.ShipCountry),
                        ShipRegion = e.Orders.Select(x => x.ShipRegion),
                        ShipCity = e.Orders.Select(x => x.ShipCity),
                        ShipAddress = e.Orders.Select(x => x.ShipAddress),
                    });
                var sql = query.ToSql();
            }

            var now = DateTime.Now.AddDays(-1).AddHours(-2);

            using (var mysql = new NorthwndContext(MySqlOptions))
            using (var sqlite = NorthwndContext.UseSqliteResource())
            {
                var employees_WhoSelled_AllKindsOfTofu = sqlite.Employees
                    .WhereSearch("Tofu", e => new
                    {
                        ProductName = e.Orders
                            .SelectMany(o => o.Order_Details)
                            .Select(x => x.Product.ProductName)
                    });
                var sql1 = employees_WhoSelled_AllKindsOfTofu.ToSql();

                var employees_WhoSelled_Tofu = sqlite.Employees
                     .WhereMatch("Tofu", e => new
                     {
                         ProductName = e.Orders
                             .SelectMany(o => o.Order_Details)
                             .Select(x => x.Product.ProductName)
                     });
                var sql2 = employees_WhoSelled_Tofu.ToSql();

                var employees_WhoSelled_LongLifeTofu = sqlite.Employees
                     .WhereMatch("Longlife Tofu", e => new
                     {
                         ProductName = e.Orders
                             .SelectMany(o => o.Order_Details)
                             .Select(x => x.Product.ProductName)
                     });
                var sql3 = employees_WhoSelled_LongLifeTofu.ToSql();
            }
            return;
        }

    }

}
