using LinqSharp.EFCore.Data.Test;
using LinqSharp.EFCore.Models.Test;
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
                var item = new LS_Provider
                {
                    Password = "0416",
                    NameModel = new NameModel { Name = "Jack", NickName = "zmjack" }
                };

                context.LS_Providers.Add(item);
                context.SaveChanges();

                var password = db.SqlQuery($"SELECT Password FROM LS_Providers;").ToArray().First()[nameof(LS_Provider.Password)];
                Assert.Equal("MDQxNg==", password);
                var nameModel = db.SqlQuery($"SELECT NameModel FROM LS_Providers;").ToArray().First()[nameof(LS_Provider.NameModel)];
                Assert.Equal(@"{""Name"":""Jack"",""NickName"":""zmjack""}", nameModel);

                var record = context.LS_Providers.First();
                Assert.Equal("0416", record.Password);
                Assert.Equal("Jack", record.NameModel.Name);
                Assert.Equal("zmjack", record.NameModel.NickName);

                context.LS_Providers.Remove(item);
                context.SaveChanges();
            }
        }
    }

}

