using LinqSharp.EFCore.Models.Test;
using NStandard;
using Xunit;

namespace LinqSharp.EFCore.Test.Shared;

public class IndexTests
{
    private readonly NameModel[] _models = new NameModel[10].Let(i => new NameModel
    {
        Name = i.ToString(),
        NickName = $"NN: {i}",
        Tag = (i / 5).ToString()
    });

    [Fact]
    public void IndexingNormalTest()
    {
        var modelsByTag = _models.IndexBy(x => x.Tag);
        var modelsByNameNickName = _models.IndexBy(x => new { x.Name, x.NickName });

        Assert.Equal(0, modelsByTag["0"].Sum(x => int.Parse(x.Tag)));
        Assert.Equal(5, modelsByTag["1"].Sum(x => int.Parse(x.Tag)));
        Assert.Equal(0, modelsByTag["2"].Sum(x => int.Parse(x.Tag)));
        Assert.Equal("1", modelsByNameNickName[new { Name = "5", NickName = "NN: 5" }].First().Tag);
    }

    [Fact]
    public void IndexingNullTest()
    {
        var modelByTag = _models.IndexBy(x => (string)null);
        Assert.Equal(5, modelByTag[null].Sum(x => int.Parse(x.Tag)));
    }

    [Fact]
    public void UniqueIndexingNormalTest()
    {
        var modelByTag = (
            from x in _models
            group x by x.Tag into g
            select g.First()
        ).UniqueIndexBy(x => x.Tag);

        Assert.Equal("0", modelByTag["0"].Target.Tag);
        Assert.Equal("1", modelByTag["1"].Target.Tag);
        Assert.Null(modelByTag["2"]);
    }

    [Fact]
    public void UniqueIndexingNullTest()
    {
        var modelByTag = _models.Take(1).UniqueIndexBy(x => (string)null);
        Assert.Equal("0", modelByTag[null].Target.Tag);
    }

    [Fact]
    public void UniqueIndexingThrowTest()
    {
        var indexed = _models.UniqueIndexBy(x => x.Tag);
        Assert.ThrowsAny<ArgumentException>(() => indexed[null]);
    }
}
