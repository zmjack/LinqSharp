using LinqSharp.Dev;
using LinqSharp.EFCore.Data.Test;
using System;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class ToSqlTests
    {
        [Fact]
        public void Test()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var now = DateTime.Now;
                var query = mysql.YearMonthModels.Where(x => DbFunc.DateTime(x.Year, x.Month, 1) == DateTime.Now);
                var sql = query.ToSql();
                var result = query.ToArray();
            }
        }

        [Fact]
        public void WhereBeforeTest()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query = mysql.Employees
                    .WhereBefore(x => x.BirthDate, new DateTime(1960, 5, 31), true);

                var sql = query.ToSql();

                var result = query.ToArray();
                Assert.Equal(6, result.Length);
            }
        }

        [Fact]
        public void WhereBetweenTest()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query = mysql.Employees
                    .WhereBetween(x => x.BirthDate, new DateTime(1960, 5, 1), new DateTime(1960, 5, 31));

                var query1 = mysql.Employees
                    .Where(x => Convert.ToDateTime("1960-05-01") <= x.BirthDate && x.BirthDate <= new DateTime(1960, 5, 31));
                var sql1 = query1.ToSql();

                var result = query.ToArray();
                Assert.Single(result);
            }
        }

        [Fact]
        public void WhereMinTest()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query = mysql.Products.WhereMin(x => x.UnitPrice);
                var records = query.ToArray();
                Assert.Single(records);
                Assert.Equal(33, records[0].ProductID);
            }
        }

        [Fact]
        public void WhereMaxTest()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query = mysql.Products.WhereMax(x => x.UnitPrice);
                var records = query.ToArray();
                Assert.Single(records);
                Assert.Equal(38, records[0].ProductID);
            }
        }

        [Fact]
        public void WhereTest()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query = mysql.Employees
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
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query = mysql.Employees.TryDelete(x => x.Country == "China");
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

            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var originResult = mysql.Regions;
                var orderedResult =
                    mysql.Regions.OrderByCase(x => x.RegionDescription, new[] { "Northern", "Eastern", "Western", "Southern" });

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

            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var originResult = mysql.Regions;
                var orderedResult =
                    mysql.Regions.OrderByCase(x => x.RegionID, new[] { 3, 1, 2, 4 });

                Assert.Equal(new[] { 1, 2, 3, 4 }, originResult.Select(x => x.RegionID));
                Assert.Equal(new[] { 3, 1, 2, 4 }, orderedResult.Select(x => x.RegionID));
            }
        }

        [Fact]
        public void Test1()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query = mysql.Employees
                    .Search("London", e => new
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

            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var employees_WhoSelled_AllKindsOfTofu = mysql.Employees
                    .Search("Tofu", e => new
                    {
                        ProductName = e.Orders
                            .SelectMany(o => o.Order_Details)
                            .Select(x => x.Product.ProductName)
                    });
                var sql1 = employees_WhoSelled_AllKindsOfTofu.ToSql();

                var employees_WhoSelled_Tofu = mysql.Employees
                     .Search("Tofu", e => new
                     {
                         ProductName = e.Orders
                             .SelectMany(o => o.Order_Details)
                             .Select(x => x.Product.ProductName)
                     }, SearchOption.Equals);
                var sql2 = employees_WhoSelled_Tofu.ToSql();

                var employees_WhoSelled_LongLifeTofu = mysql.Employees
                     .Search("Longlife Tofu", e => new
                     {
                         ProductName = e.Orders
                             .SelectMany(o => o.Order_Details)
                             .Select(x => x.Product.ProductName)
                     }, SearchOption.Equals);
                var sql3 = employees_WhoSelled_LongLifeTofu.ToSql();
            }
            return;
        }

    }

}
