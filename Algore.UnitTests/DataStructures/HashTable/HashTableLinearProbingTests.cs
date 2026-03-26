using Algore.DataStructures.HashTable;
using Xunit;

namespace Algore.UnitTests.DataStructures.HashTable;

public class HashTableLinearProbingTests
{
    private sealed record Person(int Id, string Name);

    [Fact]
    public void Search_EmptyTable_ReturnsDefault()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 8, predicate: i => i);

        var result = table.Search(10);

        Assert.Equal(0, result);
    }

    [Fact]
    public void Insert_AndSearch_SingleValue_Works()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 8, predicate: i => i);

        table.Insert(10);

        Assert.Equal(10, table.Search(10));
    }

    [Fact]
    public void Search_MissingKeyInNonEmptyTable_ReturnsDefault()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 8, predicate: i => i);
        table.Insert(10);
        table.Insert(20);
        table.Insert(30);

        var result = table.Search(99);

        Assert.Equal(0, result);
    }

    [Fact]
    public void Insert_MultipleValues_SearchFindsAllValues()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 8, predicate: i => i);

        foreach (var value in new[] { 10, 20, 30, 15, 25, 5, 1, 50 })
        {
            table.Insert(value);
        }

        Assert.Equal(10, table.Search(10));
        Assert.Equal(20, table.Search(20));
        Assert.Equal(30, table.Search(30));
        Assert.Equal(15, table.Search(15));
        Assert.Equal(25, table.Search(25));
        Assert.Equal(5, table.Search(5));
        Assert.Equal(1, table.Search(1));
        Assert.Equal(50, table.Search(50));
    }

    [Fact]
    public void Insert_CollisionsAreHandledByLinearProbing()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 2, predicate: i => i);

        table.Insert(1);
        table.Insert(3);
        table.Insert(5);

        Assert.Equal(1, table.Search(1));
        Assert.Equal(3, table.Search(3));
        Assert.Equal(5, table.Search(5));
    }

    [Fact]
    public void Insert_DuplicateKey_ReplacesExistingValue()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 8, predicate: i => i);

        table.Insert(10);
        table.Insert(10);
        table.Insert(10);

        Assert.Equal(10, table.Search(10));
    }

    [Fact]
    public void Insert_TriggersResize_AndSearchStillWorks()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 2, predicate: i => i);

        for (var i = 0; i < 20; i++)
        {
            table.Insert(i);
        }

        for (var i = 0; i < 20; i++)
        {
            Assert.Equal(i, table.Search(i));
        }
    }

    [Fact]
    public void Remove_DeletesItem_AndSearchReturnsDefault()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 8, predicate: i => i);

        table.Insert(10);
        table.Insert(20);
        table.Insert(30);

        var removed = table.Remove(20);

        Assert.True(removed);
        Assert.Equal(0, table.Search(20));
        Assert.Equal(10, table.Search(10));
        Assert.Equal(30, table.Search(30));
    }

    [Fact]
    public void Insert_AfterRemove_ReusesDeletedSlotCorrectly()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 8, predicate: i => i);

        table.Insert(10);
        table.Insert(18);
        table.Insert(26);

        table.Remove(18);
        table.Insert(34);

        Assert.Equal(10, table.Search(10));
        Assert.Equal(26, table.Search(26));
        Assert.Equal(34, table.Search(34));
    }

    [Fact]
    public void Insert_AndSearch_CustomTypeWithKeySelector_WorksCorrectly()
    {
        var table = new HashTableLinearProbing<Person, int>(capacity: 8, predicate: p => p.Id);

        var alice = new Person(2, "Alice");
        var bob = new Person(1, "Bob");
        var carol = new Person(3, "Carol");

        table.Insert(alice);
        table.Insert(bob);
        table.Insert(carol);

        Assert.Equal(alice, table.Search(2));
        Assert.Equal(bob, table.Search(1));
        Assert.Equal(carol, table.Search(3));
    }

    [Fact]
    public void Search_WithExplicitPredicateParameter_UsesProvidedPredicate()
    {
        var table = new HashTableLinearProbing<Person, int>(capacity: 8);

        var bob = new Person(1, "Bob");
        table.Insert(bob, p => p.Id);

        var result = table.Search(1, p => p.Id);

        Assert.Equal(bob, result);
    }

    [Fact]
    public void GetTotalCollisions_NoCollisions_ReturnsZero()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 16, predicate: i => i);

        table.Insert(1);
        table.Insert(2);
        table.Insert(3);

        Assert.Equal(0, table.GetTotalCollisions());
    }

    [Fact]
    public void GetTotalCollisions_WithCollisions_ReturnsCorrectCount()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 8, predicate: i => i);

        table.Insert(0);
        table.Insert(8);
        table.Insert(16);

        Assert.Equal(2, table.GetTotalCollisions());
    }

    [Fact]
    public void CollisionStatistics_MultipleInsertions_TracksAccurately()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 8, predicate: i => i);

        for (var i = 0; i < 10; i++)
        {
            table.Insert(i);
        }

        Assert.Equal(2, table.GetTotalCollisions());
        Assert.Equal(1.6, table.GetAverageProbeLength());
    }

    [Fact]
    public void GetAverageCollisionsPerInsertion_NoCollisions_ReturnsZero()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 16, predicate: i => i);

        table.Insert(1);
        table.Insert(2);
        table.Insert(3);

        Assert.Equal(0, table.GetAverageCollisionsPerInsertion());
    }

    [Fact]
    public void GetAverageCollisionsPerInsertion_WithCollisions_ReturnsCorrectAverage()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 4, predicate: i => i);

        table.Insert(0);
        table.Insert(4);
        table.Insert(8);

        Assert.Equal(2.0 / 3.0, table.GetAverageCollisionsPerInsertion(), precision: 10);
    }

    [Fact]
    public void GetAverageChainLength_EmptyTable_ReturnsZero()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 8, predicate: i => i);

        Assert.Equal(0, table.GetAverageChainLength());
    }

    [Fact]
    public void GetAverageChainLength_NoCollisions_ReturnsOne()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 16, predicate: i => i);

        table.Insert(0);
        table.Insert(2);
        table.Insert(4);

        Assert.Equal(1.0, table.GetAverageChainLength());
    }

    [Fact]
    public void GetAverageChainLength_WithConsecutiveEntries_ReturnsCorrectAverage()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 8, predicate: i => i);

        table.Insert(0);
        table.Insert(8);
        table.Insert(16);

        Assert.Equal(3.0, table.GetAverageChainLength());
    }

    [Fact]
    public void GetAverageChainLength_MultipleChains_CalculatesCorrectly()
    {
        var table = new HashTableLinearProbing<int, int>(capacity: 8, predicate: i => i);

        table.Insert(0);
        table.Insert(1);
        table.Insert(4);
        table.Insert(5);

        var avgChainLength = table.GetAverageChainLength();
        Assert.Equal(2.0, avgChainLength);
    }
}
