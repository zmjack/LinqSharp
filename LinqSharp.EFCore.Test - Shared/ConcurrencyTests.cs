using LinqSharp.EFCore.Data.Test;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class ConcurrencyTests
    {
        private readonly Random Random = new Random();

        [Fact]
        public void CheckDefaultTest()
        {
            var num = Random.Next();

            using (var context1 = ApplicationDbContext.UseMySql())
            using (var context2 = ApplicationDbContext.UseMySql())
            {
                context1.ConcurrencyModels.Add(new ConcurrencyModel
                {
                    RandomNumber = num,
                    CheckDefault = 0,
                });
                context1.SaveChanges();

                var record1 = context1.ConcurrencyModels.First(x => x.RandomNumber == num);
                record1.CheckDefault = 1;

                var record2 = context2.ConcurrencyModels.First(x => x.RandomNumber == num);
                record2.CheckDefault = 2;

                context1.SaveChanges();

                Assert.ThrowsAny<DbUpdateConcurrencyException>(() => context2.SaveChanges());
            }
        }

        [Fact]
        public void CheckThrowTest()
        {
            using (var context1 = ApplicationDbContext.UseMySql())
            using (var context2 = ApplicationDbContext.UseMySql())
            {

            }
        }

        [Fact]
        public void CheckStoreWinTest()
        {
            using (var context1 = ApplicationDbContext.UseMySql())
            using (var context2 = ApplicationDbContext.UseMySql())
            {

            }
        }

        [Fact]
        public void CheckClientWinTest()
        {
            using (var context1 = ApplicationDbContext.UseMySql())
            using (var context2 = ApplicationDbContext.UseMySql())
            {

            }
        }

        [Fact]
        public void CheckCombineTest()
        {
            using (var context1 = ApplicationDbContext.UseMySql())
            using (var context2 = ApplicationDbContext.UseMySql())
            {

            }
        }

    }
}
