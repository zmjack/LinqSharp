using LinqSharp.Data.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqSharp.Test
{
    public class ProviderTests
    {
        [Fact]
        public void Test1()
        {
            using (var db = new ApplicationDbScope())
            using (var context = new ApplicationDbContext())
            {
                var item = new ProviderTestModel { Password = "0416" };

                context.ProviderTestModels.Add(item);
                context.SaveChanges();

                var storePassword = db.SqlQuery($"SELECT Password FROM ProviderTestModels;").ToArray().First()["Password"];
                Assert.Equal("MDQxNg==", storePassword);

                var ormPassword = context.ProviderTestModels.First().Password;
                Assert.Equal("0416", ormPassword);

                context.ProviderTestModels.Remove(item);
                context.SaveChanges();
            }
        }
    }

}

