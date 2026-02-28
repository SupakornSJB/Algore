namespace Algore.Helpers.Sort;

public static class MergeSortImplementation
{
    public static void MergeSort<T>(
        List<T> list, 
        bool asc,
        IComparer<T>? comparer = null) where T : IComparable
    {
        var temp = new T[list.Count];
        MergeSortImpl(list, temp, 0, list.Count - 1, asc, comparer);
    }

    private static void MergeSortImpl<T>(
        List<T> list, 
        T[] temp, 
        int left, 
        int right, 
        bool asc,
        IComparer<T>? comparer = null) where T : IComparable
    {
        if (left >= right)
        {
            return;
        }
        
        var halfIndex = left + (right - left)/ 2;
        MergeSortImpl(list, temp, left, halfIndex, asc, comparer);
        MergeSortImpl(list, temp, halfIndex + 1, right, asc, comparer);
        
        Merge(list, temp, left, halfIndex, right, asc, comparer);
    }

    private static void Merge<T>(
        List<T> list, 
        T[] temp, 
        int left, 
        int mid, 
        int right, 
        bool asc,
        IComparer<T>? comparer = null) where T : IComparable
    {
        var i = left;
        var j = mid + 1;
        var k = left;

        while (i <= mid && j <= right)
        {
            var comparison = comparer?.Compare(list[i], list[j]) ?? list[i].CompareTo(list[j]);
            var takeLeft = asc ? (comparison <= 0) : (comparison >= 0);
            temp[k++] = takeLeft ? list[i++] : list[j++];
        }

        while (i <= mid)
        {
            temp[k++] = list[i++];
        }

        while (j <= right)
        {
            temp[k++] = list[j++];
        }

        for (var idx = left; idx <= right; idx++)
        {
            list[idx] = temp[idx];  
        }
    }
}