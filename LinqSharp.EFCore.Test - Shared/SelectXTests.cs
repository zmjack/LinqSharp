using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LinqSharp.EFCore.Test
{
    public class SelectXTests
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
            new Tree
            {
                Name = "6",
            },
        };

        [Fact]
        public void SelectUntilTest()
        {
            var expected = new[] { "1", "2", "3", "4", "5", "6" };
            var actual = Trees.SelectUntil(x => x.Children, x => !(x?.Any() ?? false)).Select(x => x.Name).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SelectWhileTest()
        {
            var expected = new[] { "A", "A-a", "A-b", "B" };
            var actual = Trees.SelectWhile(x => x.Children, x => x?.Any() ?? false).Select(x => x.Name).ToArray();
            Assert.Equal(expected, actual);
        }

    }
}
