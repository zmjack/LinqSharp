using LinqSharp.EFCore.Data.Test;
using Northwnd;
using NStandard;
using System;
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
            public string PropName;
            public string Method;
            public string Value;
        }

        private class SearchEvaluator<TSource> : Evaluator<string, WhereExp<TSource>>
        {
            public WhereHelper<TSource> WhereHelper;

            public SearchEvaluator(WhereHelper<TSource> helper)
            {
                WhereHelper = helper;
            }

            public override Func<WhereExp<TSource>, WhereExp<TSource>, WhereExp<TSource>> GetOpFunction(string op)
            {
                return op switch
                {
                    "|" => (left, right) => left | right,
                    "&" => (left, right) => left & right,
                    _ => throw new NotImplementedException(),
                };
            }

            public override int GetOpLevel(string op)
            {
                return op switch
                {
                    "&" => 1,
                    "|" => 2,
                    _ => throw new NotImplementedException(),
                };
            }
        }

        [Fact]
        public void Test2()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query1 = mysql.Categories.XWhere(h =>
                {
                    return h.Where(x => x.CategoryName.Contains("Con")) | h.Where(x => x.Description == "Cheeses") & h.Where(x => x.Description.Contains("fish"));
                });

                var query2 = mysql.Categories.XWhere(h =>
                {
                    return
                        h.Or(
                            h.Where(x => x.CategoryName.Contains("Con")),
                            h.And(
                                h.Where(x => x.Description == "Cheeses"),
                                h.Where(x => x.Description.Contains("fish"))));
                });

                var searches = new[]
                {
                    new SearchModel { PropName = nameof(Category.CategoryName), Method = "contains", Value = "Con" },
                    new SearchModel { PropName = nameof(Category.Description), Method = "equals", Value = "Cheeses" },
                    new SearchModel { PropName = nameof(Category.Description), Method = "contains", Value = "fish" },
                };
                var operators = new[] { "|", "&" };
                var query3 = mysql.Categories.XWhere(h =>
                {
                    var operands = searches.Select(x =>
                    {
                        var method = x.Method switch
                        {
                            "contains" => MethodUnit.StringContains,
                            "equals" => MethodUnit.StringEquals,
                            _ => throw new NotSupportedException(),
                        };
                        return h.WhereDynamic(b => b.Property(x.PropName).Invoke(method, x.Value));
                    });
                    return new SearchEvaluator<Category>(h).Eval(operators, operands);
                });

                Assert.Equal(new[] { 2, 3 }, query1.Select(x => x.CategoryID));
                Assert.Equal(new[] { 2, 3 }, query2.Select(x => x.CategoryID));
                Assert.Equal(new[] { 2, 3 }, query3.Select(x => x.CategoryID));
            }
        }

    }
}
