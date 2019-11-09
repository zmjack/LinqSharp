using Dawnx.Ranges;
using System.Linq;
using Xunit;

namespace NLinq.Test
{
    public class XIEnumerableTests
    {
        [Fact]
        public void PageTest()
        {
            var items = IntegerRange.Create(10);
            Assert.Equal(4, items.SelectPage(2, 3).PageCount);
            Assert.Equal(2, items.SelectPage(2, 5).PageCount);
            Assert.True(items.SelectPage(1, 3).IsFristPage);
            Assert.True(items.SelectPage(4, 3).IsLastPage);
            Assert.Equal(IntegerRange.Create(5), items.SelectPage(1, 5).ToArray());
            Assert.Equal(IntegerRange.Create(5, 10), items.SelectPage(2, 5).ToArray());
        }

    }
}
