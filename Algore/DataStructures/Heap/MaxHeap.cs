namespace Algore.DataStructures.Heap;

public sealed class MaxHeap<TElement, TKey>(Func<TElement, TKey>? predicate = null)
    : BinaryHeapBase<TElement, TKey>(predicate)
    where TKey : IComparable<TKey>
{
    protected override bool HasHigherPriority(TKey left, TKey right)
    {
        return left.CompareTo(right) > 0;
    }
}
