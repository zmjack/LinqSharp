using LinqSharp.EFCore.Data.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class KeyValueEntityTests
    {
        [Fact]
        public void AppRegistryTest()
        {
            var registries = new AppRegistry[]
            {
                new AppRegistry { Item = "Person1", Key = "Name", Value = "zmjack" },
                new AppRegistry { Item = "Person1", Key = "Age", Value = "28" },
                new AppRegistry { Item = "Person1", Key = "Enable", Value = "unknown" },

                new AppRegistry { Item = "Person2", Key = "Name", Value = "ashe" },
                new AppRegistry { Item = "Person2", Key = "Age", Value = "27" },
            };

            var zmjack = AppRegistryAgent.Attach(registries, "Person1");
            zmjack.Age = 999;

            Assert.Equal("Person1", zmjack.GetItemName());
            Assert.Equal(999, zmjack.Age);
            Assert.Equal("NickName", zmjack.NickName);
            Assert.False(zmjack.Enable);
            Assert.Null(zmjack.Address);

            Assert.Throws<KeyNotFoundException>(() => zmjack.NickName = "new");
            Assert.Equal("999", registries.FirstOrDefault(x => x.Key == nameof(AppRegistryAgent.Age))?.Value);
        }

        [Fact]
        public void AppRegistryDbTest()
        {
            using (var context = ApplicationDbContext.UseMySql())
            {
                var accessor = context.GetAppRegistriesAccessor();
                var zmjack = accessor.GetItem<AppRegistryAgent>("zmjack");
                zmjack.Enable = true;
                zmjack.Name = "zmjack";
                zmjack.Age = 29;
                zmjack.NickName = "zmjack";
                zmjack.Birthday = new DateTime(1991, 1, 1);
                context.SaveChanges();
            }

            using (var context = ApplicationDbContext.UseMySql())
            {
                var accessor = context.GetAppRegistriesAccessor();
                var zmjack = accessor.GetItem<AppRegistryAgent>("zmjack");
                Assert.True(zmjack.Enable);
                Assert.Equal("zmjack", zmjack.Name);
                Assert.Equal(29, zmjack.Age);
                Assert.Equal("zmjack", zmjack.NickName);
                Assert.Equal(new DateTime(1991, 1, 1), zmjack.Birthday);
                Assert.Null(zmjack.Address);
            }
        }

    }
}
