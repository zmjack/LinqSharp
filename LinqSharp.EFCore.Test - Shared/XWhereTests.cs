using LinqSharp.EFCore.Data.Test;
using Northwnd;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class XWhereTests
    {
        [Fact]
        public void Test0()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                string[] values = { "T", "F" };

                var query1 = mysql.Products.XWhere(h => h.Or(values.Select(v => h.Where(x => x.ProductName.Contains(v)))));
                var sql1 = query1.ToSql();

                var query2 = mysql.Products.XWhere(h => h.Or(values, v => x => x.ProductName.Contains(v)));
                var sql2 = query2.ToSql();

                Assert.Equal(sql1, sql2);
            }
        }

        [Fact]
        public void Test00()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query = mysql.Products.Where(x => x.CategoryID == 123);
                var query1 = mysql.Products.XWhere(h => h.Property("CategoryID") == 123);
                var query2 = mysql.Products.XWhere(h => h.Property(x => x.CategoryID) == 123);
                var sql1 = query1.ToSql();
            }
        }

        [Fact]
        public void Test1()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var searches = new[] { ("Mr.", 1955), ("Ms.", 1963) };

                var query1 = mysql.Employees.XWhere(h =>
                {
                    return h.Or(searches, s => x => x.TitleOfCourtesy == s.Item1 && x.BirthDate.Value.Year == s.Item2);
                });

                var query2 = mysql.Employees.XWhere(h =>
                {
                    return h.Where(x => x.TitleOfCourtesy == "Mr." && x.BirthDate.Value.Year == 1955)
                         | h.Where(x => x.TitleOfCourtesy == "Ms." && x.BirthDate.Value.Year == 1963);
                });

                var query3 = mysql.Employees.XWhere(h =>
                {
                    return
                        h.Or(
                            h.Where(x => x.TitleOfCourtesy == "Mr." && x.BirthDate.Value.Year == 1955),
                            h.Where(x => x.TitleOfCourtesy == "Ms." && x.BirthDate.Value.Year == 1963));
                });

                var query4 = mysql.Employees.XWhere(h =>
                {
                    var parts = searches
                        .Select(s => h.Where(x => x.TitleOfCourtesy == s.Item1 && x.BirthDate.Value.Year == s.Item2))
                        .ToArray();
                    return h.Or(parts);
                });

                Assert.Equal(new[] { 3, 5 }, query1.Select(x => x.EmployeeID));
                Assert.Equal(new[] { 3, 5 }, query2.Select(x => x.EmployeeID));
                Assert.Equal(new[] { 3, 5 }, query3.Select(x => x.EmployeeID));
            }
        }

        private class SearchModel
        {
            public string Link;
            public string PropName;
            public string Method;
            public string Value;
        }

        [Fact]
        public void Test2_1()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query = mysql.Categories.XWhere(h =>
                {
                    return h.Where(x => x.CategoryName.Contains("Con")) | h.Where(x => x.Description == "Cheeses") & h.Where(x => x.Description.Contains("fish"));
                });

                Assert.Equal(new[] { 2, 3 }, query.Select(x => x.CategoryID));
            }
        }

        [Fact]
        public void Test2_2()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query = mysql.Categories.XWhere(h =>
                {
                    return
                        h.Or(
                            h.Where(x => x.CategoryName.Contains("Con")),
                            h.And(
                                h.Where(x => x.Description == "Cheeses"),
                                h.Where(x => x.Description.Contains("fish"))));
                });

                Assert.Equal(new[] { 2, 3 }, query.Select(x => x.CategoryID));
            }
        }

        [Fact]
        public void Test2_3()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var searches = new[]
                {
                    new SearchModel { Link = "", PropName = nameof(Category.CategoryName), Method = "contains", Value = "Con" },
                    new SearchModel { Link = "|", PropName = nameof(Category.Description), Method = "equals", Value = "Cheeses" },
                    new SearchModel { Link = "&", PropName = nameof(Category.Description), Method = "contains", Value = "fish" },
                };
                var operators = searches.Select(x => x.Link).Skip(1);

                // No priority
                var query = mysql.Categories.XWhere(h =>
                {
                    WhereExp<Category> exp = null;

                    foreach (var search in searches)
                    {
                        var _exp = search.Method switch
                        {
                            "contains" => h.Property<string>(search.PropName).Contains(search.Value),
                            "equals" => h.Property<string>(search.PropName) == search.Value,
                            _ => throw new NotSupportedException(),
                        };

                        switch (search.Link)
                        {
                            case "": exp = _exp; break;
                            case "|": exp |= _exp; break;
                            case "&": exp &= _exp; break;
                        }
                    }

                    return exp;
                });
                var sql = query.ToSql();

                Assert.Equal(new int[0], query.Select(x => x.CategoryID));
            }
        }

        private class SearchEvaluator<TSource> : Evaluator<WhereExp<TSource>, string>
        {
            public WhereHelper<TSource> WhereHelper;

            public SearchEvaluator(WhereHelper<TSource> helper)
            {
                WhereHelper = helper;
            }

            protected override Dictionary<string, BinaryOpFunc<WhereExp<TSource>>> OpFunctions { get; } = new Dictionary<string, BinaryOpFunc<WhereExp<TSource>>>
            {
                ["|"] = (left, right) => left | right,
                ["&"] = (left, right) => left & right,
            };

            protected override Dictionary<string, int> OpLevels { get; } = new Dictionary<string, int>
            {
                ["&"] = 1,
                ["|"] = 2,
            };
        }

        [Fact]
        public void Test2_4()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var searches = new[]
                {
                    new SearchModel { Link = "", PropName = nameof(Category.CategoryName), Method = "contains", Value = "Con" },
                    new SearchModel { Link = "|", PropName = nameof(Category.Description), Method = "equals", Value = "Cheeses" },
                    new SearchModel { Link = "&", PropName = nameof(Category.Description), Method = "contains", Value = "fish" },
                };
                var operators = searches.Select(x => x.Link).Skip(1);

                var query = mysql.Categories.XWhere(h =>
                {
                    var operands = searches.Select(search =>
                    {
                        return search.Method switch
                        {
                            "contains" => h.Property<string>(search.PropName).Contains(search.Value),
                            "equals" => h.Property<string>(search.PropName) == search.Value,
                            _ => throw new NotSupportedException(),
                        };
                    });

                    return new SearchEvaluator<Category>(h).Eval(operands, operators);
                });

                Assert.Equal(new[] { 2, 3 }, query.Select(x => x.CategoryID));
            }
        }

        [Fact]
        public void NullTest()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var expected = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                var actual = mysql.Categories.XWhere(h => null).Select(x => x.CategoryID).ToArray();
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void PropertyTest1()
        {
            using var mysql = ApplicationDbContext.UseMySql();

            var query = mysql.Categories.XWhere(h =>
            {
                return (h.Property<string>("CategoryName") + "a").Contains("Con");
            });
            var sql = query.ToSql();
        }

        [Fact]
        public void PropertyTest2()
        {
            using var mysql = ApplicationDbContext.UseMySql();

            var query1 = mysql.Categories.XWhere(h =>
            {
                return h.Property<string>("CategoryName") + "a" == h.Property<string>("Description");
            });
            var query2 = mysql.Categories.Where(x => x.CategoryName + "a" == x.Description);
            var sql1 = query1.ToSql();
            var sql2 = query2.ToSql();
        }

    }
}
