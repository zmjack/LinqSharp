using LinqSharp.EFCore.Data.Test;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class MySqlTests
    {
        [Fact]
        public void Test1()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                //var s = mysql.Suppliers.DistinctBy(x => x.Address).ToArray();
            }
        }

    }
}
