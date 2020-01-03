using Xunit;

namespace NLinq.Test
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
