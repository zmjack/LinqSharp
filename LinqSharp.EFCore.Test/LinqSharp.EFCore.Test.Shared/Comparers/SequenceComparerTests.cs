using LinqSharp.Comparers;
using Xunit;

namespace LinqSharp.EFCore.Test.Shared.Comparers;

public class SequenceComparerTests
{
    [Fact]
    public void NormalTest()
    {
        var x = new List<int> { 1, 2, 3 };
        var y = new List<int> { 1, 2, 3 };
        var z = new List<int> { 1, 2, 4 };
        var comparer = new SequenceComparer<int>();
        Assert.True(comparer.Equals(x, y));
        Assert.False(comparer.Equals(x, z));
        Assert.False(comparer.Equals(x, null));
        Assert.False(comparer.Equals(null, y));
        Assert.True(comparer.Equals(null, null));
    }

}
