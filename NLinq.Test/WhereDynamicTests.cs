using Microsoft.EntityFrameworkCore;
using NStandard;
using SimpleData;
using SimpleData.Northwnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Xunit;

namespace NLinq.Test
{
    public class DynamicTests
    {
        private readonly DbContextOptions SqliteOptions = SimpleSources.NorthwndOptions;

        [Fact]
        public void Test1()
        {
            using (var sqlite = new NorthwndContext(SqliteOptions))
            {
                var query = sqlite.Categories
                    .WhereDynamic(x => x.Property(nameof(Category.CategoryName)).Invoke(BuiltInMethod.StringContains, "Con"));
                var sql = query.ToSql();

                Assert.Equal(@"SELECT ""c"".""CategoryID"", ""c"".""CategoryName"", ""c"".""Description"", ""c"".""Picture""
FROM ""Categories"" AS ""c""
WHERE instr(""c"".""CategoryName"", 'Con') > 0;
", sql);
            }
        }

        [Fact]
        public void Test2()
        {
            var in_searches = new[]
            {
                new { link = "", prop = nameof(Category.CategoryName), method = "contains", value = "Con" },
                new { link = "or", prop = nameof(Category.Description), method = "equals", value = "Cheeses" },
                new { link = "or", prop = nameof(Category.Description), method = "contains", value = "fish" },
            };
            MethodInfo parseMethod(string method) => method switch
            {
                "equals" => BuiltInMethod.StringEquals,
                "contains" => BuiltInMethod.StringContains,
                _ => throw new NotSupportedException(),
            };

            string sql;
            using (var sqlite = new NorthwndContext(SqliteOptions))
            {
                var query = sqlite.Categories
                    .Begin().Then(builder =>
                    {
                        foreach (var search in in_searches)
                        {
                            var predicate = (Action<DynamicExpressionBuilder<Category>>)(x => x.Property(search.prop).Invoke(parseMethod(search.method), search.value));
                            switch (search.link)
                            {
                                case "": builder.SetDynamic(predicate); break;
                                case "or": builder.OrDynamic(predicate); break;
                                case "and": builder.AndDynamic(predicate); break;
                            }
                        }
                    }).End();
                sql = query.ToSql();

                Assert.Equal(@"SELECT ""c"".""CategoryID"", ""c"".""CategoryName"", ""c"".""Description"", ""c"".""Picture""
FROM ""Categories"" AS ""c""
WHERE ((instr(""c"".""CategoryName"", 'Con') > 0) OR (""c"".""Description"" = 'Cheeses')) OR (instr(""c"".""Description"", 'fish') > 0);
", sql);
            }
        }

    }
}
