using Xunit;

namespace LinqSharp.Test
{
    public class PagedEnumerableTests
    {
        [Fact]
        public void Test1()
        {
            var items = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            {
                var page = items.SelectPage(1, 3);
                Assert.True(page.IsFristPage);
                Assert.False(page.IsLastPage);
                Assert.Equal(4, page.PageCount);
                Assert.Equal(10, page.SourceCount);
            }
            {
                var page = items.SelectPage(1, 5);
                Assert.True(page.IsFristPage);
                Assert.False(page.IsLastPage);
                Assert.Equal(2, page.PageCount);
                Assert.Equal(10, page.SourceCount);
            }
            {
                var page = items.SelectPage(1, 10);
                Assert.True(page.IsFristPage);
                Assert.True(page.IsLastPage);
                Assert.Equal(1, page.PageCount);
                Assert.Equal(10, page.SourceCount);
            }

        }
    }
}
