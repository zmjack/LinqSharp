using LinqSharp.EFCore.Data;
using LinqSharp.EFCore.Data.Test;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class CacheableTests
    {
        public class NameContainer : ICacheable<NameContainer.PreQueries>
        {
            public PreQueries Source { get; }
            public void OnCache()
            {
            }

            public class PreQueries
            {
                public PreQuery<ApplicationDbContext, LS_Name> LS_Names { get; set; }
            }

            public NameContainer(string name)
            {
                Source = new PreQueries
                {
                    LS_Names = new PreQuery<ApplicationDbContext, LS_Name>(x => x.LS_Names).Where(x => x.Name == name).Where(x => x.Note == "note"),
                };
            }
        }

        [Fact]
        public void CacheableTest()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            using var trans = mysql.Database.BeginTransaction();
            mysql.LS_Names.Add(new LS_Name { Name = "A", Note = "note" });
            mysql.LS_Names.Add(new LS_Name { Name = "B", Note = "note" });
            mysql.LS_Names.Add(new LS_Name { Name = "C", Note = "note" });
            mysql.SaveChanges();

            var containers = new[] { "A", "C" }.Select(n => new NameContainer(n)).ToArray();
            containers.Feed(mysql);
            Assert.Equal(new[] { "A", "C" }, containers.SelectMany(x => x.Source.LS_Names.Result).Select(x => x.Name));

            var container_b = new NameContainer("B");
            container_b.Feed(mysql);
            Assert.Equal("B", container_b.Source.LS_Names.Result.First().Name);
        }

        public class NameContainerWithFilter : ICacheable<NameContainerWithFilter.PreQueries>
        {
            public PreQueries Source { get; }
            public void OnCache()
            {
            }

            public class PreQueries
            {
                public PreQuery<ApplicationDbContext, LS_Name> LS_Names { get; set; }
            }

            public NameContainerWithFilter(string name)
            {
                Source = new PreQueries
                {
                    LS_Names = new PreQuery<ApplicationDbContext, LS_Name>(x => x.LS_Names).Filter(h =>
                    {
                        return h.Where(x => x.Name == name) & h.Where(x => x.Note == "note");
                    }),
                };
            }
        }

        [Fact]
        public void CacheableFilterTest()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            using var trans = mysql.Database.BeginTransaction();
            mysql.LS_Names.Add(new LS_Name { Name = "A", Note = "note" });
            mysql.LS_Names.Add(new LS_Name { Name = "B", Note = "note" });
            mysql.LS_Names.Add(new LS_Name { Name = "C", Note = "note" });
            mysql.SaveChanges();

            var containers = new[] { "A", "C" }.Select(n => new NameContainerWithFilter(n)).ToArray();
            containers.Feed(mysql);
            Assert.Equal(new[] { "A", "C" }, containers.SelectMany(x => x.Source.LS_Names.Result).Select(x => x.Name));

            var container_b = new NameContainerWithFilter("B");
            container_b.Feed(mysql);
            Assert.Equal("B", container_b.Source.LS_Names.Result.First().Name);
        }

    }
}
