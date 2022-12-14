using LinqSharp.EFCore.Data.Test;
using Northwnd;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class QuickDataViewTests
    {
        [Fact]
        public void Test()
        {
            using var context = ApplicationDbContext.UseMySql();

            var array1 = context.Regions.Take(3).ToArray();
            var array2 = context.Regions.Skip(1).Take(3).ToArray();

            var result = array1.FullJoin(array2, x => x.RegionID, x => x.RegionID, (l, r) => new
            {
                Left = l,
                Right = r,
            });

            var view = QuickDataView.Create(new[]
            {
                DataBinding.Create("Left", array1, x => x.RegionID),
                DataBinding.Create("Right", array2, x => x.RegionID),
            }, (key, dict) =>
            {
                return new
                {
                    Left = dict.GetValueOrDefault("Left") as Region,
                    Right = dict.GetValueOrDefault("Right") as Region,
                };
            });

            Assert.True(result.SequenceEqual(view.Values));
        }
    }
}
