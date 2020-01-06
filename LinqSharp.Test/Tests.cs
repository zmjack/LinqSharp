using Xunit;

namespace LinqSharp.Test
{
    public class Tests
    {
        [Fact]
        public void Test1()
        {
            using (var mysql = new ApplicationDbContext())
            {
            }
        }
    }
}
