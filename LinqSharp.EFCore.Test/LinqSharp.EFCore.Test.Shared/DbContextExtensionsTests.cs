using LinqSharp.EFCore.Data;
using LinqSharp.EFCore.Data.Test;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class DbContextExtensionsTests
    {
        [Fact]
        public void Test1()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            var name = mysql.GetTableName<LS_Name>();
            Assert.Equal("LS_Names", name);
        }
    }
}