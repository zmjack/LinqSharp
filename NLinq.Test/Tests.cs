using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xunit;

namespace NLinq.Test
{
    public class Tests
    {
        [Fact]
        public void Test1()
        {
            using (var mysql = new ApplicationDbContext())
            {
            }
        }
    }
}
