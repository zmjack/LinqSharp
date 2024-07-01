using LinqSharp.EFCore.Data.Test;
using NStandard;
using Xunit;

namespace LinqSharp.EFCore.Test;

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

        AppRegistry _tom, _jerry;

        using (var context = ApplicationDbContext.UseMySql())
        using (var query = context.BeginAgentQuery(x => x.AppRegistries))
        {
            _tom = query.GetAgent<AppRegistry>("/User/Tom");
            _jerry = query.GetAgent<AppRegistry>("/User/Jerry");
        }

        Assert.Equal("Default", _tom.Theme);
        Assert.Equal("grey", _tom.Color);
        Assert.Equal(50, _tom.Volume);
        Assert.Equal(now, _tom.LoginTime);
        Assert.False(_tom.Lock);

        Assert.Equal("Sky", _jerry.Theme);
        Assert.Equal("brown", _jerry.Color);
        Assert.Equal(10, _jerry.Volume);
        Assert.Equal(now, _jerry.LoginTime);
        Assert.True(_jerry.Lock);
    }

}
