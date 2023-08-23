using LinqSharp.EFCore.Data;
using LinqSharp.EFCore.Data.Test;
using LinqSharp.Query;
using Microsoft.EntityFrameworkCore;
using Northwnd;
using NStandard;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class FilterTests
    {
        public class DateTimeQueryFilter : IFieldQueryFilter<DateTime?>
        {
            public DateTime Start { get; set; }
            public DateTime End { get; set; }

            Expression<Func<DateTime?, bool>> IFieldQueryFilter<DateTime?>.Predicate => x => Start <= x && x <= End;
        }

        public class DateTimeFilter : IFieldFilter<DateTime?>
        {
            public DateTime Start { get; set; }
            public DateTime End { get; set; }

            public QueryExpression<DateTime?> Filter(QueryHelper<DateTime?> h)
            {
                return h.Where(x => Start <= x) & h.Where(x => x <= End);
            }
        }

        public class EntityFilter : IQueryFilter<Employee>, IQueryFilter<Product>
        {
            public int[] Ids { get; set; }

            public IQueryable<Employee> Apply(IQueryable<Employee> source) => source.Where(x => Ids.Contains(x.EmployeeID));
            public IQueryable<Product> Apply(IQueryable<Product> source) => source.Where(x => Ids.Contains(x.ProductID));
        }

        public class IdsFilter : IFieldFilter<int>
        {
            public int[] Ids { get; set; }

            public QueryExpression<int> Filter(QueryHelper<int> h)
            {
                return h.Where(x => Ids.Contains(x));
            }
        }

        [Fact]
        public void QueryFilterTest()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            var filter = new EntityFilter { Ids = new[] { 1 } };

            var productIds = mysql.Products.Filter(filter).Select(x => x.ProductID).ToArray();
            var employeeIds = mysql.Employees.Filter(filter).Select(x => x.EmployeeID).ToArray();

            Assert.Equal(new[] { 1 }, productIds);
            Assert.Equal(new[] { 1 }, employeeIds);
        }

        [Fact]
        public void FilterFilterByTest()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            var filter = new IdsFilter { Ids = new[] { 1 } };

            var productIds = mysql.Products.Filter(h =>
            {
                return h.FilterBy(x => x.ProductID, filter);
            }).Select(x => x.ProductID).ToArray();
            var employeeIds = mysql.Employees.Filter(h =>
            {
                return h.FilterBy(x => x.EmployeeID, filter);
            }).Select(x => x.EmployeeID).ToArray();

            Assert.Equal(new[] { 1 }, productIds);
            Assert.Equal(new[] { 1 }, employeeIds);
        }

        [Fact]
        public void InTest1()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            var query = mysql.Employees.Filter(h =>
            {
                return h.Property("EmployeeID").In(new object[] { 1, 2 });
            });

            var actual = query.Select(x => x.EmployeeID).ToArray();
            Assert.Equal(new[] { 1, 2 }, actual);
        }

        [Fact]
        public void InTest2()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            var query = mysql.Employees.Filter(h =>
            {
                return h.Property("EmployeeID").In(new int[] { 1, 2 });
            });

            var actual = query.Select(x => x.EmployeeID).ToArray();
            Assert.Equal(new[] { 1, 2 }, actual);
        }

        [Fact]
        public void Test0()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            string[] values = { "T", "F" };

            var query1 = mysql.Products.Filter(h => h.Or(values.Select(v => h.Where(x => x.ProductName.Contains(v)))));
            var sql1 = query1.ToQueryString();

            var query2 = mysql.Products.Filter(h => h.Or(values, v => x => x.ProductName.Contains(v)));
            var sql2 = query2.ToQueryString();

            Assert.Equal(sql1, sql2);
        }

        [Fact]
        public void Test00()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            var query = mysql.Products.Where(x => x.CategoryID == 123).ToQueryString();
            var query1 = mysql.Products.Filter(h => h.Property("CategoryID") == 123).ToQueryString();
            var query2 = mysql.Products.Filter(h => h.Property(x => x.CategoryID) == 123).ToQueryString();
            var query3 = mysql.Products.Filter(h => h.Property(x => x.CategoryID) == null).ToQueryString();
        }

        [Fact]
        public void Test1()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            var searches = new[] { ("Mr.", 1955), ("Ms.", 1963) };

            var query1 = mysql.Employees.Filter(h =>
            {
                return h.Or(searches, s => x => x.TitleOfCourtesy == s.Item1 && x.BirthDate.Value.Year == s.Item2);
            });

            var query2 = mysql.Employees.Filter(h =>
            {
                return h.Where(x => x.TitleOfCourtesy == "Mr." && x.BirthDate.Value.Year == 1955)
                     | h.Where(x => x.TitleOfCourtesy == "Ms." && x.BirthDate.Value.Year == 1963);
            });

            var query3 = mysql.Employees.Filter(h =>
            {
                return
                    h.Or(
                        h.Where(x => x.TitleOfCourtesy == "Mr." && x.BirthDate.Value.Year == 1955),
                        h.Where(x => x.TitleOfCourtesy == "Ms." && x.BirthDate.Value.Year == 1963));
            });

            var query4 = mysql.Employees.Filter(h =>
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
            using var mysql = ApplicationDbContext.UseMySql();
            var query = mysql.Categories.Filter(h =>
            {
                return h.Where(x => x.CategoryName.Contains("Con")) | h.Where(x => x.Description == "Cheeses") & h.Where(x => x.Description.Contains("fish"));
            });

            Assert.Equal(new[] { 2, 3 }, query.Select(x => x.CategoryID));
        }

        [Fact]
        public void Test2_2()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            var query = mysql.Categories.Filter(h =>
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

        [Fact]
        public void Test2_3()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            var searches = new[]
            {
                new SearchModel { Link = "", PropName = nameof(Category.CategoryName), Method = "contains", Value = "Con" },
                new SearchModel { Link = "|", PropName = nameof(Category.Description), Method = "equals", Value = "Cheeses" },
                new SearchModel { Link = "&", PropName = nameof(Category.Description), Method = "contains", Value = "fish" },
            };
            var operators = searches.Select(x => x.Link).Skip(1);

            // No priority
            var query = mysql.Categories.Filter(h =>
            {
                QueryExpression<Category> exp = null;

                foreach (var search in searches)
                {
                    var _exp = search.Method switch
                    {
                        "contains" => h.Property(search.PropName).Contains(search.Value),
                        "equals" => h.Property(search.PropName) == search.Value,
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
            var sql = query.ToQueryString();

            Assert.Equal(new int[0], query.Select(x => x.CategoryID));
        }

        //TODO: fixed
        //private class SearchEvaluator<TSource> : Evaluator<WhereExp<TSource>, string>
        //{
        //    public WhereHelper<TSource> WhereHelper;

        //    public SearchEvaluator(WhereHelper<TSource> helper)
        //    {
        //        WhereHelper = helper;
        //    }

        //    protected override Dictionary<string, BinaryOpFunc<WhereExp<TSource>>> OpFunctions { get; } = new Dictionary<string, BinaryOpFunc<WhereExp<TSource>>>
        //    {
        //        ["|"] = (left, right) => left | right,
        //        ["&"] = (left, right) => left & right,
        //    };

        //    protected override Dictionary<string, int> OpLevels { get; } = new Dictionary<string, int>
        //    {
        //        ["&"] = 1,
        //        ["|"] = 2,
        //    };
        //}

        //[Fact]
        //public void Test2_4()
        //{
        //    using (var mysql = ApplicationDbContext.UseMySql())
        //    {
        //        var searches = new[]
        //        {
        //            new SearchModel { Link = "", PropName = nameof(Category.CategoryName), Method = "contains", Value = "Con" },
        //            new SearchModel { Link = "|", PropName = nameof(Category.Description), Method = "equals", Value = "Cheeses" },
        //            new SearchModel { Link = "&", PropName = nameof(Category.Description), Method = "contains", Value = "fish" },
        //        };
        //        var operators = searches.Select(x => x.Link).Skip(1);

        //        var query = mysql.Categories.XWhere(h =>
        //        {
        //            var operands = searches.Select(search =>
        //            {
        //                return search.Method switch
        //                {
        //                    "contains" => h.Property<string>(search.PropName).Contains(search.Value),
        //                    "equals" => h.Property<string>(search.PropName) == search.Value,
        //                    _ => throw new NotSupportedException(),
        //                };
        //            });

        //            return new SearchEvaluator<Category>(h).Eval(operands, operators);
        //        });

        //        Assert.Equal(new[] { 2, 3 }, query.Select(x => x.CategoryID));
        //    }
        //}

        [Fact]
        public void EmptyTest()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            var expected = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var actual = mysql.Categories.Filter(h => h.Empty).Select(x => x.CategoryID).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PropertyTest1()
        {
            using var mysql = ApplicationDbContext.UseMySql();

            var query = mysql.Categories.Filter(h =>
            {
                return (h.Property("CategoryName") + "a").Contains("Con");
            });
            var sql = query.ToQueryString();
        }

        [Fact]
        public void PropertyTest2()
        {
            using var mysql = ApplicationDbContext.UseMySql();

            var query1 = mysql.Categories.Filter(h =>
            {
                return h.Property("CategoryName") + "a" == h.Property("Description");
            });
            var query2 = mysql.Categories.Where(x => x.CategoryName + "a" == x.Description);
            var sql1 = query1.ToQueryString();
            var sql2 = query2.ToQueryString();
        }

        [Fact]
        public void OwnedTest()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            mysql.Clients.Add(new Client
            {
                Name = "A",
                Address = new Address
                {
                    City = "A",
                    Street = "a101",
                }
            });
            var query = mysql.Clients.Filter(h =>
            {
                return h.Property("Address", "City") == "A";
            });
            var sql = query.ToQueryString();

#if EFCORE5_0_OR_GREATER
            Assert.Equal(@"SELECT `c`.`Id`, `c`.`Name`, `c`.`Address_City`, `c`.`Address_Street`
FROM `Clients` AS `c`
WHERE `c`.`Address_City` = 'A'", sql);

#elif EFCORE3_1_OR_GREATER
            Assert.Equal(@"SELECT `c`.`Id`, `c`.`Name`, `t`.`Id`, `t`.`Address_City`, `t`.`Address_Street`
FROM `Clients` AS `c`
LEFT JOIN (
    SELECT `c0`.`Id`, `c0`.`Address_City`, `c0`.`Address_Street`
    FROM `Clients` AS `c0`
    WHERE `c0`.`Address_Street` IS NOT NULL OR `c0`.`Address_City` IS NOT NULL
) AS `t` ON `c`.`Id` = `t`.`Id`
WHERE `t`.`Address_City` = 'A';
", sql);

#else
            Assert.Equal(@"SELECT `c`.`Id`, `c`.`Name`, `c`.`Id`, `c`.`Address_City`, `c`.`Address_Street`
FROM `Clients` AS `c`
WHERE `c`.`Address_City` = 'A';
", sql);

#endif
        }

        [Fact]
        public void DateTimeQueryFilterTest()
        {
            var filter = new DateTimeQueryFilter
            {
                Start = new DateTime(1996, 7, 1),
                End = new DateTime(1996, 8, 1),
            };

            using var mysql = ApplicationDbContext.UseMySql();
            var query = mysql.Orders.FilterBy(x => x.OrderDate, filter);
            var sql = query.ToQueryString();
            var result = query.ToArray();

            Assert.Equal(24, result.Length);

#if EFCORE6_0_OR_GREATER
            Assert.Equal(@"SET @__Start_0 = TIMESTAMP '1996-07-01 00:00:00';
SET @__End_1 = TIMESTAMP '1996-08-01 00:00:00';

SELECT `@`.`OrderID`, `@`.`CustomerID`, `@`.`EmployeeID`, `@`.`Freight`, `@`.`OrderDate`, `@`.`RequiredDate`, `@`.`ShipAddress`, `@`.`ShipCity`, `@`.`ShipCountry`, `@`.`ShipName`, `@`.`ShipPostalCode`, `@`.`ShipRegion`, `@`.`ShipVia`, `@`.`ShippedDate`
FROM `@Northwnd.Orders` AS `@`
WHERE (@__Start_0 <= `@`.`OrderDate`) AND (`@`.`OrderDate` <= @__End_1)", sql);

#elif EFCORE5_0_OR_GREATER
            Assert.Equal(@"SET @__Start_0 = '1996-07-01 00:00:00';
SET @__End_1 = '1996-08-01 00:00:00';

SELECT `@`.`OrderID`, `@`.`CustomerID`, `@`.`EmployeeID`, `@`.`Freight`, `@`.`OrderDate`, `@`.`RequiredDate`, `@`.`ShipAddress`, `@`.`ShipCity`, `@`.`ShipCountry`, `@`.`ShipName`, `@`.`ShipPostalCode`, `@`.`ShipRegion`, `@`.`ShipVia`, `@`.`ShippedDate`
FROM `@Northwnd.Orders` AS `@`
WHERE (@__Start_0 <= `@`.`OrderDate`) AND (`@`.`OrderDate` <= @__End_1)", sql);

#elif EFCORE3_1_OR_GREATER
            Assert.Equal(@"SELECT `@`.`OrderID`, `@`.`CustomerID`, `@`.`EmployeeID`, `@`.`Freight`, `@`.`OrderDate`, `@`.`RequiredDate`, `@`.`ShipAddress`, `@`.`ShipCity`, `@`.`ShipCountry`, `@`.`ShipName`, `@`.`ShipPostalCode`, `@`.`ShipRegion`, `@`.`ShipVia`, `@`.`ShippedDate`
FROM `@Northwnd.Orders` AS `@`
WHERE (@__Start_0 <= `@`.`OrderDate`) AND (`@`.`OrderDate` <= @__End_1);
", sql);

#else
            Assert.Equal(@"SELECT `x`.`OrderID`, `x`.`CustomerID`, `x`.`EmployeeID`, `x`.`Freight`, `x`.`OrderDate`, `x`.`RequiredDate`, `x`.`ShipAddress`, `x`.`ShipCity`, `x`.`ShipCountry`, `x`.`ShipName`, `x`.`ShipPostalCode`, `x`.`ShipRegion`, `x`.`ShipVia`, `x`.`ShippedDate`
FROM `@Northwnd.Orders` AS `x`
WHERE ('1996-07-01 00:00:00.000000' <= `x`.`OrderDate`) AND (`x`.`OrderDate` <= '1996-08-01 00:00:00.000000');
", sql);
#endif
        }

        [Fact]
        public void DateTimeFilterTest()
        {
            var filter = new DateTimeFilter
            {
                Start = new DateTime(1996, 7, 1),
                End = new DateTime(1996, 8, 1),
            };

            using var mysql = ApplicationDbContext.UseMySql();
            var query = mysql.Orders.FilterBy(x => x.OrderDate, filter);
            var sql = query.ToQueryString();
            var result = query.ToArray();

            Assert.Equal(24, result.Length);

#if EFCORE6_0_OR_GREATER
            Assert.Equal(@"SET @__Start_0 = TIMESTAMP '1996-07-01 00:00:00';
SET @__End_1 = TIMESTAMP '1996-08-01 00:00:00';

SELECT `@`.`OrderID`, `@`.`CustomerID`, `@`.`EmployeeID`, `@`.`Freight`, `@`.`OrderDate`, `@`.`RequiredDate`, `@`.`ShipAddress`, `@`.`ShipCity`, `@`.`ShipCountry`, `@`.`ShipName`, `@`.`ShipPostalCode`, `@`.`ShipRegion`, `@`.`ShipVia`, `@`.`ShippedDate`
FROM `@Northwnd.Orders` AS `@`
WHERE (@__Start_0 <= `@`.`OrderDate`) AND (`@`.`OrderDate` <= @__End_1)", sql);

#elif EFCORE5_0_OR_GREATER
            Assert.Equal(@"SET @__Start_0 = '1996-07-01 00:00:00';
SET @__End_1 = '1996-08-01 00:00:00';

SELECT `@`.`OrderID`, `@`.`CustomerID`, `@`.`EmployeeID`, `@`.`Freight`, `@`.`OrderDate`, `@`.`RequiredDate`, `@`.`ShipAddress`, `@`.`ShipCity`, `@`.`ShipCountry`, `@`.`ShipName`, `@`.`ShipPostalCode`, `@`.`ShipRegion`, `@`.`ShipVia`, `@`.`ShippedDate`
FROM `@Northwnd.Orders` AS `@`
WHERE (@__Start_0 <= `@`.`OrderDate`) AND (`@`.`OrderDate` <= @__End_1)", sql);

#elif EFCORE3_1_OR_GREATER
            Assert.Equal(@"SELECT `@`.`OrderID`, `@`.`CustomerID`, `@`.`EmployeeID`, `@`.`Freight`, `@`.`OrderDate`, `@`.`RequiredDate`, `@`.`ShipAddress`, `@`.`ShipCity`, `@`.`ShipCountry`, `@`.`ShipName`, `@`.`ShipPostalCode`, `@`.`ShipRegion`, `@`.`ShipVia`, `@`.`ShippedDate`
FROM `@Northwnd.Orders` AS `@`
WHERE (@__Start_0 <= `@`.`OrderDate`) AND (`@`.`OrderDate` <= @__End_1);
", sql);

#else
            Assert.Equal(@"SELECT `x`.`OrderID`, `x`.`CustomerID`, `x`.`EmployeeID`, `x`.`Freight`, `x`.`OrderDate`, `x`.`RequiredDate`, `x`.`ShipAddress`, `x`.`ShipCity`, `x`.`ShipCountry`, `x`.`ShipName`, `x`.`ShipPostalCode`, `x`.`ShipRegion`, `x`.`ShipVia`, `x`.`ShippedDate`
FROM `@Northwnd.Orders` AS `x`
WHERE ('1996-07-01 00:00:00.000000' <= `x`.`OrderDate`) AND (`x`.`OrderDate` <= '1996-08-01 00:00:00.000000');
", sql);
#endif
        }
    }
}
