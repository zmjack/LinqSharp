using LinqSharp.EFCore.Data;
using LinqSharp.EFCore.Data.Test;
using Microsoft.EntityFrameworkCore;
using NStandard;
using System;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class AddOrUpdateTests
    {
        [Fact]
        public void Test1()
        {
            var now = DateTime.Now;
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                mysql.LS_Names.Delete(x => true);

                // AddOrUpdate
                mysql.LS_Names
                    .AddOrUpdate(x => new { x.Name, x.CreationTime }, new LS_Name { Name = "zmjack", CreationTime = now, Note = "Added" })
                    .Then(entry =>
                    {
                        Assert.Equal(EntityState.Added, entry.State);
                    });
                mysql.SaveChanges();

                var entry = mysql.LS_Names
                    .AddOrUpdate(x => new { x.Name, x.CreationTime }, new LS_Name { Name = "zmjack", CreationTime = now, Note = "Modified" })
                    .Then(entry =>
                    {
                        Assert.Equal(EntityState.Modified, entry.State);
                    });
                mysql.SaveChanges();

                // AddOrUpdateRange
                mysql.LS_Names.AddOrUpdateRange(x => new { x.Name, x.CreationTime }, new[]
                {
                    new LS_Name { Name = "zmjack", CreationTime = now, Note = "Modified(2)" },
                    new LS_Name { Name = "zmjack(2)", CreationTime = now, Note = "Added" },
                });
                mysql.SaveChanges();
                Assert.Equal(2, mysql.LS_Names.Count());
                Assert.Equal(1, mysql.LS_Names.Count(x => x.Note.Contains("Modified")));

                mysql.LS_Names.AddOrUpdateRange(x => new { x.Name, x.CreationTime }, new[]
                {
                    new LS_Name { Name = "zmjack", CreationTime = now, Note = "Modified(3)" },
                    new LS_Name { Name = "zmjack(2)", CreationTime = now, Note = "Modified" },
                });
                mysql.SaveChanges();
                Assert.Equal(2, mysql.LS_Names.Count(x => x.Note.Contains("Modified")));

                mysql.LS_Names.AddOrUpdateRange(x => new { x.Name, x.CreationTime }, new[]
                {
                    new LS_Name { Name = "zmjack", CreationTime = now, Note = "Modified(3)" },
                    new LS_Name { Name = "zmjack(2)", CreationTime = now, Note = "Modified" },
                }, options =>
                {
                    options.Update = (record, entity) => record.Note = entity.Note + " -- Changed";
                });
                mysql.SaveChanges();
                Assert.Equal(2, mysql.LS_Names.Count(x => x.Note.Contains("Changed")));

                // Clear
                mysql.LS_Names.Delete(x => true);
                mysql.SaveChanges();
            }
        }

        [Fact]
        public void Test2()
        {
            using var mysql = ApplicationDbContext.UseMySql();

            // AddOrUpdateRange - Empty
            mysql.LS_Names.AddOrUpdateRange(x => new { x.Name, x.CreationTime }, new LS_Name[0]);
            mysql.SaveChanges();
        }

        [Fact]
        public void TmpTest()
        {
            using var mysql = ApplicationDbContext.UseMySql();
            var month = DateTime.Now.StartOfMonth();

            var records = new LS_Name[5_000].Let(i =>
            {
                return new LS_Name { Name = i.ToString(), CreationTime = month.AddDays(i), };
            });

            PerfProbe.Perf.UseUdpClient("127.0.0.1", 26778);

            PerfProbe.Perf.Set("AddOrUpdateRange");
            mysql.LS_Names.AddOrUpdateRange(x => new { x.Name, x.CreationTime }, records, options =>
            {
            });

            PerfProbe.Perf.Set("Save");
            mysql.SaveChanges();
            PerfProbe.Perf.End();

            // Clear
            //mysql.LS_Names.Delete(x => true);
            //mysql.SaveChanges();
        }

    }
}
