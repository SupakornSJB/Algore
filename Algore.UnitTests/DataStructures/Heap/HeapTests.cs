using Algore.DataStructures.Heap;
using Algore.Interfaces;
using Xunit;

namespace Algore.UnitTests.DataStructures.Heap;

public class HeapTests
{
    private sealed record Person(int Id, string Name);

    [Fact]
    public void MinHeap_InsertAndExtract_ReturnsAscendingOrder()
    {
        var heap = new MinHeap<int, int>(i => i);
        foreach (var value in new[] { 5, 2, 9, 1, 7, 3 })
        {
            heap.Insert(value);
        }

        var extracted = new List<int>();
        while (heap.Count > 0)
        {
            extracted.Add(heap.ExtractRoot());
        }

        Assert.Equal(new[] { 1, 2, 3, 5, 7, 9 }, extracted);
    }

    [Fact]
    public void MaxHeap_InsertAndExtract_ReturnsDescendingOrder()
    {
        var heap = new MaxHeap<int, int>(i => i);
        foreach (var value in new[] { 5, 2, 9, 1, 7, 3 })
        {
            heap.Insert(value);
        }

        var extracted = new List<int>();
        while (heap.Count > 0)
        {
            extracted.Add(heap.ExtractRoot());
        }

        Assert.Equal(new[] { 9, 7, 5, 3, 2, 1 }, extracted);
    }

    [Fact]
    public void Peek_EmptyHeap_ReturnsDefault()
    {
        var heap = new MinHeap<int, int>(i => i);

        Assert.Equal(0, heap.Peek());
    }

    [Fact]
    public void ExtractRoot_EmptyHeap_ReturnsDefault()
    {
        var heap = new MaxHeap<int, int>(i => i);

        Assert.Equal(0, heap.ExtractRoot());
    }

    [Fact]
    public void Search_WithExistingAndMissingKeys_Works()
    {
        var heap = new MinHeap<int, int>(i => i);
        heap.Insert(10);
        heap.Insert(4);
        heap.Insert(12);

        Assert.Equal(4, heap.Search(4));
        Assert.Equal(0, heap.Search(99));
    }

    [Fact]
    public void Heap_ImplementsIDataStructure()
    {
        IDataStructure<int, int> heap = new MinHeap<int, int>(i => i);
        heap.Insert(42);

        Assert.Equal(42, heap.Search(42));
    }

    [Fact]
    public void Insert_WithExplicitPredicate_InitializesHeapPredicate()
    {
        var heap = new MinHeap<Person, int>();
        var alice = new Person(1, "Alice");
        heap.Insert(alice, p => p.Id);

        Assert.Equal(alice, heap.Search(1, p => p.Id));
    }

    [Fact]
    public void Insert_ChangingPredicateAfterInitialization_Throws()
    {
        var heap = new MinHeap<Person, int>(p => p.Id);
        heap.Insert(new Person(1, "Alice"));

        Assert.Throws<InvalidOperationException>(() =>
            heap.Insert(new Person(2, "Bob"), p => p.Name.Length));
    }
}
