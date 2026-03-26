using Algore.DataStructures.Tree;
using Algore.DataStructures.Tree.Implementations;
using Xunit;

namespace Algore.UnitTests.DataStructures.Tree;

public class RedBlackTreeTests
{
    private sealed record Person(int Id, string Name);
    
    [Fact]
    public void Search_EmptyTree_ReturnsDefault()
    {
        var tree = new RedBlackTree<int, int>(i => i);

        var result = tree.Search(10);

        Assert.Equal(default, result);
    }

    [Fact]
    public void Insert_RootNode_SearchReturnsRootValue()
    {
        var tree = new RedBlackTree<int, int>(i => i);
        tree.Insert(10);

        var result = tree.Search(10);

        Assert.Equal(10, result);
    }

    [Fact]
    public void Search_LeftAndRightSubtrees_ReturnsExpectedValues()
    {
        var tree = new RedBlackTree<int, int>(i => i);
        tree.Insert(10);
        tree.Insert(5);
        tree.Insert(15);
        tree.Insert(2);
        tree.Insert(7);
        tree.Insert(12);
        tree.Insert(20);

        Assert.Equal(2, tree.Search(2));
        Assert.Equal(7, tree.Search(7));
        Assert.Equal(12, tree.Search(12));
        Assert.Equal(20, tree.Search(20));
    }

    [Fact]
    public void Search_MissingKeyInNonEmptyTree_ReturnsDefault()
    {
        var tree = new RedBlackTree<int, int>(i => i);
        tree.Insert(10);
        tree.Insert(5);
        tree.Insert(15);

        var result = tree.Search(9);

        Assert.Equal(default, result);
    }

    [Fact]
    public void Insert_DuplicateKeys_StillFindsOriginalAndGreaterValues()
    {
        var tree = new RedBlackTree<int, int>(i => i);
        tree.Insert(10);
        tree.Insert(10);
        tree.Insert(10);
        tree.Insert(11);

        Assert.Equal(10, tree.Search(10));
        Assert.Equal(11, tree.Search(11));
    }

    [Fact]
    public void Insert_MultipleValues_SearchStillFindsAllValues()
    {
        var tree = new RedBlackTree<int, int>(i => i);

        foreach (var value in new[] { 10, 20, 30, 15, 25, 5, 1, 50 })
        {
            tree.Insert(value);
        }

        Assert.Equal(10, tree.Search(10));
        Assert.Equal(20, tree.Search(20));
        Assert.Equal(30, tree.Search(30));
        Assert.Equal(15, tree.Search(15));
        Assert.Equal(25, tree.Search(25));
        Assert.Equal(5, tree.Search(5));
        Assert.Equal(1, tree.Search(1));
        Assert.Equal(50, tree.Search(50));
    }

    [Fact]
    public void Insert_AndSearch_CustomTypeWithKeySelector_WorksCorrectly()
    {
        var tree = new RedBlackTree<Person, int>(p => p.Id);
        var alice = new Person(2, "Alice");
        var bob = new Person(1, "Bob");
        var carol = new Person(3, "Carol");

        tree.Insert(alice);
        tree.Insert(bob);
        tree.Insert(carol);

        var result = tree.Search(3);

        Assert.Equal(carol, result);
    }

    [Fact]
    public void Search_WithExplicitPredicateParameter_UsesProvidedPredicate()
    {
        var tree = new RedBlackTree<Person, int>(p => p.Id);
        var bob = new Person(1, "Bob");

        tree.Insert(bob, p => p.Id);

        var result = tree.Search(1, p => p.Id);

        Assert.Equal(bob, result);
    }
    
    [Fact]
    public void Insert_CustomType_WithKeySelector_Works()
    {
        var tree = new RedBlackTree<Person, int>(p => p.Id);

        var alice = new Person(2, "Alice");
        var bob = new Person(1, "Bob");
        var carol = new Person(3, "Carol");

        tree.Insert(alice);
        tree.Insert(bob);
        tree.Insert(carol);

        Assert.Equal(alice, tree.Search(2));
        Assert.Equal(bob, tree.Search(1));
        Assert.Equal(carol, tree.Search(3));
    }
    
    
    [Fact]
    public void GetHeight_SingleNodeRbt_ReturnsZero()
    {
        var rbt = new RedBlackTree<int, int>(p => p);
        rbt.Insert(10);

        Assert.Equal(0, rbt.Height);
    }

    [Fact]
    public void GetHeight_RbtWithMultipleValues_ReturnsReasonableBalancedHeight()
    {
        var rbt = new RedBlackTree<int, int>(p => p);

        foreach (var value in new[] { 10, 20, 30, 15, 25, 5, 1, 50 })
        {
            rbt.Insert(value);
        }

        Assert.InRange(rbt.Height, 2, 4);
    }

    [Fact]
    public void GetHeight_RbtCustomType_WorksCorrectly()
    {
        var rbt = new RedBlackTree<Person, int>(p => p.Id);

        rbt.Insert(new Person(2, "Alice"));
        rbt.Insert(new Person(1, "Bob"));
        rbt.Insert(new Person(3, "Carol"));

        Assert.Equal(1, rbt.Height);
    }
}