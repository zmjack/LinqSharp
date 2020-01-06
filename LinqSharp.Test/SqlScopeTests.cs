using MySql.Data.MySqlClient;
using System;
using Xunit;

namespace LinqSharp.Test
{
    public class SqlScopeTests
    {
        private class MySqlScope : SqlScope<MySqlConnection, MySqlCommand, MySqlParameter>
        {
            public MySqlScope() : this(new MySqlConnection(ApplicationDbContext.CONNECT_STRING)) { }
            public MySqlScope(MySqlConnection model) : base(model) { }
        }

        [Fact]
        public void Test1()
        {
            using (var mysql = new MySqlScope())
            {
                var regionId = 5;
                var description = "Center";
                var now = DateTime.Now;

                mysql.Sql($"insert into regions (RegionID, RegionDescription) values ({regionId}, {description});");
                mysql.Sql($"delete from regions where regionId={regionId}");
            }
        }

    }
}
