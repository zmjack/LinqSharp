using LinqSharp.EFCore.Data.Test;
using LinqSharp.EFCore.Models.Test;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class ProviderTests
{
    [Fact]
    public void Test1()
    {
        using var db = ApplicationDbScope.UseDefault();
        using var context = ApplicationDbContext.UseMySql();

        string GetPassword() => db.SqlQuery($"SELECT Password FROM LS_Providers;").First()[nameof(LS_Provider.Password)].ToString();
        string GetNameModel() => db.SqlQuery($"SELECT NameModel FROM LS_Providers;").First()[nameof(LS_Provider.NameModel)].ToString();
        string GetJsonModel() => db.SqlQuery($"SELECT JsonModel FROM LS_Providers;").First()[nameof(LS_Provider.JsonModel)].ToString();
        string GetDictionaryModel() => db.SqlQuery($"SELECT DictionaryModel FROM LS_Providers;").First()[nameof(LS_Provider.DictionaryModel)].ToString();

        using (context.BeginDirectQuery())
        {
            context.LS_Providers.Truncate();
        }

        var item = new LS_Provider
        {
            Password = "0416",
            NameModel = new NameModel { Name = "Jack", NickName = "zmjack" },
            JsonModel = new NameModel { Name = "Jack", NickName = "zmjack" },
            DictionaryModel = new Dictionary<string, string>
            {
                ["Field"] = "Field Value",
            },
        };
        context.LS_Providers.Add(item);
        context.SaveChanges();

        Assert.Equal("MAA0ADEANgA=", GetPassword());
        Assert.Equal(@"{""Name"":""Jack"",""NickName"":""zmjack"",""Tag"":null}", GetNameModel());
        Assert.Equal(@"{""Name"":""Jack"",""NickName"":""zmjack"",""Tag"":null}", GetJsonModel());
        Assert.Equal(@"{""Field"":""Field Value""}", GetDictionaryModel());

        var record = context.LS_Providers.First();
        Assert.Equal("0416", record.Password);
        Assert.Equal("Jack", record.NameModel.Name);
        Assert.Equal("zmjack", record.NameModel.NickName);

        item.Password = "120416";
        context.SaveChanges();
        Assert.Equal("MQAyADAANAAxADYA", GetPassword());

        item.NameModel.Tag = "Hi there.";
        context.SaveChanges();
        Assert.Equal(@"{""Name"":""Jack"",""NickName"":""zmjack"",""Tag"":""Hi there.""}", GetNameModel());

        using (context.BeginDirectQuery())
        {
            context.LS_Providers.Truncate();
        }

        context.SaveChanges();
    }
}

