using LinqSharp.EFCore.Data;
using LinqSharp.EFCore.Data.Test;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class XDbContextTests
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