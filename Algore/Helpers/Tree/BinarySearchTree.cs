using Algore.Interfaces;
namespace Algore.Helpers.Tree;

public class BinarySearchTree<T, TKey>(Func<T, TKey> predicate) : IDataStructure<T, TKey>
    where TKey : IComparable<TKey>
{
    private readonly Func<T, TKey> _predicate = predicate;
    private Node<T, TKey>? Root { get; set; }

    private Node<T, TKey>? Traverse(TKey key, Func<T, TKey>? predicate = null)
    {
        predicate ??= _predicate;
        ArgumentNullException.ThrowIfNull(predicate);

        var current = Root;
        while (current != null)
        {
            var comparison = key.CompareTo(predicate(current.Value));
            if (comparison == 0) return current;
            current = comparison < 0 ? current.Left : current.Right;
        }
        
        return null;
    }

    public T? Search(TKey key, Func<T, TKey>? predicate = null)
    {
        predicate ??= _predicate;
        ArgumentNullException.ThrowIfNull(predicate);
        
        var current  = Traverse(key, predicate);
        if (current == null) return default;
        return key.CompareTo(predicate(current.Value)) == 0 ? current.Value : default;
    }

    public void Insert(T element, Func<T, TKey>? predicate = null)
    {
        predicate ??= _predicate;
        ArgumentNullException.ThrowIfNull(predicate);
        
        if (Root == null)
        {
            Root = new Node<T, TKey>
            {
                Value = element
            };
            
            return;
        }

        var key = predicate(element);
        var current = Root;
        while (current != null)
        {
            var comparison = key.CompareTo(predicate(current.Value));
            if (comparison < 0)
            {
                if (current.Left == null)
                {
                    current.Left = new Node<T, TKey>
                    {
                        Value = element
                    };

                    return;
                }

                current = current.Left;
            }
            else
            {
                if (current.Right == null)
                {
                    current.Right = new Node<T, TKey>
                    {
                        Value = element
                    };

                    return;
                }

                current = current.Right;
            }
        }
    }
}
