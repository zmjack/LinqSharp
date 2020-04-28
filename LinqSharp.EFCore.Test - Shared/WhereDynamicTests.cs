using LinqSharp.EFCore.Data.Test;
using Northwnd;
using System;
using System.Reflection;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class DynamicTests
    {
        [Fact]
        public void Test1()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query = mysql.Categories
                    .WhereDynamic(x => x.SetDynamic(x => x.Property(nameof(Category.CategoryName)).Invoke(MethodUnit.StringContains, "Con")));

                Assert.Equal(@"Param_0 => Param_0.CategoryName.Contains(""Con"")", Utility.GetExpString(query));
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
            static MethodInfo parseMethod(string method) => method switch
            {
                "equals" => MethodUnit.StringEquals,
                "contains" => MethodUnit.StringContains,
                _ => throw new NotSupportedException(),
            };

            using (var mysql = ApplicationDbContext.UseMySql())
            {
                var query = mysql.Categories
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
                var exp = Utility.GetExpString(query);
                Assert.Equal("Param_0 => ((Param_0.CategoryName.Contains(\"Con\") OrElse Param_0.Description.Equals(\"Cheeses\")) AndAlso Param_0.Description.Contains(\"fish\"))", exp);
            }
        }

    }
}
