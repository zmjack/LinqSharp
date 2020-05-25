using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class SelectLeafsTests
    {
        private class Tree
        {
            public string Name { get; set; }
            public Tree[] Children { get; set; }
        }

        private Tree[] Trees = new[]
        {
            new Tree
            {
                Name = "A",
                Children = new[]
                {
                    new Tree
                    {
                        Name = "A-a",
                        Children = new []
                        {
                            new Tree { Name = "1" },
                            new Tree { Name = "2" },
                        }
                    },
                    new Tree
                    {
                        Name = "A-b",
                        Children = new []
                        {
                            new Tree { Name = "3" },
                        }
                    },
                },
            },
            new Tree
            {
                Name = "B",
                Children = new[]
                {
                    new Tree { Name = "4" },
                    new Tree { Name = "5" },
                },
            },
        };

        [Fact]
        public void Test1()
        {
            var expected = new[] { "1", "2", "3", "4", "5" };
            var actual = Trees.SelectLeafs(x => x.Children).Select(x => x.Name);
            Assert.Equal(expected, actual);
        }

    }
}
