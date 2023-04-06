using LinqSharp.EFCore.Data;
using LinqSharp.EFCore.Data.Test;
using LinqSharp.EFCore.Translators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class SqlServerTests
    {
        [Fact]
        public void DateTimeTest()
        {
            using var db = ApplicationDbContext.UseSqlServer();
            using var trans = db.Database.BeginTransaction();

            db.YearMonthModels.AddRange(new[]
            {
                new YearMonthModel { Date = new DateTime(2012, 1, 1), Year = 2012, Month = 1, Day = 1 },
                new YearMonthModel { Date = new DateTime(2012, 4, 16), Year = 2012, Month = 4, Day = 16 },
                new YearMonthModel { Date = new DateTime(2012, 5, 18), Year = 2012, Month = 5, Day = 18 },
            });
            db.SaveChanges();

            var query = db.YearMonthModels.Where(x => DbDateTime.Create(x.Year, x.Month, x.Day, 1, 1, 1) >= DateTime.Now);
            var sql = query.ToQueryString();
            Assert.Equal(0, query.Count());

            trans.Rollback();
        }

        [Fact]
        public void RandomTest()
        {
            using var db = ApplicationDbContext.UseSqlServer();
            using var trans = db.Database.BeginTransaction();

            db.YearMonthModels.AddRange(new[]
            {
                new YearMonthModel { Date = new DateTime(2012, 1, 1), Year = 2012, Month = 1, Day = 1 },
                new YearMonthModel { Date = new DateTime(2012, 4, 16), Year = 2012, Month = 4, Day = 16 },
                new YearMonthModel { Date = new DateTime(2012, 5, 18), Year = 2012, Month = 5, Day = 18 },
            });
            db.SaveChanges();

            var query = db.YearMonthModels.Random(2);
            var sql = query.ToQueryString();
            Assert.Equal(2, query.Count());

            trans.Rollback();
        }

    }
}
