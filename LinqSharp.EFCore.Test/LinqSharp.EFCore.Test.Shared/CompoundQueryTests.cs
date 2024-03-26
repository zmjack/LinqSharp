using LinqSharp.EFCore.Data.Test;
using Microsoft.EntityFrameworkCore;
using Northwnd.Data;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class CompoundQueryTests
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

        var queryDefs = queryParams.Select(p =>
        {
            return new QueryDef<OrderDetail>()
                .Where(x =>
                    x.ProductLink.CategoryLink.CategoryName == p.CategoryName
                    && x.OrderLink.OrderDate.Value.Year == p.Year);
        }).ToArray();

        using (var query = mysql.BeginCompoundQuery(x => x.OrderDetails
            .AsNoTracking()
            .Include(x => x.ProductLink.CategoryLink)
            .Include(x => x.OrderLink)))
        {
            var combinedResult = query.Feed(queryDefs);
            Assert.Equal(240, combinedResult.Length);
        }

        //container
        //    .Include(x => x.ProductLink.CategoryLink)
        //    .Include(x => x.OrderLink);
        Assert.Equal(78, queryDefs[0].Result.Length);
        Assert.Equal(162, queryDefs[1].Result.Length);
    }

    [Fact]
    public void CombineTest()
    {
        using var mysql = ApplicationDbContext.UseMySql();

        using var query = mysql.BeginCompoundQuery(x => x.Products);
        query
            .Include(x => x.OrderDetails).ThenInclude(x => x.OrderLink)
            .Include(x => x.SupplierLink).ThenInclude(x => x.Products);

        var queryDefs = new[]
        {
            new QueryDef<Product>().Where(x => x.SupplierID == 1),
            new QueryDef<Product>().Where(x => x.SupplierID == 2),
            new QueryDef<Product>().Where(x => x.SupplierID == 3 && x.ProductID == 100),
        };

        Assert.Equal("x => (x.SupplierID == Convert(1, Nullable`1))", queryDefs[0].ToString());
        Assert.Equal("x => (x.SupplierID == Convert(2, Nullable`1))", queryDefs[1].ToString());
        Assert.Equal("x => ((x.SupplierID == Convert(3, Nullable`1)) AndAlso (x.ProductID == 100))", queryDefs[2].ToString());

        var sql = (
            from x in query.BuildQuery(queryDefs)
            select new { x.ProductID, x.SupplierLink.CompanyName }
        ).ToQueryString();

#if EFCORE5_0_OR_GREATER
        Assert.Equal(@"SELECT `@`.`ProductID`, `@0`.`CompanyName`
FROM `@n.Products` AS `@`
LEFT JOIN `@n.Suppliers` AS `@0` ON `@`.`SupplierID` = `@0`.`SupplierID`
WHERE `@`.`SupplierID` IN (1, 2) OR ((`@`.`SupplierID` = 3) AND (`@`.`ProductID` = 100))", sql);

#elif EFCORE3_1_OR_GREATER
        Assert.Equal(@"SELECT `@`.`ProductID`, `@0`.`CompanyName`
FROM `@n.Products` AS `@`
LEFT JOIN `@n.Suppliers` AS `@0` ON `@`.`SupplierID` = `@0`.`SupplierID`
WHERE ((`@`.`SupplierID` = 1) OR (`@`.`SupplierID` = 2)) OR ((`@`.`SupplierID` = 3) AND (`@`.`ProductID` = 100));
", sql);

#else
        Assert.Equal(@"SELECT `x`.`ProductID`, `x.SupplierLink`.`CompanyName`
FROM `@n.Products` AS `x`
LEFT JOIN `@n.Suppliers` AS `x.SupplierLink` ON `x`.`SupplierID` = `x.SupplierLink`.`SupplierID`
WHERE `x`.`SupplierID` IN (1, 2) OR ((`x`.`SupplierID` = 3) AND (`x`.`ProductID` = 100));
", sql);
#endif
    }
}
