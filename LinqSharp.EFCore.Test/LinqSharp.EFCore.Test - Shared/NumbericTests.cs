using System;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class NumbericTests
    {
        private struct NumbericStruct
        {
            public int Value { get; set; }
            public static NumbericStruct operator +(NumbericStruct left, NumbericStruct right) => new() { Value = left.Value + right.Value };
            public static NumbericStruct operator -(NumbericStruct left, NumbericStruct right) => new() { Value = left.Value - right.Value };
            public static NumbericStruct operator *(NumbericStruct left, int multiple) => new() { Value = left.Value * multiple };
            public static NumbericStruct operator /(NumbericStruct left, int divisor) => new() { Value = left.Value / divisor };
        }

        private class NumbericClass
        {
            public int Value { get; set; }
            public static NumbericClass operator +(NumbericClass left, NumbericClass right) => new() { Value = left.Value + right.Value };
            public static NumbericClass operator -(NumbericClass left, NumbericClass right) => new() { Value = left.Value - right.Value };
            public static NumbericClass operator *(NumbericClass left, int multiple) => new() { Value = left.Value * multiple };
            public static NumbericClass operator /(NumbericClass left, int divisor) => new() { Value = left.Value / divisor };
        }

        [Fact]
        public void StrcutAverageTest()
        {
            Assert.Equal(new NumbericStruct { Value = 3 }, new[]
            {
                new NumbericStruct { Value = 2 },
                new NumbericStruct { Value = 4 },
            }.Average());
            Assert.ThrowsAny<InvalidOperationException>(() => new NumbericStruct[0].Average());

            Assert.Equal(new NumbericStruct { Value = 2 }, new NumbericStruct?[]
            {
                new NumbericStruct { Value = 2 },
                null,
            }.Average());
            Assert.Null(new NumbericStruct?[] { null, null }.Average());
            Assert.Null(new NumbericStruct?[0].Average());
        }

        [Fact]
        public void StrcutSumTest()
        {
            Assert.Equal(new NumbericStruct { Value = 6 }, new[]
            {
                new NumbericStruct { Value = 2 },
                new NumbericStruct { Value = 4 },
            }.Sum());
            Assert.ThrowsAny<InvalidOperationException>(() => new NumbericStruct[0].Sum());

            Assert.Null(new NumbericStruct?[]
            {
                new NumbericStruct { Value = 2 },
                null,
            }.Sum());
            Assert.Null(new NumbericStruct?[] { null, null }.Sum());
            Assert.Null(new NumbericStruct?[0].Sum());
        }

        [Fact]
        public void ClassAverageTest()
        {
            Assert.Equal(3, new[]
            {
                new NumbericClass { Value = 2 },
                new NumbericClass { Value = 4 },
            }.Average()?.Value);

            Assert.Equal(2, new NumbericClass[]
            {
                new NumbericClass { Value = 2 },
                null,
            }.Average()?.Value);

            Assert.Null(new NumbericClass[] { null, null }.Average());
            Assert.Null(new NumbericClass[0].Sum());
        }

        [Fact]
        public void ClassSumTest()
        {
            Assert.Equal(6, new[]
            {
                new NumbericClass { Value = 2 },
                new NumbericClass { Value = 4 },
            }.Sum()?.Value);

            Assert.Null(new NumbericClass[]
            {
                new NumbericClass { Value = 2 },
                null,
            }.Sum());

            Assert.Null(new NumbericClass[] { null, null }.Sum());
            Assert.Null(new NumbericClass[0].Sum());
        }

    }
}
