using Algore.Interfaces;

namespace Algore.DataStructures.HashTable;

public class HashTableLinearProbing<TElement, TKey>(int capacity = 16, Func<TElement, TKey>? predicate = null)
    : IDataStructure<TElement, TKey>
    where TKey : IComparable<TKey>
{
    private sealed class Entry
    {
        public required TKey Key { get; init; }
        public required TElement Value { get; set; }
        public bool IsDeleted { get; set; }
    }

    private readonly Func<TElement, TKey>? _defaultPredicate = predicate;
    private Entry?[] _entries = new Entry?[Math.Max(4, capacity)];
    private int _count;
    private int _totalProbeLength;
    private int _totalCollisions;

    public TElement? Search(TKey key, Func<TElement, TKey>? predicate = null)
    {
        predicate ??= _defaultPredicate;
        ArgumentNullException.ThrowIfNull(predicate);

        var index = FindIndex(key);
        return index < 0 ? default : _entries[index]!.Value;
    }

    public void Insert(TElement element, Func<TElement, TKey>? predicate = null)
    {
        predicate ??= _defaultPredicate;
        ArgumentNullException.ThrowIfNull(predicate);

        var key = predicate(element);
        InsertInternal(key, element);
    }

    private void InsertInternal(TKey key, TElement element, int loadFactor = 0, int totalProbeCount = 0)
    {
        var hash = GetStartIndex(key);
        var firstDeletedIndex = -1;

        for (var i = loadFactor == 0 ? 0 : capacity * (int) Math.Pow(2, loadFactor - 1); 
             i < capacity * Math.Pow(2, loadFactor); i++)
        {
            var index = (hash + i) % (capacity * (int) Math.Pow(2, loadFactor));
            var entry = _entries[index];

            if (entry == null)
            {
                var targetIndex = firstDeletedIndex >= 0 ? firstDeletedIndex : index;
                _entries[targetIndex] = new Entry
                {
                    Key = key,
                    Value = element,
                    IsDeleted = false
                };

                if (i > 0)
                {
                    _totalCollisions++;
                }
                
                _count++;
                _totalProbeLength += totalProbeCount;

                return;
            }

            if (entry.IsDeleted)
            {
                if (firstDeletedIndex < 0)
                {
                    firstDeletedIndex = index;
                }

                totalProbeCount++;
                continue;
            }

            if (entry.Key.CompareTo(key) == 0)
            {
                entry.Value = element;
                return;
            }

            totalProbeCount++;
        }

        if (firstDeletedIndex >= 0)
        {
            _entries[firstDeletedIndex] = new Entry
            {
                Key = key,
                Value = element,
                IsDeleted = false
            };
            _count++;
            _totalProbeLength += totalProbeCount;

            return;
        }

        if (capacity * Math.Pow(2, loadFactor + 1) > _entries.Length)
        {
            Array.Resize(ref _entries, _entries.Length * 2);
        }
        
        InsertInternal(key, element, loadFactor + 1, totalProbeCount);
    }

    private int FindIndex(TKey key, int loadFactor = 0)
    {
        var hash = GetStartIndex(key);

        for (var i =  loadFactor == 0 ? 0 : capacity * (int) Math.Pow(2, loadFactor - 1); 
             i < capacity * Math.Pow(2, loadFactor); i++)
        {
            var index = (hash + i) % (capacity * (int) Math.Pow(2, loadFactor));
            var entry = _entries[index];

            if (entry == null)
            {
                return -1;
            }

            if (!entry.IsDeleted && entry.Key.CompareTo(key) == 0)
            {
                return index;
            }
        }

        if (capacity * Math.Pow(2, loadFactor + 1) > _entries.Length)
        {
            return -1;
        }

        return FindIndex(key, loadFactor + 1);
    }

    private int GetStartIndex(TKey key)
    {
        return (key.GetHashCode() & int.MaxValue) % capacity;
    }

    public bool Remove(TKey key)
    {
        var index = FindIndex(key);
        if (index < 0)
        {
            return false;
        }

        _entries[index]!.IsDeleted = true;
        _count--;
        return true;
    }
    
    public double GetTotalCollisions() => _totalCollisions;
    
    public double GetAverageCollisionsPerInsertion() => _count > 0 ? (double)_totalCollisions / _count : 0;
    
    public double GetTotalProbeLength() => _totalProbeLength;

    public double GetAverageProbeLength() => _count > 0 ? (double)_totalProbeLength / _count : 0;

    public double GetAverageChainLength()
    {
        if (_count == 0)
        {
            return 0;
        }

        var totalChainLength = 0;
        var chainCount = 0;

        for (var i = 0; i < _entries.Length; i++)
        {
            if (_entries[i] == null || _entries[i]!.IsDeleted)
            {
                continue;
            }

            var chainLength = 0;

            while (i < _entries.Length && _entries[i] != null && !_entries[i]!.IsDeleted)
            {
                chainLength++;
                i++;
            }

            if (chainLength > 0)
            {
                totalChainLength += chainLength;
                chainCount++;
            }

            i--;
        }

        return chainCount > 0 ? (double)totalChainLength / chainCount : 0;
    }
}
