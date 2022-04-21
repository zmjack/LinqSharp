using NStandard;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class JoinTests
    {
        private readonly int[] leftNumbers = new[] { 1, 2 };
        private readonly int[] rightNumbers = new[] { 2, 3 };

        private readonly Ref<int>[] leftRefNumbers = new[] { Ref.Clone(1), Ref.Clone(2) };
        private readonly Ref<int>[] rightRefNumbers = new[] { Ref.Clone(2), Ref.Clone(3) };

        [Fact]
        public void FullJoinTest()
        {
            var result = leftNumbers.FullJoin(rightNumbers, x => x, x => x, (l, r) => new { Left = l, Right = r });
            Assert.Equal(new[]
            {
                new { Left = 1, Right = 0 },
                new { Left = 2, Right = 2 },
                new { Left = 0, Right = 3 },
            }, result);

            var refResult = leftRefNumbers.FullJoin(rightRefNumbers, x => x, x => x, (l, r) => new { Left = l?.Struct, Right = r?.Struct });
            Assert.Equal(new[]
            {
                new { Left = (int?)1, Right = (int?)null },
                new { Left = (int?)2, Right = (int?)2 },
                new { Left = (int?)null, Right = (int?)3 },
            }, refResult);
        }

        [Fact]
        public void LeftJoinTest()
        {
            var result = leftNumbers.LeftJoin(rightNumbers, x => x, x => x, (l, r) => new { Left = l, Right = r });
            Assert.Equal(new[]
            {
                new { Left = 1, Right = 0 },
                new { Left = 2, Right = 2 },
            }, result);

            var refResult = leftRefNumbers.LeftJoin(rightRefNumbers, x => x, x => x, (l, r) => new { Left = l?.Struct, Right = r?.Struct });
            Assert.Equal(new[]
            {
                new { Left = (int?)1, Right = (int?)null },
                new { Left = (int?)2, Right = (int?)2 },
            }, refResult);
        }

        [Fact]
        public void RightJoinTest()
        {
            var result = leftNumbers.RightJoin(rightNumbers, x => x, x => x, (l, r) => new { Left = l, Right = r });
            Assert.Equal(new[]
            {
                new { Left = 2, Right = 2 },
                new { Left = 0, Right = 3 },
            }, result);

            var refResult = leftRefNumbers.RightJoin(rightRefNumbers, x => x, x => x, (l, r) => new { Left = l?.Struct, Right = r?.Struct });
            Assert.Equal(new[]
            {
                new { Left = (int?)2, Right = (int?)2 },
                new { Left = (int?)null, Right = (int?)3 },
            }, refResult);
        }
    }
}
