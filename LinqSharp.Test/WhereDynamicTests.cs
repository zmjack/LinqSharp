using Northwnd;
using System;
using System.Reflection;
using Xunit;

namespace LinqSharp.Test
{
    public class DynamicTests
    {
        [Fact]
        public void Test1()
        {
            using (var sqlite = NorthwndContext.UseSqliteResource())
            {
                var query = sqlite.Categories
                    .WhereDynamic(x => x.SetDynamic(x => x.Property(nameof(Category.CategoryName)).Invoke(BuiltInMethod.StringContains, "Con")));
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
                new { link = "and", prop = nameof(Category.Description), method = "contains", value = "fish" },
            };
            MethodInfo parseMethod(string method) => method switch
            {
                "equals" => BuiltInMethod.StringEquals,
                "contains" => BuiltInMethod.StringContains,
                _ => throw new NotSupportedException(),
            };

            string sql;
            using (var sqlite = NorthwndContext.UseSqliteResource())
            {
                var query = sqlite.Categories
                    .WhereDynamic(builder =>
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
                    });
                sql = query.ToSql();

                Assert.Equal(@"SELECT ""c"".""CategoryID"", ""c"".""CategoryName"", ""c"".""Description"", ""c"".""Picture""
FROM ""Categories"" AS ""c""
WHERE ((instr(""c"".""CategoryName"", 'Con') > 0) OR (""c"".""Description"" = 'Cheeses')) AND (instr(""c"".""Description"", 'fish') > 0);
", sql);
            }
        }

    }
}
