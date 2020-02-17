using LinqSharp.Data.Test;
using SqlPlus.Data.Test;
using System.Linq;
using Xunit;

namespace LinqSharp.Test
{
    public class ProviderTests
    {
        [Fact]
        public void Test1()
        {
            using (var db = ApplicationDbScope.UseDefault())
            using (var context = ApplicationDbContext.UseDefault())
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

