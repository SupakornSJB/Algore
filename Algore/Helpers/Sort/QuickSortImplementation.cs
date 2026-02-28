namespace Algore.Helpers.Sort;

public static class QuickSortImplementation
{
    public static void QuickSort<T>(List<T> list, bool acs = true) where T : IComparable
    {
        QuickSortImpl(list, 0, list.Count - 1, acs);
    }
    
    private static void QuickSortImpl<T>(List<T> list, int left, int right, bool asc) where T : IComparable
    {
        if (left >= right) return;
        
        var pivotPoint = Partition(list, left, right, asc);
        QuickSortImpl(list, left, pivotPoint - 1, asc);
        QuickSortImpl(list, pivotPoint + 1, right, asc);
    }

    private static int Partition<T>(List<T> list, int left, int right, bool asc) where T : IComparable
    {
        if (left >= right) return left;
        var i = left - 1;
        var k = left;
        var pivot = list[right];

        while (k != right)
        {
            if (asc && list[k].CompareTo(pivot) <= 0)
            {
                Swap(list, i+1, k);
                i++;
            }
            
            if (!asc && list[k].CompareTo(pivot) >= 0)
            {
                Swap(list, i+1, k);
                i++;
            }
        
            k++;
        }
        
        Swap(list, i+1, right);
        return i+1;
    }

    private static void Swap<T>(List<T> list, int left, int right)
    {
        var temp = list[left];
        list[left] = list[right];
        list[right] = temp;
    }
}