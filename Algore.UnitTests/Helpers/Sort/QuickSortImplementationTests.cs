using Algore.Helpers.Sort;
using Xunit;

namespace Algore.UnitTests.Helpers.Sort;

public class QuickSortImplementationTests
{
    [Fact]
    public void QuickSort_EmptyList_DoesNotThrow()
    {
        var list = new List<int>();

        QuickSortImplementation.Sort(list, acs: true);

        Assert.Empty(list);
    }

    [Fact]
    public void QuickSort_SingleElement_Unchanged()
    {
        var list = new List<int> { 42 };

        QuickSortImplementation.Sort(list, acs: true);

        Assert.Equal(new List<int> { 42 }, list);
    }

    [Fact]
    public void QuickSort_Ascending_SortsCorrectly()
    {
        var list = new List<int> { 5, 1, 4, 2, 8, 0, -3 };

        QuickSortImplementation.Sort(list, acs: true);

        Assert.Equal(new List<int> { -3, 0, 1, 2, 4, 5, 8 }, list);
    }

    [Fact]
    public void QuickSort_Descending_SortsCorrectly()
    {
        var list = new List<int> { 5, 1, 4, 2, 8, 0, -3 };

        QuickSortImplementation.Sort(list, acs: false);

        Assert.Equal(new List<int> { 8, 5, 4, 2, 1, 0, -3 }, list);
    }

    [Fact]
    public void QuickSort_WithDuplicates_SortsCorrectly()
    {
        var list = new List<int> { 3, 1, 2, 3, 2, 1, 3 };

        QuickSortImplementation.Sort(list, acs: true);

        Assert.Equal(new List<int> { 1, 1, 2, 2, 3, 3, 3 }, list);
    }

    [Fact]
    public void QuickSort_AlreadySortedAscending_RemainsSorted()
    {
        var list = new List<int> { -2, -1, 0, 3, 3, 10 };

        QuickSortImplementation.Sort(list, acs: true);

        Assert.Equal(new List<int> { -2, -1, 0, 3, 3, 10 }, list);
    }

    [Fact]
    public void QuickSort_Strings_SortsLexicographicallyAscending()
    {
        var list = new List<string> { "delta", "alpha", "charlie", "bravo" };

        QuickSortImplementation.Sort(list, acs: true);

        Assert.Equal(new List<string> { "alpha", "bravo", "charlie", "delta" }, list);
    }
}
