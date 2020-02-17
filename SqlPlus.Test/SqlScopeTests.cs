using SqlPlus.Data.Test;
using System;
using Xunit;

namespace SqlPlus.Test
{
    public class SqlScopeTests
    {
        [Fact]
        public void Test1()
        {
            using (var mysql = ApplicationDbScope.UseDefault())
            {
                var regionId = 5;
                var description = "Center";
                var now = DateTime.Now;

                mysql.Sql($"INSERT INTO `@Northwnd.Regions` (RegionID, RegionDescription) VALUES ({regionId}, {description});");
                mysql.Sql($"DELETE FROM `@Northwnd.Regions` WHERE RegionID={regionId}");
            }
        }

    }
}
