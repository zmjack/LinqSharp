﻿using LinqSharp.Data.Test;
using Xunit;

namespace LinqSharp.Test
{
    public class MySqlTests
    {
        [Fact]
        public void Test1()
        {
            using (var mysql = ApplicationDbContext.UseDefault())
            {
                //var s = mysql.Suppliers.DistinctBy(x => x.Address).ToArray();
            }
        }

    }
}
