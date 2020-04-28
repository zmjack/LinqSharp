using LinqSharp.EFCore.Data.Test;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
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
                        Name = "Jack",
                        Age = 29,
                        State = EState.Default,
                    }
                };

                context.ProviderTestModels.Add(item);
                context.SaveChanges();

                var password = db.SqlQuery($"SELECT Password FROM ProviderTestModels;").ToArray().First()[nameof(ProviderTestModel.Password)];
                Assert.Equal("MDQxNg==", password);
                var simpleModel = db.SqlQuery($"SELECT SimpleModel FROM ProviderTestModels;").ToArray().First()[nameof(ProviderTestModel.SimpleModel)];
                Assert.Equal(@"{""Id"":""00000000-0000-0000-0000-000000000000"",""Name"":""Jack"",""Age"":29,""Birthday"":null,""State"":0}", simpleModel);

                var record = context.ProviderTestModels.First();
                Assert.Equal("0416", record.Password);
                Assert.Equal("Jack", record.SimpleModel.Name);
                Assert.Equal(29, record.SimpleModel.Age);
                Assert.Equal(EState.Default, record.SimpleModel.State);

                context.ProviderTestModels.Remove(item);
                context.SaveChanges();
            }
        }
    }

}

