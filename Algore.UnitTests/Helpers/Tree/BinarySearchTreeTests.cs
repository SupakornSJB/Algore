using Algore.Helpers.Tree;
using Xunit;

namespace Algore.UnitTests.Helpers.Tree;

public class BinarySearchTreeTests
{
    private sealed record Person(int Id, string Name);

    [Fact]
    public void Search_EmptyTree_ReturnsDefault()
    {
        var bst = new BinarySearchTree<int, int>(i => i);

        var result = bst.Search(10);

        Assert.Equal(default, result);
    }

    [Fact]
    public void Insert_DuplicateKey_DoesNotLoseExistingRightSubtree()
    {
        var bst = new BinarySearchTree<int, int>(i => i);
        bst.Insert(10);
        bst.Insert(20);
        bst.Insert(30);

        bst.Insert(20);

        Assert.Equal(30, bst.Search(30));
    }

    [Fact]
    public void Search_RootNode_ReturnsRootValue()
    {
        var bst = new BinarySearchTree<int, int>(i => i);
        bst.Insert(10);

        var result = bst.Search(10);

        Assert.Equal(10, result);
    }

    [Fact]
    public void Search_LeftAndRightSubtrees_ReturnsExpectedValues()
    {
        var bst = new BinarySearchTree<int, int>(i => i);
        bst.Insert(10);
        bst.Insert(5);
        bst.Insert(15);
        bst.Insert(2);
        bst.Insert(7);
        bst.Insert(12);
        bst.Insert(20);

        Assert.Equal(2, bst.Search(2));
        Assert.Equal(7, bst.Search(7));
        Assert.Equal(12, bst.Search(12));
        Assert.Equal(20, bst.Search(20));
    }

    [Fact]
    public void Search_MissingKeyInNonEmptyTree_ReturnsDefault()
    {
        var bst = new BinarySearchTree<int, int>(i => i);
        bst.Insert(10);
        bst.Insert(5);
        bst.Insert(15);

        var result = bst.Search(9);

        Assert.Equal(default, result);
    }

    [Fact]
    public void Insert_DuplicateKeys_StillFindsOriginalAndGreaterValues()
    {
        var bst = new BinarySearchTree<int, int>(i => i);
        bst.Insert(10);
        bst.Insert(10);
        bst.Insert(10);
        bst.Insert(11);

        Assert.Equal(10, bst.Search(10));
        Assert.Equal(11, bst.Search(11));
    }

    [Fact]
    public void Insert_AndSearch_CustomTypeWithKeySelector_WorksCorrectly()
    {
        var bst = new BinarySearchTree<Person, int>(p => p.Id);
        var alice = new Person(2, "Alice");
        var bob = new Person(1, "Bob");
        var carol = new Person(3, "Carol");
        bst.Insert(alice);
        bst.Insert(bob);
        bst.Insert(carol);

        var result = bst.Search(3);

        Assert.Equal(carol, result);
    }

    [Fact]
    public void Search_WithExplicitPredicateParameter_UsesProvidedPredicate()
    {
        var bst = new BinarySearchTree<Person, int>(p => p.Id);
        var bob = new Person(1, "Bob");
        bst.Insert(bob, p => p.Id);

        var result = bst.Search(1, p => p.Id);

        Assert.Equal(bob, result);
    }
}
