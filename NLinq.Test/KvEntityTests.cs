using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace NLinq.Test
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

        public class AppRegistryTest
        {
            [Fact]
            public void Test1()
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
        }

        public class AppRegistryDbTest
        {
            [Fact]
            public void Test1()
            {
                using (var context = new ApplicationDbContext())
                {
                    var appRegistry = context.AppRegistriesAgent["zmjack"];
                    appRegistry.Enable = true;
                    appRegistry.Name = "zmjack";
                    appRegistry.Age = 29;

                    context.SaveChanges();
                }

                using (var context = new ApplicationDbContext())
                {
                    var zmjack = context.AppRegistriesAgent["zmjack"];
                    zmjack.Name = "zmjack";
                    zmjack.Age = 29;

                    Assert.True(zmjack.Enable);
                    Assert.Equal("zmjack", zmjack.Name);
                    Assert.Equal(29, zmjack.Age);
                    Assert.Null(zmjack.Address);
                }
            }
        }

    }
}
