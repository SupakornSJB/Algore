using Xunit;

namespace Algore.UnitTests.Helpers.Sort;

public class MergeSortImplementationTests
{
    [Fact]
    public void MergeSort_EmptyList_DoesNotThrow()
    {
        var list = new List<int>();

        Algore.Helpers.Sort.MergeSortImplementation.MergeSort(list, asc: true);

        Assert.Empty(list);
    }

    [Fact]
    public void MergeSort_SingleElement_Unchanged()
    {
        var list = new List<int> { 42 };

        Algore.Helpers.Sort.MergeSortImplementation.MergeSort(list, asc: true);

        Assert.Equal(new List<int> { 42 }, list);
    }

    [Fact]
    public void MergeSort_Ascending_SortsCorrectly()
    {
        var list = new List<int> { 5, 1, 4, 2, 8, 0, -3 };

        Algore.Helpers.Sort.MergeSortImplementation.MergeSort(list, asc: true);

        Assert.Equal(new List<int> { -3, 0, 1, 2, 4, 5, 8 }, list);
    }

    [Fact]
    public void MergeSort_Descending_SortsCorrectly()
    {
        var list = new List<int> { 5, 1, 4, 2, 8, 0, -3 };

        Algore.Helpers.Sort.MergeSortImplementation.MergeSort(list, asc: false);

        Assert.Equal(new List<int> { 8, 5, 4, 2, 1, 0, -3 }, list);
    }

    [Fact]
    public void MergeSort_WithDuplicates_SortsCorrectly()
    {
        var list = new List<int> { 3, 1, 2, 3, 2, 1, 3 };

        Algore.Helpers.Sort.MergeSortImplementation.MergeSort(list, asc: true);

        Assert.Equal(new List<int> { 1, 1, 2, 2, 3, 3, 3 }, list);
    }

    [Fact]
    public void MergeSort_CustomComparer_SortsByStringLength()
    {
        var list = new List<string> { "aaaa", "b", "ccc", "dd", "" };

        Algore.Helpers.Sort.MergeSortImplementation.MergeSort(list, asc: true, comparer: Comparer<string>.Create(
            (x, y) => (x?.Length ?? 0).CompareTo(y?.Length ?? 0)));

        Assert.Equal(new List<string> { "", "b", "dd", "ccc", "aaaa" }, list);
    }
}