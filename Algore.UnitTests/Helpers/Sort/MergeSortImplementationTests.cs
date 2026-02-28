using Algore.Helpers.Sort;
using Xunit;

namespace Algore.UnitTests.Helpers.Sort;

public class MergeSortImplementationTests
{
    private sealed record Item(string Key, int OriginalIndex) : IComparable<Item>
    {
        public int CompareTo(Item? other)
        {
            throw new NotImplementedException();
        }
    }

    private static readonly IComparer<Item> KeyOnlyComparer =
        Comparer<Item>.Create((a, b) => string.Compare(a.Key, b.Key, StringComparison.Ordinal));
    
    [Fact]
    public void MergeSort_EmptyList_DoesNotThrow()
    {
        var list = new List<int>();

        MergeSortImplementation.Sort(list, asc: true);

        Assert.Empty(list);
    }

    [Fact]
    public void MergeSort_SingleElement_Unchanged()
    {
        var list = new List<int> { 42 };

        MergeSortImplementation.Sort(list, asc: true);

        Assert.Equal(new List<int> { 42 }, list);
    }

    [Fact]
    public void MergeSort_Ascending_SortsCorrectly()
    {
        var list = new List<int> { 5, 1, 4, 2, 8, 0, -3 };

        MergeSortImplementation.Sort(list, asc: true);

        Assert.Equal(new List<int> { -3, 0, 1, 2, 4, 5, 8 }, list);
    }

    [Fact]
    public void MergeSort_Descending_SortsCorrectly()
    {
        var list = new List<int> { 5, 1, 4, 2, 8, 0, -3 };

        MergeSortImplementation.Sort(list, asc: false);

        Assert.Equal(new List<int> { 8, 5, 4, 2, 1, 0, -3 }, list);
    }

    [Fact]
    public void MergeSort_WithDuplicates_SortsCorrectly()
    {
        var list = new List<int> { 3, 1, 2, 3, 2, 1, 3 };

        MergeSortImplementation.Sort(list, asc: true);

        Assert.Equal(new List<int> { 1, 1, 2, 2, 3, 3, 3 }, list);
    }

    [Fact]
    public void MergeSort_CustomComparer_SortsByStringLength()
    {
        var list = new List<string> { "aaaa", "b", "ccc", "dd", "" };

        MergeSortImplementation.Sort(list, asc: true, comparer: Comparer<string>.Create(
            (x, y) => (x?.Length ?? 0).CompareTo(y?.Length ?? 0)));

        Assert.Equal(new List<string> { "", "b", "dd", "ccc", "aaaa" }, list);
    }
    
    #region Stability Tests
    [Fact]
    public void MergeSort_IsStable_ForEqualKeys()
    {
        var list = new List<Item>
        {
            new("x", 0),
            new("a", 1),
            new("x", 2),
            new("x", 3),
            new("b", 4),
        };

        MergeSortImplementation.Sort(list, asc: true, comparer: KeyOnlyComparer);

        // All "x" items should remain in original order: 0,2,3
        var xIndices = list.Where(i => i.Key == "x").Select(i => i.OriginalIndex).ToList();
        Assert.Equal(new List<int> { 0, 2, 3 }, xIndices);
    }
    
    [Fact]
    public void MergeSort_CaseInsensitiveComparer_SortsCorrectly()
    {
        var list = new List<string> { "b", "A", "a", "B", "aa", "AA" };

        MergeSortImplementation.Sort(list, asc: true, comparer: StringComparer.OrdinalIgnoreCase);

        Assert.Equal(new List<string> { "A", "a", "aa", "AA", "b", "B" }, list);
    }
    #endregion
}