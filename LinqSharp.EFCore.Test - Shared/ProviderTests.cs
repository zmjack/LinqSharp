using LinqSharp.Data.Test;
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
            using (var context = ApplicationDbContext.UseMySql())
            {
                var item = new ProviderTestModel
                {
                    Password = "0416",
                    SimpleModel = new SimpleModel
                    {
                        NickName = "Jack",
                        Age = 29,
                        State = EState.Default,
                    }
                };

                context.ProviderTestModels.Add(item);
                context.SaveChanges();

                var storePassword = db.SqlQuery($"SELECT Password FROM ProviderTestModels;").ToArray().First()["Password"];
                Assert.Equal("MDQxNg==", storePassword);
                var storeFreeModel = db.SqlQuery($"SELECT FreeModel FROM ProviderTestModels;").ToArray().First()["FreeModel"];
                Assert.Equal(@"{""Id"":""00000000-0000-0000-0000-000000000000"",""Name"":""Jack"",""Age"":29,""State"":0}", storeFreeModel);

                var record = context.ProviderTestModels.First();
                Assert.Equal("0416", record.Password);
                Assert.Equal("Jack", record.SimpleModel.NickName);
                Assert.Equal(29, record.SimpleModel.Age);
                Assert.Equal(EState.Default, record.SimpleModel.State);

                context.ProviderTestModels.Remove(item);
                context.SaveChanges();
            }
        }
    }

}

