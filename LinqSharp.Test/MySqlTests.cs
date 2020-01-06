using System.Linq;
using Xunit;

namespace LinqSharp.Test
{
    public class MySqlTests
    {
        [Fact]
        public void Test1()
        {
            using (var mysql = new ApplicationDbContext())
            {
                var query = mysql.Categories.Where(x => x.CategoryName == "Beverages");
                var result = query.First();
                var sql = query.ToSql();

                //var s = mysql.Suppliers.DistinctBy(x => x.Address).ToArray();
            }
        }

    }
}
