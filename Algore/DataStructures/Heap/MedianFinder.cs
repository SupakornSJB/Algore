namespace Algore.DataStructures.Heap;

public class MedianFinder
{
    private readonly PriorityQueue<int, int> _high = new();
    private readonly PriorityQueue<int, int> _low = new(Comparer<int>.Create((a, b) => b.CompareTo(a)));
    private readonly Dictionary<int, int> _delayed = [];
    private readonly Dictionary<int, int> _counts = [];

    private int _lowSize;
    private int _highSize;

    public int Count => _lowSize + _highSize;

    public void Insert(int value)
    {
        if (_lowSize == 0)
        {
            _low.Enqueue(value, value);
            _lowSize++;
            IncrementCount(value);
            return;
        }

        PruneLow();
        var lowTop = _low.Peek();

        if (value <= lowTop)
        {
            _low.Enqueue(value, value);
            _lowSize++;
        }
        else
        {
            _high.Enqueue(value, value);
            _highSize++;
        }

        IncrementCount(value);
        Rebalance();
    }

    public bool Delete(int value)
    {
        if (!_counts.TryGetValue(value, out var currentCount) || currentCount == 0)
        {
            return false;
        }

        PruneLow();
        PruneHigh();

        var removeFromLow = _lowSize > 0 && value <= _low.Peek();

        if (currentCount == 1)
        {
            _counts.Remove(value);
        }
        else
        {
            _counts[value] = currentCount - 1;
        }

        _delayed[value] = _delayed.GetValueOrDefault(value) + 1;

        if (removeFromLow)
        {
            _lowSize--;
            if (_low.Count > 0 && _low.Peek() == value)
            {
                PruneLow();
            }
        }
        else
        {
            _highSize--;
            if (_high.Count > 0 && _high.Peek() == value)
            {
                PruneHigh();
            }
        }

        Rebalance();
        return true;
    }

    public double FindMedian()
    {
        if (Count == 0)
        {
            throw new InvalidOperationException("Cannot find median of an empty data structure.");
        }

        PruneLow();
        PruneHigh();

        if (_lowSize > _highSize)
        {
            return _low.Peek();
        }

        return (_low.Peek() + _high.Peek()) / 2.0;
    }

    private void Rebalance()
    {
        PruneLow();
        PruneHigh();

        if (_lowSize > _highSize + 1)
        {
            var moved = _low.Dequeue();
            _high.Enqueue(moved, moved);
            _lowSize--;
            _highSize++;
            PruneLow();
        }
        else if (_lowSize < _highSize)
        {
            var moved = _high.Dequeue();
            _low.Enqueue(moved, moved);
            _highSize--;
            _lowSize++;
            PruneHigh();
        }
    }

    private void PruneLow()
    {
        while (_low.Count > 0 && _delayed.TryGetValue(_low.Peek(), out var deleteCount) && deleteCount > 0)
        {
            var value = _low.Dequeue();
            if (deleteCount == 1)
            {
                _delayed.Remove(value);
            }
            else
            {
                _delayed[value] = deleteCount - 1;
            }
        }
    }

    private void PruneHigh()
    {
        while (_high.Count > 0 && _delayed.TryGetValue(_high.Peek(), out var deleteCount) && deleteCount > 0)
        {
            var value = _high.Dequeue();
            if (deleteCount == 1)
            {
                _delayed.Remove(value);
            }
            else
            {
                _delayed[value] = deleteCount - 1;
            }
        }
    }

    private void IncrementCount(int value)
    {
        _counts[value] = _counts.GetValueOrDefault(value) + 1;
    }
}
