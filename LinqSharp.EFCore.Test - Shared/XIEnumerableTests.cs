using LinqSharp.EFCore.Models.Test;
using NStandard;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
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

        [Fact]
        public void PadTest()
        {
            var items = new[]
            {
                new NameModel { Name = "A", NickName = "a", Tag = "01" },
                new NameModel { Name = "B", NickName = "b", Tag = "01" },
            };

            var filled1 = items.Pad(x => x.Name, new[] { "A", "B", "C" }, key => new NameModel { Name = key, NickName = ":c" });
            Assert.Equal(new[] { "A", "B", "C" }, filled1.Select(x => x.Name).ToArray());
            Assert.Equal(new[] { "a", "b", ":c" }, filled1.Select(x => x.NickName).ToArray());

            var filled2 = items.Pad(x => new { x.Name, x.Tag }, new[]
            {
                new { Name = "A", Tag = "01" },
                new { Name = "B", Tag = "01" },
                new { Name = "B", Tag = "02" },
            }, key => new NameModel { Name = key.Name, NickName = ":c", Tag = key.Tag });
            Assert.Equal(new[] { "A", "B", "B" }, filled2.Select(x => x.Name).ToArray());
            Assert.Equal(new[] { "a", "b", ":c" }, filled2.Select(x => x.NickName).ToArray());
        }

    }
}
