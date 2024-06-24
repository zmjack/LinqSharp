using System;
using Xunit;
using static NStandard.Measures.StorageCapacity;

namespace LinqSharp.EFCore.Test;

public class QSum_And_QAverageTests
{
    [Fact]
    public void ValueTest()
    {
        b[] values = [10, 20, 45];
        Assert.Equal(75, values.QSum());
        Assert.Equal(25, values.QAverage());
        Assert.Equal(25, values.QAverageOrDefault());
        Assert.Equal(25, values.QAverageOrDefault(-1));
    }

    [Fact]
    public void NullableValueTest()
    {
        b?[] values = [10, 20, 45];
        Assert.Equal(75, values.QSum());
        Assert.Equal(25, values.QAverage());
        Assert.Equal(25, values.QAverageOrDefault());
        Assert.Equal(25, values.QAverageOrDefault(-1));
    }

    [Fact]
    public void EmptyValueTest()
    {
        b[] values = [];
        Assert.Equal(0, values.QSum());
        Assert.ThrowsAny<InvalidOperationException>(() => values.QAverage());
        Assert.Equal(default, values.QAverageOrDefault());
        Assert.Equal(-1, values.QAverageOrDefault(-1));
    }

    [Fact]
    public void EmptyNullableValueTest()
    {
        b?[] values = [];
        Assert.Equal(0, values.QSum());
        Assert.Null(values.QAverage());
        Assert.Null(values.QAverageOrDefault());
        Assert.Equal(-1, values.QAverageOrDefault(-1));
    }

}
