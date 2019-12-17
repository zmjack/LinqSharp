using Microsoft.EntityFrameworkCore;
using Northwnd;
using System.Linq;
using Xunit;

namespace NLinq.Test
{
    public class MySqlTests
    {
        [Fact]
        public void Test1()
        {
            using (var mysql = new NorthwndContext(
                new DbContextOptionsBuilder().UseMySql("server=127.0.0.1;database=northwnd").Options))
            {
                var query = mysql.Categories.Where(x => x.CategoryName == "Beverages");
                var result = query.First();
                var sql = query.ToSql();


                //var s = mysql.Suppliers.DistinctBy(x => x.Address).ToArray();
            }
        }

    }
}
