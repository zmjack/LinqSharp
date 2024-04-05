using LinqSharp.EFCore.Data.Test;
using NStandard;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test;

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

            Assert.Equal(
            [
                ["Eastern", "Western"],
                ["Northern", "Southern"],
            ], chunks);
        }

        {
            var chunks = (
                from chunk in regions.Chunk(3)
                select chunk.Select(x => x.RegionDescription).ToArray()
            ).ToArray();

            Assert.Equal(
            [
                ["Eastern", "Western", "Northern"],
                ["Southern"],
            ], chunks);
        }
    }

    [Fact]
    public void GroupByCountTest()
    {
        using var mysql = ApplicationDbContext.UseMySql();
        var regions = mysql.Regions.ToArray();

        {
#pragma warning disable CS0618 // Type or member is obsolete
            var chunks = (
                from chunk in regions.Chunk(2)
                select chunk.Select(x => x.RegionDescription).ToArray()
            ).ToArray();
#pragma warning restore CS0618 // Type or member is obsolete

            Assert.Equal(
            [
                ["Eastern", "Western"],
                ["Northern", "Southern"],
            ], chunks);
        }

        {
#pragma warning disable CS0618 // Type or member is obsolete
            var chunks = (
                from chunk in regions.Chunk(3)
                select chunk.Select(x => x.RegionDescription).ToArray()
            ).ToArray();
#pragma warning restore CS0618 // Type or member is obsolete

            Assert.Equal(
            [
                ["Eastern", "Western", "Northern"],
                ["Southern"],
            ], chunks);
        }

        {
            var chunks = (
                from chunk in regions.GroupByCount(3, PadDirection.Left)
                select chunk.Select(x => x.RegionDescription).ToArray()
            ).ToArray();

            Assert.Equal(
            [
                ["Eastern"],
                ["Western", "Northern", "Southern"],
            ], chunks);
        }
    }

}
