using LinqSharp.Data.Test;
using Northwnd;
using NStandard;
using System.Linq;
using Xunit;

namespace LinqSharp.Test
{
    public class GroupByCountTests
    {
        [Fact]
        public void Test1()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            var regions = mysql.Regions.ToArray();

            regions.GroupByCount(2).ToArray().Then(groups =>
            {
                Assert.Equal(2, groups.Count());
                Assert.Equal(2, groups[0].Count());
                Assert.Equal(2, groups[1].Count());
            });

            regions.GroupByCount(3).ToArray().Then(groups =>
            {
                Assert.Equal(2, groups.Count());
                Assert.Equal(3, groups[0].Count());
                Assert.Single(groups[1]);
            });

            regions.GroupByCount(3, PadDirection.Left).ToArray().Then(groups =>
            {
                Assert.Equal(2, groups.Count());
                Assert.Single(groups[0]);
                Assert.Equal(3, groups[1].Count());
            });
        }

    }
}
