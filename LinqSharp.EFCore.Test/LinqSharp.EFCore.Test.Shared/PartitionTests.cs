using System;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class PartitionTests
{
    [Fact]
    public void Test1()
    {
        var arr = new[] { 1, 2, 3 };
        var partitions = arr.PartitionBy(x => x < 3, x => x < 5, x => x < 7);
        Assert.Equal(new[] { 1, 2 }, partitions[0]);
        Assert.Equal(new[] { 3 }, partitions[1]);
        Assert.Equal(Array.Empty<int>(), partitions[2]);
    }
}
