﻿using Xunit;

namespace LinqSharp.EFCore.Test;

public class SelectAnyTests
{
    private class Tree
    {
        public string Name { get; set; }
        public Tree[] Children { get; set; }
    }

    private Tree[] Trees =
    [
        new Tree
        {
            Name = "A",
            Children =
            [
                new Tree
                {
                    Name = "A-a",
                    Children =
                    [
                        new Tree { Name = "1" },
                        new Tree { Name = "2" },
                    ]
                },
                new Tree
                {
                    Name = "A-b",
                    Children =
                    [
                        new Tree { Name = "3" },
                    ]
                },
            ],
        },
        new Tree
        {
            Name = "B",
            Children =
            [
                new Tree { Name = "4" },
                new Tree { Name = "5" },
            ],
        },
        new Tree
        {
            Name = "6",
        },
    ];

    [Fact]
    public void SelectUntilTest()
    {
        var expected = new[] { "1", "2", "3", "4", "5", "6" };
        var actual = Trees.SelectUntil(x => x.Children, x => !(x.Children?.Any() ?? false)).Select(x => x.Name).ToArray();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SelectWhileTest()
    {
        var expected = new[] { "A", "A-a", "A-b", "B" };
        var actual = Trees.SelectWhile(x => x.Children, x => x.Children?.Any() ?? false).Select(x => x.Name).ToArray();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SelectMoreTest1()
    {
        var expected = new[] { "A", "A-a", "1", "2", "A-b", "3", "B", "4", "5", "6" };
        var actual = Trees.SelectMore(x => x.Children).Select(x => x.Name).ToArray();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SelectMoreTest2()
    {
        var expected = new[] { "A", "A-a", "A-b" };
        var actual = Trees.SelectMore(x => x.Children, x => x.Name.Contains("A")).Select(x => x.Name).ToArray();
        Assert.Equal(expected, actual);
    }

}
