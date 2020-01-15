using LinqSharp.Data.Test;
using MySql.Data.MySqlClient;
using System;
using Xunit;

namespace LinqSharp.Test
{
    public class SqlScopeTests
    {
        [Fact]
        public void Test1()
        {
            using (var mysql = new ApplicationDbScope())
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
