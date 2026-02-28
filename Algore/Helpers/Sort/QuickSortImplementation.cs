using Algore.Interfaces;

namespace Algore.Helpers.Sort;

public abstract class QuickSortImplementation : ISortImplementation
{
    public static void Sort<T>(
        List<T> list, 
        bool acs = true, 
        IComparer<T>? comparer = null) where T : IComparable<T>
    {
        QuickSortImpl(list, 0, list.Count - 1, acs, comparer);
    }
    
    private static void QuickSortImpl<T>(
        List<T> list, 
        int left, 
        int right, 
        bool asc, 
        IComparer<T>? comparer = null) where T : IComparable<T>
    {
        if (left >= right) return;
        
        var pivotPoint = Partition(list, left, right, asc, comparer);
        QuickSortImpl(list, left, pivotPoint - 1, asc, comparer);
        QuickSortImpl(list, pivotPoint + 1, right, asc, comparer);
    }

    private static int Partition<T>(
        List<T> list, 
        int left, 
        int right, 
        bool asc, 
        IComparer<T>? comparer = null) where T : IComparable<T>
    {
        if (left >= right) return left;
        var i = left - 1;
        var k = left;
        var pivot = list[right];

        while (k != right)
        {
            var comparison = comparer?.Compare(list[k], pivot) ?? list[k].CompareTo(pivot);
            if (asc ? comparison <= 0 : comparison >= 0)
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
        (list[left], list[right]) = (list[right], list[left]);
    }
}