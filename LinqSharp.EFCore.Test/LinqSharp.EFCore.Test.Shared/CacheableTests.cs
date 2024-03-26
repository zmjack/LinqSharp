using LinqSharp.EFCore.Data;
using LinqSharp.EFCore.Data.Test;
using NStandard;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test;

public class CacheableTests
{
    public class NameCacheContainer : ICacheable<NameCacheContainer.DataSource>
    {
        public DataSource Source { get; }
        public void OnCached()
        {
        }

        public class DataSource
        {
            public QueryDef<LS_Name> LS_Names { get; set; }
            public QueryDef<LS_Name> LS_Names_FromFilter { get; set; }
        }

        public NameCacheContainer(string name)
        {
            Source = new DataSource
            {
                LS_Names = new QueryDef<LS_Name>().Where(x => x.Name == name).Where(x => x.Note == "note"),
                LS_Names_FromFilter = new QueryDef<LS_Name>().Filter(h =>
                {
                    return h.Where(x => x.Name == name) & h.Where(x => x.Note == "note");
                }),
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

        var caches = new[] { "A", "C" }.Select(n => new NameCacheContainer(n)).ToArray();

        using var query = mysql.BeginCompoundQuery(x => x.LS_Names);
        query.Feed(caches.Select(x => x.Source.LS_Names).ToArray());

        Assert.Equal(new[] { "A", "C" }, caches.SelectMany(x => x.Source.LS_Names.Result).Select(x => x.Name));

        var cache_b = new NameCacheContainer("B");
        query.Feed(cache_b.Source.LS_Names);
        Assert.Equal("B", cache_b.Source.LS_Names.Result.First().Name);
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

        var caches = new[] { "A", "C" }.Select(n => new NameCacheContainer(n)).ToArray();

        using var compound = mysql.BeginCompoundQuery(x => x.LS_Names);
        compound.Feed(caches.Select(x => x.Source.LS_Names_FromFilter).ToArray());
        Assert.Equal(new[] { "A", "C" }, caches.SelectMany(x => x.Source.LS_Names_FromFilter.Result).Select(x => x.Name));

        var cache_b = new NameCacheContainer("B");
        compound.Feed(cache_b.Source.LS_Names_FromFilter);
        Assert.Equal("B", cache_b.Source.LS_Names_FromFilter.Result.First().Name);
    }

}
