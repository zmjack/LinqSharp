using LinqSharp.EFCore.Models.Test;
using NStandard;
using System.Linq;
using Xunit;

namespace LinqSharp.EFCore.Test.Shared
{
    public class IndexTests
    {
        [Fact]
        public void Test1()
        {
            var arr = new NameModel[10].Let(i => new NameModel { Name = i.ToString(), NickName = $"NN: {i}", Tag = (i / 5).ToString() });

            var modelsByTag = arr.Index(x => x.Tag);
            var modelsByNameNickName = arr.Index(x => new { x.Name, x.NickName });

            Assert.Equal(0, modelsByTag["0"].Sum(x => int.Parse(x.Tag)));

            Assert.Equal(0, modelsByTag["0"].Select(x => int.Parse(x.Tag)).Sum());
            Assert.Equal(5, modelsByTag["1"].Select(x => int.Parse(x.Tag)).Sum());
            Assert.Equal(0, modelsByTag["2"].Select(x => int.Parse(x.Tag)).Sum());

            Assert.Equal("1", modelsByNameNickName[new { Name = "5", NickName = "NN: 5" }].First().Tag);
        }
    }
}
