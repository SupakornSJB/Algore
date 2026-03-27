using Algore.Interfaces;

namespace Algore.DataStructures.Heap;

public abstract class BinaryHeapBase<TElement, TKey>(Func<TElement, TKey>? predicate = null)
    : IDataStructure<TElement, TKey>
    where TKey : IComparable<TKey>
{
    private readonly List<TElement> _heap = [];
    private Func<TElement, TKey>? _structurePredicate = predicate;

    public int Count => _heap.Count;

    public TElement? Peek()
    {
        return _heap.Count == 0 ? default : _heap[0];
    }

    public TElement? ExtractRoot()
    {
        if (_heap.Count == 0)
        {
            return default;
        }

        if (_heap.Count == 1)
        {
            var onlyItem = _heap[0];
            _heap.RemoveAt(0);
            return onlyItem;
        }

        var predicate = ResolveStructurePredicate();
        var root = _heap[0];
        _heap[0] = _heap[^1];
        _heap.RemoveAt(_heap.Count - 1);
        SiftDown(0, predicate);
        return root;
    }

    public TElement? Search(TKey key, Func<TElement, TKey>? predicate = null)
    {
        predicate ??= _structurePredicate;
        ArgumentNullException.ThrowIfNull(predicate);

        foreach (var item in _heap)
        {
            if (predicate(item).CompareTo(key) == 0)
            {
                return item;
            }
        }

        return default;
    }

    public void Insert(TElement element, Func<TElement, TKey>? predicate = null)
    {
        var structurePredicate = ResolveStructurePredicate(predicate);
        _heap.Add(element);
        SiftUp(_heap.Count - 1, structurePredicate);
    }

    protected abstract bool HasHigherPriority(TKey left, TKey right);

    private Func<TElement, TKey> ResolveStructurePredicate(Func<TElement, TKey>? predicate = null)
    {
        if (predicate is not null)
        {
            if (_structurePredicate is null)
            {
                _structurePredicate = predicate;
            }
            else if (!ReferenceEquals(_structurePredicate, predicate))
            {
                throw new InvalidOperationException("Heap key selector cannot change after initialization.");
            }
        }

        ArgumentNullException.ThrowIfNull(_structurePredicate);
        return _structurePredicate;
    }

    private void SiftUp(int index, Func<TElement, TKey> predicate)
    {
        while (index > 0)
        {
            var parentIndex = (index - 1) / 2;
            var childKey = predicate(_heap[index]);
            var parentKey = predicate(_heap[parentIndex]);

            if (!HasHigherPriority(childKey, parentKey))
            {
                break;
            }

            (_heap[parentIndex], _heap[index]) = (_heap[index], _heap[parentIndex]);
            index = parentIndex;
        }
    }

    private void SiftDown(int index, Func<TElement, TKey> predicate)
    {
        while (true)
        {
            var left = 2 * index + 1;
            var right = left + 1;
            var candidate = index;

            if (left < _heap.Count &&
                HasHigherPriority(predicate(_heap[left]), predicate(_heap[candidate])))
            {
                candidate = left;
            }

            if (right < _heap.Count &&
                HasHigherPriority(predicate(_heap[right]), predicate(_heap[candidate])))
            {
                candidate = right;
            }

            if (candidate == index)
            {
                break;
            }

            (_heap[index], _heap[candidate]) = (_heap[candidate], _heap[index]);
            index = candidate;
        }
    }
}
