using NStandard;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class TierTests
    {
        [Fact]
        public void Test1()
        {
            static string Repeat(string s, int times)
            {
                return StringExtensions.Repeat(s, times);
            }

            var arr = new int[10].Let(i => i);
            var tiers = arr.TierBy(x => x / 5, x => x % 2);

            var sb = new StringBuilder();

            foreach (var tier0 in tiers)
            {
                foreach (var tier1 in tier0.SubTiers)
                {
                    foreach (var number in tier1)
                    {
                        sb.AppendLine($"    ({tier0.Key},{tier1.Key}) = {number}");
                    }
                    sb.AppendLine($"{Repeat("    ", 2 - tier1.Span)}[Sum] = {tier1.Sum()}");
                }
                sb.AppendLine($"{Repeat("    ", 2 - tier0.Span)}[Sum] = {tier0.Sum()}");
            }

            Assert.Equal("""
    (0,0) = 0
    (0,0) = 2
    (0,0) = 4
    [Sum] = 6
    (0,1) = 1
    (0,1) = 3
    [Sum] = 4
[Sum] = 10
    (1,1) = 5
    (1,1) = 7
    (1,1) = 9
    [Sum] = 21
    (1,0) = 6
    (1,0) = 8
    [Sum] = 14
[Sum] = 35

""", sb.ToString());
        }
    }
}
