using LinqSharp.EFCore.Data.Test;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class AgentQueryTests
    {
        [Fact]
        public void AgentTests()
        {
            var now = DateTime.Now.StartOfSecond();

            using (var context = ApplicationDbContext.UseMySql())
            using (context.BeginDirectQuery())
            {
                context.AppRegistries.Truncate();

            }

            using (var context = ApplicationDbContext.UseMySql())
            using (var query = context.BeginAgentQuery(x => x.AppRegistries))
            {
                var tom = query.GetAgent<AppRegistry>("/User/Tom");
                var jerry = query.GetAgent<AppRegistry>("/User/Jerry");

                tom.Color = "grey";
                tom.Volume = 50;
                tom.LoginTime = now;

                jerry.Theme = "Sky";
                jerry.Color = "brown";
                jerry.Volume = 10;
                jerry.LoginTime = now;
                jerry.Lock = true;

                context.SaveChanges();
            }

            using (var context = ApplicationDbContext.UseMySql())
            using (var query = context.BeginAgentQuery(x => x.AppRegistries))
            {
                var tom = query.GetAgent<AppRegistry>("/User/Tom");
                var jerry = query.GetAgent<AppRegistry>("/User/Jerry");

                Assert.Equal("Default", tom.Theme);
                Assert.Equal("grey", tom.Color);
                Assert.Equal(50, tom.Volume);
                Assert.Equal(now, tom.LoginTime);
                Assert.False(tom.Lock);

                Assert.Equal("Sky", jerry.Theme);
                Assert.Equal("brown", jerry.Color);
                Assert.Equal(10, jerry.Volume);
                Assert.Equal(now, jerry.LoginTime);
                Assert.True(jerry.Lock);
            }
        }

    }
}
