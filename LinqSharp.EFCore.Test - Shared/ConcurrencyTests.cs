using LinqSharp.Data.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqSharp.Test
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
                context1.SimpleModels.Add(new SimpleModel
                {
                    RandomNumber = num,
                    CheckDefault = 0,
                });
                context1.SaveChanges();

                var record1 = context1.SimpleModels.First(x => x.RandomNumber == num);
                record1.CheckDefault = 1;

                var record2 = context2.SimpleModels.First(x => x.RandomNumber == num);
                record2.CheckDefault = 2;

                context1.SaveChanges();
                context2.SaveChanges();
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
