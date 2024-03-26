using LinqSharp.EFCore.Data.Test;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class ProviderNameTests
{
    [Fact]
    public void Test1()
    {
        using var context = ApplicationDbContext.UseMySql();
        var providerName = context.SimpleRows.GetProviderName();
        Assert.Equal(ProviderName.MySql, providerName);
    }

}
