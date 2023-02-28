using LinqSharp.EFCore.Data.Test;
using LinqSharp.EFCore.Navigation;
using Microsoft.EntityFrameworkCore;
using Northwnd;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class PreQueryTests
    {
        [Fact]
        public void SimpleTest()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            (string CategoryName, int Year)[] queryParams = new[]
            {
                ("Beverages", 1996),
                ("Seafood", 1997),
            };

            var preQueries = queryParams.Select(p =>
            {
                return new PreQuery<ApplicationDbContext, OrderDetail>(x => x.OrderDetails)
                    .Include(x => x.ProductLink.CategoryLink)
                    .Include(x => x.OrderLink)
                    .Where(x =>
                        x.ProductLink.CategoryLink.CategoryName == p.CategoryName
                        && x.OrderLink.OrderDate.Value.Year == p.Year);
            }).ToArray();
            var query = preQueries.FeedAll(mysql);

            Assert.Equal(240, query.Length);
            Assert.Equal(78, preQueries[0].Result.Length);
            Assert.Equal(162, preQueries[1].Result.Length);
        }

        [Fact]
        public void CombineTest()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            var preQueries = new[]
            {
                new PreQuery<ApplicationDbContext, Product>(x => x.Products)
                    .Include(x => x.OrderDetails).ThenInclude(x => x.OrderLink)
                    .Include(x => x.SupplierLink).ThenInclude(x => x.Products)
                    .Where(x => x.SupplierID == 1),

                new PreQuery<ApplicationDbContext, Product>(x => x.Products)
                    .Include(x => x.OrderDetails).ThenInclude(x => x.OrderLink)
                    .Include(x => x.SupplierLink).ThenInclude(x => x.Products)
                    .Where(x => x.SupplierID == 2),

                new PreQuery<ApplicationDbContext, Product>(x => x.Products)
                    .Include(x => x.OrderDetails).ThenInclude(x => x.OrderLink)
                    .Include(x => x.SupplierLink).ThenInclude(x => x.Products)
                    .Where(x => x.SupplierID == 3 && x.ProductID == 100),
            };

            Assert.Equal("x => (x.SupplierID == Convert(1, Nullable`1))", preQueries[0].ToString());
            Assert.Equal("x => (x.SupplierID == Convert(2, Nullable`1))", preQueries[1].ToString());
            Assert.Equal("x => ((x.SupplierID == Convert(3, Nullable`1)) AndAlso (x.ProductID == 100))", preQueries[2].ToString());

            var sql = (
                from x in preQueries.ToQuery(mysql)
                select new { x.ProductID, x.SupplierLink.CompanyName }
            ).ToQueryString();

#if EFCORE5_0_OR_GREATER
            Assert.Equal(@"SELECT `@`.`ProductID`, `@0`.`CompanyName`
FROM `@Northwnd.Products` AS `@`
LEFT JOIN `@Northwnd.Suppliers` AS `@0` ON `@`.`SupplierID` = `@0`.`SupplierID`
WHERE `@`.`SupplierID` IN (1, 2) OR ((`@`.`SupplierID` = 3) AND (`@`.`ProductID` = 100))", sql);

#elif EFCORE3_1_OR_GREATER
            Assert.Equal(@"SELECT `@`.`ProductID`, `@0`.`CompanyName`
FROM `@Northwnd.Products` AS `@`
LEFT JOIN `@Northwnd.Suppliers` AS `@0` ON `@`.`SupplierID` = `@0`.`SupplierID`
WHERE ((`@`.`SupplierID` = 1) OR (`@`.`SupplierID` = 2)) OR ((`@`.`SupplierID` = 3) AND (`@`.`ProductID` = 100));
", sql);

#elif EFCORE3_0_OR_GREATER
            Assert.Equal(@"SELECT `@`.`ProductID`, `@0`.`CompanyName`
FROM `@Northwnd.Products` AS `@`
LEFT JOIN `@Northwnd.Suppliers` AS `@0` ON `@`.`SupplierID` = `@0`.`SupplierID`
WHERE (((`@`.`SupplierID` = 1) AND `@`.`SupplierID` IS NOT NULL) OR ((`@`.`SupplierID` = 2) AND `@`.`SupplierID` IS NOT NULL)) OR (((`@`.`SupplierID` = 3) AND `@`.`SupplierID` IS NOT NULL) AND (`@`.`ProductID` = 100));
", sql);

#else
            Assert.Equal(@"SELECT `x`.`ProductID`, `x.SupplierLink`.`CompanyName`
FROM `@Northwnd.Products` AS `x`
LEFT JOIN `@Northwnd.Suppliers` AS `x.SupplierLink` ON `x`.`SupplierID` = `x.SupplierLink`.`SupplierID`
WHERE `x`.`SupplierID` IN (1, 2) OR ((`x`.`SupplierID` = 3) AND (`x`.`ProductID` = 100));
", sql);
#endif
        }
    }

}
