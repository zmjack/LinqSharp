using LinqSharp.EFCore.Data;
using LinqSharp.EFCore.Data.Test;
using Microsoft.EntityFrameworkCore;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class AddOrUpdateTests
    {
        [Fact]
        public void Test1()
        {
            using (var mysql = ApplicationDbContext.UseMySql())
            {
                // AddOrUpdate
                mysql.LS_Names.AddOrUpdate(x => x.Name, new LS_Name { Name = "zmjack", Note = "Added" }).Then(entry =>
                {
                    Assert.Equal(EntityState.Added, entry.State);
                });
                mysql.SaveChanges();

                var entry = mysql.LS_Names.AddOrUpdate(x => x.Name, new LS_Name { Name = "zmjack", Note = "Modified" }).Then(entry =>
                {
                    Assert.Equal(EntityState.Modified, entry.State);
                });
                mysql.SaveChanges();

                // AddOrUpdateRange
                mysql.LS_Names.AddOrUpdateRange(x => x.Name, new[]
                {
                    new LS_Name { Name = "zmjack", Note = "Modified(2)" },
                    new LS_Name { Name = "zmjack(2)", Note = "Added" },
                });
                mysql.SaveChanges();
                Assert.Equal(2, mysql.LS_Names.Count());
                Assert.Equal(1, mysql.LS_Names.Count(x => x.Note.Contains("Modified")));

                mysql.LS_Names.AddOrUpdateRange(x => x.Name, new[]
                {
                    new LS_Name { Name = "zmjack", Note = "Modified(3)" },
                    new LS_Name { Name = "zmjack(2)", Note = "Modified" },
                });
                mysql.SaveChanges();
                Assert.Equal(2, mysql.LS_Names.Count(x => x.Note.Contains("Modified")));

                mysql.LS_Names.AddOrUpdateRange(x => x.Name, new[]
                {
                    new LS_Name { Name = "zmjack", Note = "Modified(3)" },
                    new LS_Name { Name = "zmjack(2)", Note = "Modified" },
                }, options =>
                {
                    options.Update = (record, entity) => record.Note = entity.Note + " -- Changed";
                });
                mysql.SaveChanges();
                Assert.Equal(2, mysql.LS_Names.Count(x => x.Note.Contains("Changed")));

                // Clear
                var records = mysql.LS_Names.ToArray();
                mysql.LS_Names.RemoveRange(records);
                mysql.SaveChanges();
            }
        }

    }
}
