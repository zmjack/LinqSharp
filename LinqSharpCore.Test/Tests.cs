using LinqSharp.Data.Test;
using System.Linq;
using Xunit;

namespace LinqSharp.Test
{
    public class Tests
    {
        [Fact]
        public void Test1()
        {
            using (var mysql = ApplicationDbContext.UseDefault())
            {
                var emplyeeList = new[]
                {
                    new
                    {
                        FirstName = "Robert",
                        LastName = "King",
                    },
                    new
                    {
                        FirstName = "Michael",
                        LastName = "Suyama",
                    },
                };
                var a = mysql.Employees.Where(x => emplyeeList.Contains(new { x.FirstName, x.LastName })).ToSql();
            }
        }
    }
}
