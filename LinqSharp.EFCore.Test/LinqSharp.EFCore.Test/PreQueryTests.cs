using LinqSharp.EFCore.Data.Test;
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

            var query1 = mysql.CreatePreQuery(x => x.AuditLevels).Include(x => x.RootLink).Where(x => x.RootLink.TotalQuantity == 1);
            var query2 = mysql.CreatePreQuery(x => x.AuditLevels).Include(x => x.Values).Where(x => x.ValueCount == 2);
            var query3 = mysql.CreatePreQuery(x => x.AuditLevels).Include(x => x.Values).Where(x => x.Values.Any(x => x.Quantity == 3));

            var result = mysql.ExcuteQueries(query1, query2, query3);

            var a1 = query1.ToEnumerable();
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
                return mysql.CreatePreQuery(x => x.OrderDetails)
                    .Include(x => x.ProductLink).ThenInclude(x => x.CategoryLink)
                    .Include(x => x.OrderLink)
                    .Where(x =>
                        x.ProductLink.CategoryLink.CategoryName == p.CategoryName
                        && x.OrderLink.OrderDate.Value.Year == p.Year);
            }).ToArray();
            var query = mysql.ExcuteQueries(preQueries);

            foreach (var preQuery in preQueries)
            {
                var a = preQuery.ToEnumerable();
            }
        }

        [Fact]
        public void Test3()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            var result = mysql.CreatePreQuery(x => x.Products).Include(x => x.OrderDetails).Excute();
        }
    }

}
