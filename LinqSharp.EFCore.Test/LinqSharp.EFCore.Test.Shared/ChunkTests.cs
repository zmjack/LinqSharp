using LinqSharp.EFCore.Data.Test;
using NStandard;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class ChunkTests
    {
        [Fact]
        public void ChunkTest()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            var regions = mysql.Regions.ToArray();

            {
                var chunks = (
                    from chunk in regions.Chunk(2)
                    select chunk.Select(x => x.RegionDescription).ToArray()
                ).ToArray();

                Assert.Equal(new[]
                {
                    new [] { "Eastern", "Western" },
                    new [] { "Northern", "Southern" },
                }, chunks);
            }

            {
                var chunks = (
                    from chunk in regions.Chunk(3)
                    select chunk.Select(x => x.RegionDescription).ToArray()
                ).ToArray();

                Assert.Equal(new[]
                {
                    new [] { "Eastern", "Western", "Northern" },
                    new [] { "Southern" },
                }, chunks);
            }
        }

        [Fact]
        public void GroupByCountTest()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            var regions = mysql.Regions.ToArray();

            {
                var chunks = (
                    from chunk in regions.GroupByCount(2)
                    select chunk.Select(x => x.RegionDescription).ToArray()
                ).ToArray();

                Assert.Equal(new[]
                {
                    new [] { "Eastern", "Western" },
                    new [] { "Northern", "Southern" },
                }, chunks);
            }

            {
                var chunks = (
                    from chunk in regions.GroupByCount(3)
                    select chunk.Select(x => x.RegionDescription).ToArray()
                ).ToArray();

                Assert.Equal(new[]
                {
                    new [] { "Eastern", "Western", "Northern" },
                    new [] { "Southern" },
                }, chunks);
            }

            {
                var chunks = (
                    from chunk in regions.GroupByCount(3, PadDirection.Left)
                    select chunk.Select(x => x.RegionDescription).ToArray()
                ).ToArray();

                Assert.Equal(new[]
                {
                    new [] { "Eastern" },
                    new [] { "Western", "Northern", "Southern" },
                }, chunks);
            }
        }

    }
}
