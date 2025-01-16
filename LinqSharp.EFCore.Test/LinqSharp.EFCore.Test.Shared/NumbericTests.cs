using LinqSharp.Design;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class NumericTests
{
    private struct NumericStruct : ISummable
    {
        public int Value { get; set; }
        public static NumericStruct operator +(NumericStruct left, NumericStruct right) => new() { Value = left.Value + right.Value };
        public static NumericStruct operator -(NumericStruct left, NumericStruct right) => new() { Value = left.Value - right.Value };
        public static NumericStruct operator *(NumericStruct left, int multiple) => new() { Value = left.Value * multiple };
        public static NumericStruct operator /(NumericStruct left, int divisor) => new() { Value = left.Value / divisor };
        public static NumericStruct operator *(NumericStruct left, long multiple) => new() { Value = checked((int)(left.Value * multiple)) };
        public static NumericStruct operator /(NumericStruct left, long divisor) => new() { Value = checked((int)(left.Value / divisor)) };
    }

    private class NumericClass : ISummable
    {
        public int Value { get; set; }
        public static NumericClass operator +(NumericClass left, NumericClass right) => new() { Value = left.Value + right.Value };
        public static NumericClass operator -(NumericClass left, NumericClass right) => new() { Value = left.Value - right.Value };
        public static NumericClass operator *(NumericClass left, int multiple) => new() { Value = left.Value * multiple };
        public static NumericClass operator /(NumericClass left, int divisor) => new() { Value = left.Value / divisor };
        public static NumericClass operator *(NumericClass left, long multiple) => new() { Value = checked((int)(left.Value * multiple)) };
        public static NumericClass operator /(NumericClass left, long divisor) => new() { Value = checked((int)(left.Value / divisor)) };
    }

    [Fact]
    public void StructAverageTest()
    {
        Assert.Equal(new NumericStruct { Value = 3 }, new[]
        {
            new NumericStruct { Value = 2 },
            new NumericStruct { Value = 4 },
        }.Average());
        Assert.ThrowsAny<InvalidOperationException>(() => new NumericStruct[0].Average());

        Assert.Equal(2, new NumericStruct?[]
        {
            new NumericStruct { Value = 2 },
            null,
        }.Average()?.Value);
        Assert.Null(new NumericStruct?[] { null, null }.Average());
        Assert.Null(new NumericStruct?[0].Average());
    }

    [Fact]
    public void StructSumTest()
    {
        Assert.Equal(new NumericStruct { Value = 6 }, new[]
        {
            new NumericStruct { Value = 2 },
            new NumericStruct { Value = 4 },
        }.Sum());
        Assert.Equal(default(NumericStruct), new NumericStruct[0].Sum());

        Assert.Equal(2, new NumericStruct?[]
        {
            new NumericStruct { Value = 2 },
            null,
        }.Sum()?.Value);
        Assert.Null(new NumericStruct?[] { null, null }.Sum());
        Assert.Null(new NumericStruct?[0].Sum());
    }

    [Fact]
    public void ClassAverageTest()
    {
        Assert.Equal(3, new[]
        {
            new NumericClass { Value = 2 },
            new NumericClass { Value = 4 },
        }.Average()?.Value);

        Assert.Equal(2, new NumericClass[]
        {
            new NumericClass { Value = 2 },
            null,
        }.Average()?.Value);
        Assert.Null(new NumericClass[] { null, null }.Average());
        Assert.Null(new NumericClass[0].Sum());
    }

    [Fact]
    public void ClassSumTest()
    {
        Assert.Equal(6, new[]
        {
            new NumericClass { Value = 2 },
            new NumericClass { Value = 4 },
        }.Sum()?.Value);

        Assert.Equal(2, new NumericClass[]
        {
            new NumericClass { Value = 2 },
            null,
        }.Sum()?.Value);
        Assert.Null(new NumericClass[] { null, null }.Sum());
        Assert.Null(new NumericClass[0].Sum());
    }

}
