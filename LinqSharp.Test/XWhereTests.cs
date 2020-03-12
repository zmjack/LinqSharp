using Northwnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqSharp.Test
{
    public class XWhereTests
    {
        [Fact]
        public void Test1()
        {
            using (var sqlite = NorthwndContext.UseSqliteResource())
            {
                var query1 = sqlite.Employees.XWhere(h =>
                {
                    var searches = new[] { ("Mr.", 1955), ("Ms.", 1963) };
                    return h.Or(searches, s => x => x.TitleOfCourtesy == s.Item1 && x.BirthDate.Value.Year == s.Item2);
                });

                var query2 = sqlite.Employees.XWhere(h =>
                {
                    return h.Where(x => x.TitleOfCourtesy == "Mr." && x.BirthDate.Value.Year == 1955)
                         | h.Where(x => x.TitleOfCourtesy == "Ms." && x.BirthDate.Value.Year == 1963);
                });

                var query3 = sqlite.Employees.XWhere(h =>
                {
                    return h.Or(
                        h.Where(x => x.TitleOfCourtesy == "Mr." && x.BirthDate.Value.Year == 1955),
                        h.Where(x => x.TitleOfCourtesy == "Ms." && x.BirthDate.Value.Year == 1963));
                });

                Assert.Equal(2, query1.Count());
                Assert.Equal(2, query2.Count());
                Assert.Equal(2, query3.Count());
            }
        }

        [Fact]
        public void Test2()
        {
            var searches = new[]
            {
                new { prop = nameof(Category.CategoryName), method = "contains", value = "Con" },
                new { prop = nameof(Category.Description), method = "equals", value = "Cheeses" },
                new { prop = nameof(Category.Description), method = "contains", value = "fish" },
            };
            var ops = new[] { "|", "&" };

            using (var sqlite = NorthwndContext.UseSqliteResource())
            {
                sqlite.Categories.XWhere(h =>
                {
                    return h.Where(x => x.CategoryName.Contains("Con")) | h.Where(x => x.Description == "Cheeses") & h.Where(x => x.Description.Contains("fish"));
                });

                sqlite.Categories.XWhere(h =>
                {
                    return
                        h.Or(
                            h.Where(x => x.CategoryName.Contains("Con")),
                            h.And(
                                h.Where(x => x.Description == "Cheeses"),
                                h.Where(x => x.Description.Contains("fish"))));
                });

                sqlite.Categories.XWhere(h =>
                {
                    return
                        h.Or(
                            h.WhereDynamic(b => b.Property("CategoryName").Invoke(MethodUnit.StringContains, "Con")),
                            h.And(
                                h.WhereDynamic(b => b.Property("Description").Invoke(MethodUnit.StringEquals, "Cheeses")),
                                h.WhereDynamic(b => b.Property("Description").Invoke(MethodUnit.StringContains, "fish"))));
                });
            }
        }

    }
}
