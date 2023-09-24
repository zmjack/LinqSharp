using NStandard;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class LayerTests
    {
        [Fact]
        public void Test1()
        {
            static string Repeat(string s, int times)
            {
                return StringExtensions.Repeat(s, times);
            }

            var arr = new int[10].Let(i => i);
            var sub0 = arr.LayerBy(x => x / 5, x => x % 2);

            var sb = new StringBuilder();

            foreach (var sub1 in sub0.SubLayers)
            {
                foreach (var sub2 in sub1.SubLayers)
                {
                    foreach (var number in sub2)
                    {
                        sb.AppendLine($"        ({sub1.Key},{sub2.Key}) = {number}");
                    }
                    sb.AppendLine($"{Repeat("    ", 3 - sub2.Span)}[Sum] = {sub2.Sum()}");
                }
                sb.AppendLine($"{Repeat("    ", 3 - sub1.Span)}[Sum] = {sub1.Sum()}");
            }
            sb.AppendLine($"{Repeat("    ", 3 - sub0.Span)}[Sum] = {sub0.Sum()}");

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
[Sum] = 45

""", sb.ToString());
        }
    }
}
