using System;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class SameOrDefaultTests
    {
        [Fact]
        public void SameOrDefaultTest()
        {
            Assert.Null(new int?[] { null, 1, 1 }.SameOrDefault());
            Assert.Null(new int?[] { 0, 1, 1 }.SameOrDefault());
            Assert.Equal(1, new int?[] { 1, 1, 1 }.SameOrDefault());
        }

        [Fact]
        public void SameTest()
        {
            Assert.ThrowsAny<InvalidOperationException>(() => new int?[] { null, 1, 1 }.Same());
            Assert.ThrowsAny<InvalidOperationException>(() => new int?[] { 0, 1, 1 }.Same());
            Assert.Equal(1, new int?[] { 1, 1, 1 }.Same());
        }

    }
}
