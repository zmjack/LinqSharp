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
            using var db = ApplicationDbScope.UseDefault();
            using var context = ApplicationDbContext.UseMySql();

            string GetPassword() => db.SqlQuery($"SELECT Password FROM LS_Providers;").First()[nameof(LS_Provider.Password)].ToString();
            string GetNameModel() => db.SqlQuery($"SELECT NameModel FROM LS_Providers;").First()[nameof(LS_Provider.NameModel)].ToString();

            context.LS_Providers.Delete(x => true);
            context.SaveChanges();

            var item = new LS_Provider
            {
                Password = "0416",
                NameModel = new NameModel { Name = "Jack", NickName = "zmjack" }
            };
            context.LS_Providers.Add(item);
            context.SaveChanges();
            Assert.Equal("MDQxNg==", GetPassword());
            Assert.Equal(@"{""Name"":""Jack"",""NickName"":""zmjack"",""Tag"":null}", GetNameModel());

            var record = context.LS_Providers.First();
            Assert.Equal("0416", record.Password);
            Assert.Equal("Jack", record.NameModel.Name);
            Assert.Equal("zmjack", record.NameModel.NickName);

            item.Password = "120416";
            context.SaveChanges();
            Assert.Equal("MTIwNDE2", GetPassword());

            item.NameModel.Tag = "Hi there.";
            context.SaveChanges();
            Assert.Equal(@"{""Name"":""Jack"",""NickName"":""zmjack"",""Tag"":""Hi there.""}", GetNameModel());

            context.LS_Providers.Delete(x => true);
            context.SaveChanges();
        }
    }

}

