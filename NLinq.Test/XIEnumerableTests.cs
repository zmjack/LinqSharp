using NStandard;
using System.Linq;
using Xunit;

namespace NLinq.Test
{
    public class XIEnumerableTests
    {
        [Fact]
        public void PageTest()
        {
            var items = new int[10].Let(i => i);

            Assert.Equal(4, items.SelectPage(2, 3).PageCount);
            Assert.Equal(2, items.SelectPage(2, 5).PageCount);
            Assert.True(items.SelectPage(1, 3).IsFristPage);
            Assert.True(items.SelectPage(4, 3).IsLastPage);
            Assert.Equal(new[] { 0, 1, 2, 3, 4 }, items.SelectPage(1, 5).ToArray());
            Assert.Equal(new[] { 5, 6, 7, 8, 9 }, items.SelectPage(2, 5).ToArray());
        }

    }
}
