using LinqSharp.EFCore.Models.Test;
using NStandard;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class IEnumerableExtensionsTests
{
    [Fact]
    public void PageTest()
    {
        var items = new int[10].Let(i => i);
        Assert.Equal(4, items.Page(2, 3).PageCount);
        Assert.Equal(2, items.Page(2, 5).PageCount);
        Assert.True(items.Page(1, 3).IsFristPage);
        Assert.True(items.Page(4, 3).IsLastPage);
        Assert.Equal([0, 1, 2, 3, 4], items.Page(1, 5).ToArray());
        Assert.Equal([5, 6, 7, 8, 9], items.Page(2, 5).ToArray());
    }

    [Fact]
    public void PadTest()
    {
        var items = new[]
        {
            new NameModel { Name = "A", NickName = "a", Tag = "01" },
            new NameModel { Name = "C", NickName = "c", Tag = "01" },
        };

        var filled1 = items.Pad(x => x.Name, ["A", "B", "C"], key => new NameModel { Name = key, NickName = $":{key.ToLower()}" });
        Assert.Equal(["A", "C", "B"], filled1.Select(x => x.Name).ToArray());
        Assert.Equal(["a", "c", ":b"], filled1.Select(x => x.NickName).ToArray());

        var filled2 = items.Pad(x => new { x.Name, x.Tag },
        [
            new { Name = "A", Tag = "01" },
            new { Name = "B", Tag = "01" },
            new { Name = "C", Tag = "01" },
        ], key => new NameModel { Name = key.Name, NickName = $":{key.Name.ToLower()}", Tag = key.Tag });
        Assert.Equal(["A", "C", "B"], filled2.Select(x => x.Name).ToArray());
        Assert.Equal(["a", "c", ":b"], filled2.Select(x => x.NickName).ToArray());
    }

    [Fact]
    public void PadLeftTest()
    {
        var array = new[] { 1, 2, 3 };
        var expected = array.PadFirst(5);
        Assert.Equal(expected, new[] { 0, 0, 1, 2, 3 });
    }

    [Fact]
    public void PadRightTest()
    {
        var array = new[] { 1, 2, 3 };
        var expected = array.PadLast(5);
        Assert.Equal(expected, new[] { 1, 2, 3, 0, 0 });
    }

    [Fact]
    public void AllSameTest()
    {
        var items = new[]
        {
            new NameModel { Name = "A", NickName = "a", Tag = "01" },
            new NameModel { Name = "B", NickName = "b", Tag = "01" },
        };
        Assert.True(items.AllSame(x => x.Tag));
        Assert.False(items.AllSame(x => x.Name));
    }

    [Fact]
    public void Tile()
    {
        var array = new[] { 1, 2, 3 };
        var expected = array.Tile(3);
        Assert.Equal(expected, new[] { 1, 2, 3, 1, 2, 3, 1, 2, 3 });
    }

    [Fact]
    public void Repeat()
    {
        var array = new[] { 1, 2, 3 };
        var expected = array.Repeat(3);
        Assert.Equal(expected, new[] { 1, 1, 1, 2, 2, 2, 3, 3, 3 });
    }

}
