using Algore.Interfaces;

namespace Algore.Helpers.Sort;

public abstract class HeapSortImplementation : ISortImplementation
{
    public static void Sort<T>(
        List<T> list, 
        bool asc = true,
        IComparer<T>? comparer = null) where T : IComparable<T>
    {
        BuildHeap(list, asc, comparer);
        for (var i = list.Count - 1; i > 0; i--)
        {
            Swap(list, 0, i);    
            MinMaxHeapify(list, 0, i, asc, comparer);
        }
    }

    private static void MinMaxHeapify<T>(
        List<T> list, 
        int rootIndex, 
        int untilIndex, 
        bool isMax,
        IComparer<T>? comparer = null) where T : IComparable<T>
    {
        var leftIndex = 2 * rootIndex + 1;
        var rightIndex = 2 * rootIndex + 2;
        
        if (leftIndex >= untilIndex && rightIndex >= untilIndex) return;
        if (leftIndex >= untilIndex) leftIndex = rootIndex;
        if (rightIndex >= untilIndex) rightIndex = rootIndex;
        
        var left = list[leftIndex];
        var right = list[rightIndex];

        var bestIndex = rootIndex;
        
        var leftComparison = comparer?.Compare(left, list[bestIndex]) ?? left.CompareTo(list[bestIndex]);
        var shouldSwapLeft = isMax ? leftComparison > 0 : leftComparison < 0;
        if (shouldSwapLeft && leftIndex <= untilIndex)
        {
            bestIndex = leftIndex;
        }
        
        var rightComparison = comparer?.Compare(right, list[bestIndex]) ?? right.CompareTo(list[bestIndex]);
        var shouldSwapRight = isMax ? rightComparison > 0 : rightComparison < 0;
        if (shouldSwapRight && rightIndex <= untilIndex)
        {
            bestIndex = rightIndex;
        }

        if (bestIndex == rootIndex) return;
        Swap(list, rootIndex, bestIndex);
        MinMaxHeapify(list, bestIndex, untilIndex, isMax, comparer);
    }

    private static void BuildHeap<T>(
        List<T> list, 
        bool isMax, 
        IComparer<T>? comparer = null) where T : IComparable<T>
    {
        var starting = (list.Count / 2) - 1; 
        for (var i = starting; i >= 0; i--)
        {
            MinMaxHeapify(list, i, list.Count, isMax, comparer);
        }
    }

    private static void Swap<T>(List<T> list, int left, int right)
    {
        (list[left], list[right]) = (list[right], list[left]);
    }
}