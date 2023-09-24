using LinqSharp.Index;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class IndexingTests
    {
        [Fact]
        public void NormalTest()
        {
            var indexing = new Indexing<int, int>(new[] { 1, 2, 2, 3 }, x => x);
            Assert.Equal(0, indexing[0].Count);
            Assert.Equal(1, indexing[1].Count);
            Assert.Equal(2, indexing[2].Count);
        }

        [Fact]
        public void EmptyTest()
        {
            var indexing = new Indexing<int, int>(new[] { 1, 2, 3 }, x => x);
            indexing.Remove(2);
            Assert.Empty(indexing[2]);
        }
    }
}
