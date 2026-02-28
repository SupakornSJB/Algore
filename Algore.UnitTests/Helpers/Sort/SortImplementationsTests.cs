using Algore.Helpers.Sort;
using Xunit;

namespace Algore.UnitTests.Helpers.Sort;

public class SortImplementationsTests 
{
    public class CustomTypeSortable : IComparable<CustomTypeSortable>
    {
        public int Value { get; set; }
        public int CompareTo(CustomTypeSortable? other)
        {
            return Value.CompareTo(other?.Value ?? 0);
        }
    }

    public static IEnumerable<object[]> CustomTypeSorters =>
    [
        new object[] { "QuickSort", (Action<List<CustomTypeSortable>, bool, IComparer<CustomTypeSortable>?>)QuickSortImplementation.Sort },
        new object[] { "MergeSort", (Action<List<CustomTypeSortable>, bool, IComparer<CustomTypeSortable>?>)MergeSortImplementation.Sort },
        new object[] { "HeapSort",  (Action<List<CustomTypeSortable>, bool, IComparer<CustomTypeSortable>?>)HeapSortImplementation.Sort },
    ];
    
    public static IEnumerable<object[]> Sorters =>
    [
        new object[] { "QuickSort", (Action<List<int>, bool, IComparer<int>?>)QuickSortImplementation.Sort },
        new object[] { "MergeSort", (Action<List<int>, bool, IComparer<int>?>)MergeSortImplementation.Sort },
        new object[] { "HeapSort",  (Action<List<int>, bool, IComparer<int>?>)HeapSortImplementation.Sort },
    ];
    
    public static IEnumerable<object[]> StringSorters =>
    [
        new object[] { "QuickSort", (Action<List<string>, bool, IComparer<string>?>)QuickSortImplementation.Sort },
        new object[] { "MergeSort", (Action<List<string>, bool, IComparer<string>?>)MergeSortImplementation.Sort },
        new object[] { "HeapSort",  (Action<List<string>, bool, IComparer<string>?>)HeapSortImplementation.Sort },
    ];

    [Theory]
    [MemberData(nameof(Sorters))]
    public void Sort_EmptyList_DoesNotThrow(string _, Action<List<int>, bool, IComparer<int>?> sort)
    {
        var list = new List<int>();

        sort(list, true, null);

        Assert.Empty(list);
    }

    [Theory]
    [MemberData(nameof(Sorters))]
    public void Sort_SingleElement_Unchanged(string _, Action<List<int>, bool, IComparer<int>?> sort)
    {
        var list = new List<int> { 42 };

        sort(list, true, null);

        Assert.Equal(new List<int> { 42 }, list);
    }

    [Theory]
    [MemberData(nameof(Sorters))]
    public void Sort_Ascending_SortsCorrectly(string _, Action<List<int>, bool, IComparer<int>?> sort)
    {
        var list = new List<int> { 5, 1, 4, 2, 8, 0, -3 };

        sort(list, true, null);

        Assert.Equal(new List<int> { -3, 0, 1, 2, 4, 5, 8 }, list);
    }

    [Theory]
    [MemberData(nameof(Sorters))]
    public void Sort_Descending_SortsCorrectly(string _, Action<List<int>, bool, IComparer<int>?> sort)
    {
        var list = new List<int> { 5, 1, 4, 2, 8, 0, -3 };

        sort(list, false, null);

        Assert.Equal(new List<int> { 8, 5, 4, 2, 1, 0, -3 }, list);
    }

    [Theory]
    [MemberData(nameof(Sorters))]
    public void Sort_WithDuplicates_SortsCorrectly(string _, Action<List<int>, bool, IComparer<int>?> sort)
    {
        var list = new List<int> { 3, 1, 2, 3, 2, 1, 3 };

        sort(list, true, null);

        Assert.Equal(new List<int> { 1, 1, 2, 2, 3, 3, 3 }, list);
    }

    [Theory]
    [MemberData(nameof(Sorters))]
    public void Sort_CustomComparer_SortsByAbsoluteValueAscending(string _, Action<List<int>, bool, IComparer<int>?> sort)
    {
        var list = new List<int> { -10, 3, -2, 7, 0, -1 };

        var absComparer = Comparer<int>.Create((x, y) => Math.Abs(x).CompareTo(Math.Abs(y)));

        sort(list, true, absComparer);

        Assert.Equal(new List<int> { 0, -1, -2, 3, 7, -10 }, list);
    }
    
    [Theory]
    [MemberData(nameof(StringSorters))]
    public void Sort_CustomComparer_SortsByStringLength(string _, Action<List<string>, bool, IComparer<string>?> sort)
    {
        var list = new List<string> { "aaaa", "b", "ccc", "dd", "" };

        var lengthComparer = Comparer<string>.Create(
            (x, y) => (x?.Length ?? 0).CompareTo(y?.Length ?? 0));

        sort(list, true, lengthComparer);

        Assert.Equal(new List<string> { "", "b", "dd", "ccc", "aaaa" }, list);
    }
    
    [Theory]
    [MemberData(nameof(CustomTypeSorters))]
    public void Sort_CustomType_Ascending_SortsCorrectly(
        string _,
        Action<List<CustomTypeSortable>, bool, IComparer<CustomTypeSortable>?> sort)
    {
        var list = new List<CustomTypeSortable>
        {
            new() { Value = 5 },
            new() { Value = 1 },
            new() { Value = 4 },
            new() { Value = 2 },
            new() { Value = 8 },
            new() { Value = 0 },
            new() { Value = -3 },
        };

        sort(list, true, null);

        Assert.Equal(new List<int> { -3, 0, 1, 2, 4, 5, 8 }, list.Select(x => x.Value).ToList());
    }

    [Theory]
    [MemberData(nameof(CustomTypeSorters))]
    public void Sort_CustomType_Descending_SortsCorrectly(
        string _,
        Action<List<CustomTypeSortable>, bool, IComparer<CustomTypeSortable>?> sort)
    {
        var list = new List<CustomTypeSortable>
        {
            new() { Value = 5 },
            new() { Value = 1 },
            new() { Value = 4 },
            new() { Value = 2 },
            new() { Value = 8 },
            new() { Value = 0 },
            new() { Value = -3 },
        };

        sort(list, false, null);

        Assert.Equal(new List<int> { 8, 5, 4, 2, 1, 0, -3 }, list.Select(x => x.Value).ToList());
    }
}
