using LinqSharp.EFCore.Data.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class KvEntityTests
    {
        [Fact]
        public void AppRegistryTest()
        {
            var regs = new AppRegistry[]
            {
                new AppRegistry { Item = "Person1", Key = "Name", Value = "zmjack" },
                new AppRegistry { Item = "Person1", Key = "Age", Value = "28" },
                new AppRegistry { Item = "Person1", Key = "Enable", Value = "unknown" },
                new AppRegistry { Item = "Person2", Key = "Name", Value = "ashe" },
                new AppRegistry { Item = "Person2", Key = "Age", Value = "27" },
            };

            var zmjack = AppRegistryAgent.Connect(regs, "Person1");
            zmjack.Age = 999;

            Assert.Equal("Person1", zmjack.GetItemString());
            Assert.Equal(999, zmjack.Age);
            Assert.Equal("haha", zmjack.NickName);
            Assert.False(zmjack.Enable);
            Assert.Null(zmjack.Address);

            Assert.Throws<KeyNotFoundException>(() => zmjack.NickName = "new");
            Assert.Equal("999", regs.FirstOrDefault(x => x.Key == nameof(AppRegistryAgent.Age))?.Value);
        }

        [Fact]
        public void AppRegistryDbTest()
        {
            using (var context = ApplicationDbContext.UseMySql())
            {
                var accessor = context.GetAppRegistriesAccessor();
                var appRegistry = accessor.Get<AppRegistryAgent>("zmjack");
                appRegistry.Enable = true;
                appRegistry.Name = "zmjack";
                appRegistry.Age = 29;
                appRegistry.Birthday = new DateTime(1991, 1, 1);
                context.SaveChanges();
            }

            using (var context = ApplicationDbContext.UseMySql())
            {
                var accessor = context.GetAppRegistriesAccessor();
                var appRegistry = accessor.Get<AppRegistryAgent>("zmjack");
                Assert.True(appRegistry.Enable);
                Assert.Equal("zmjack", appRegistry.Name);
                Assert.Equal(29, appRegistry.Age);
                Assert.Equal(new DateTime(1991, 1, 1), appRegistry.Birthday);
                Assert.Null(appRegistry.Address);
            }
        }

    }
}
