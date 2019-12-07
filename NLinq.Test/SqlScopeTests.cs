using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NLinq.Test
{
    public class SqlScopeTests
    {
        private class MySqlScope : SqlScope<MySqlConnection, MySqlCommand, MySqlParameter>
        {
            public MySqlScope() : this(new MySqlConnection("server=127.0.0.1;database=northwnd")) { }
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

                mysql.Sql($"insert into region (RegionID, RegionDescription) values ({regionId}, {description});");
                mysql.Sql($"delete from region where regionId={regionId}");
            }
        }

    }
}
