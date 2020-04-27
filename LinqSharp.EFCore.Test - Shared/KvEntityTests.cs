using LinqSharp.Data.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LinqSharp.Test
{
    public class KvEntityTests
    {
        public class _AppRegistry : KvEntity { }

        public class _AppRegistryAccessor : KvEntityAccessor<_AppRegistryAccessor, _AppRegistry>
        {
            public virtual string Name { get; set; }

            public virtual int Age { get; set; }

            public virtual string NickName { get; set; } = "haha";

            public virtual string Address { get; set; }
        }

        [Fact]
        public void AppRegistryTest()
        {
            var regs = new _AppRegistry[]
            {
                    new _AppRegistry { Item = "Person1", Key = "Name", Value = "zmjack" },
                    new _AppRegistry { Item = "Person1", Key = "Age", Value = "28" },
                    new _AppRegistry { Item = "Person2", Key = "Name", Value = "ashe" },
                    new _AppRegistry { Item = "Person2", Key = "Age", Value = "27" },
            };

            var zmjack = _AppRegistryAccessor.Connect(regs, "Person1");
            zmjack.Age = 999;

            Assert.Equal("Person1", zmjack.GetItemString());
            Assert.Equal(999, zmjack.Age);
            Assert.Equal("haha", zmjack.NickName);
            Assert.Null(zmjack.Address);

            Assert.Throws<KeyNotFoundException>(() => zmjack.NickName = "new");
            Assert.Equal("999", regs.FirstOrDefault(x => x.Key == nameof(_AppRegistryAccessor.Age))?.Value);
        }

        [Fact]
        public void AppRegistryDbTest()
        {
            using (var context = ApplicationDbContext.UseMySql())
            {
                var appRegistry = context.AppRegistriesAgent["zmjack"];
                appRegistry.Enable = true;
                appRegistry.Name = "zmjack";
                appRegistry.Age = 29;
                appRegistry.Birthday = new DateTime(1991, 1, 1);

                context.SaveChanges();
            }

            using (var context = ApplicationDbContext.UseMySql())
            {
                var zmjack = context.AppRegistriesAgent["zmjack"];
                Assert.True(zmjack.Enable);
                Assert.Equal("zmjack", zmjack.Name);
                Assert.Equal(29, zmjack.Age);
                Assert.Equal(new DateTime(1991, 1, 1), zmjack.Birthday);
                Assert.Null(zmjack.Address);
            }
        }

    }
}
