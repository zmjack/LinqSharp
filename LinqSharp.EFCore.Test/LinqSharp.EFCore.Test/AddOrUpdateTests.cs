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
            using var mysql = ApplicationDbContext.UseMySql();
            mysql.LS_Names.Delete(x => true);

            var item1 = new LS_Name { Name = "zmjack", CreationTime = now, Note = "Added" };
            // AddOrUpdate
            var state1 = mysql.LS_Names.AddOrUpdate(x => new { x.Name, x.CreationTime }, ref item1);
            Assert.Equal(EntityState.Added, state1.State);
            mysql.SaveChanges();

            var item2 = new LS_Name { Name = "zmjack", CreationTime = now, Note = "Modified" };
            var state2 = mysql.LS_Names.AddOrUpdate(x => new { x.Name, x.CreationTime }, ref item2, options => options.Update = (record, entity) => record.Accept(entity));
            Assert.Equal(EntityState.Modified, state2.State);
            mysql.SaveChanges();

            // AddOrUpdateRange
            mysql.LS_Names.AddOrUpdateRange(x => new { x.Name, x.CreationTime }, new[]
            {
                    new LS_Name { Name = "zmjack", CreationTime = now, Note = "Unchanged" },
                    new LS_Name { Name = "zmjack(2)", CreationTime = now, Note = "Added" },
                });
            mysql.SaveChanges();
            Assert.Equal(2, mysql.LS_Names.Count());
            Assert.Equal(1, mysql.LS_Names.Count(x => x.Note == "Modified"));

            mysql.LS_Names.AddOrUpdateRange(x => new { x.Name, x.CreationTime }, new[]
            {
                    new LS_Name { Name = "zmjack", CreationTime = now, Note = "Modified - 3" },
                    new LS_Name { Name = "zmjack(2)", CreationTime = now, Note = "Modified - 3" },
                }, options => options.Update = (record, entity) => record.Note = entity.Note + " -- Changed");
            mysql.SaveChanges();
            Assert.Equal(2, mysql.LS_Names.Count(x => x.Note.Contains("Changed")));

            // Clear
            mysql.LS_Names.Delete(x => true);
            mysql.SaveChanges();
        }

        [Fact]
        public void Test2()
        {
            using var mysql = ApplicationDbContext.UseMySql();

            // AddOrUpdateRange - Empty
            mysql.LS_Names.AddOrUpdateRange(x => new { x.Name, x.CreationTime }, new LS_Name[0]);
            mysql.SaveChanges();
        }

    }
}
