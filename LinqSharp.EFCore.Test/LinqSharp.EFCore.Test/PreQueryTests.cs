using LinqSharp.EFCore.Data.Test;
using Northwnd;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class PreQueryTests
    {
        [Fact]
        public void Test1()
        {
            using var mysql = ApplicationDbContext.UseMySql();

            var query1 = new PreQuery<ApplicationDbContext, AuditLevel>(x => x.AuditLevels).Include(x => x.RootLink).Where(x => x.RootLink.TotalQuantity == 1);
            var query2 = new PreQuery<ApplicationDbContext, AuditLevel>(x => x.AuditLevels).Include(x => x.Values).Where(x => x.ValueCount == 2);
            var query3 = new PreQuery<ApplicationDbContext, AuditLevel>(x => x.AuditLevels).Include(x => x.Values).Where(x => x.Values.Any(x => x.Quantity == 3));

            var result = PreQuery.Execute(mysql, query1, query2, query3);
            var a1 = query1.Result;
        }

        [Fact]
        public void Test2()
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
                    .Include(x => x.ProductLink).ThenInclude(x => x.CategoryLink)
                    .Include(x => x.OrderLink)
                    .Where(x =>
                        x.ProductLink.CategoryLink.CategoryName == p.CategoryName
                        && x.OrderLink.OrderDate.Value.Year == p.Year);
            }).ToArray();
            var query = PreQuery.Execute(mysql, preQueries);

            foreach (var preQuery in preQueries)
            {
                var a = preQuery.Result;
            }
        }
    }

}
