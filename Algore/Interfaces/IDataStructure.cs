namespace Algore.Interfaces;

public interface IDataStructure<TElement, in TKey> where TKey : IComparable<TKey>
{
    public TElement? Search(TKey key, Func<TElement, TKey>? predicate = null);
    public void Insert(TElement element, Func<TElement, TKey>? predicate = null);
}