namespace Algore.DataStructures.Tree.Base;

public abstract class BinarySearchTreeBase<T, TKey, TNode>(Func<T, TKey> predicate)
    where TKey : IComparable<TKey>
    where TNode : TreeNodeBase<T, TNode>
{
    protected readonly Func<T, TKey> Predicate = predicate;
    protected TNode? Root { get; set; }

    protected abstract TKey GetKey(TNode node);
    protected abstract TNode? GetLeft(TNode node);
    protected abstract TNode? GetRight(TNode node);
    protected abstract void SetLeft(TNode node, TNode? child);
    protected abstract void SetRight(TNode node, TNode? child);

    private TNode? Traverse(TKey key, Func<T, TKey>? predicate = null)
    {
        predicate ??= Predicate;
        ArgumentNullException.ThrowIfNull(predicate);

        var current = Root;
        while (current != null)
        {
            var comparison = key.CompareTo(GetKey(current));
            if (comparison == 0) return current;

            current = comparison < 0 ? GetLeft(current) : GetRight(current);
        }

        return null;
    }

    public T? Search(TKey key, Func<T, TKey>? predicate = null)
    {
        predicate ??= Predicate;
        ArgumentNullException.ThrowIfNull(predicate);

        var current = Traverse(key, predicate);
        return current == null ? default : GetValue(current);
    }

    protected abstract T GetValue(TNode node);
    protected abstract TNode CreateNode(T element);
    protected abstract void InsertFixup(TNode node);

    public void Insert(T element, Func<T, TKey>? predicate = null)
    {
        predicate ??= Predicate;
        ArgumentNullException.ThrowIfNull(predicate);

        var newNode = CreateNode(element);
        var key = predicate(element);

        TNode? parent = null;
        var current = Root;

        while (current != null)
        {
            parent = current;
            var comparison = key.CompareTo(GetKey(current));
            current = comparison < 0 ? GetLeft(current) : GetRight(current);
        }

        if (parent == null)
        {
            Root = newNode;
        }
        else if (key.CompareTo(GetKey(parent)) < 0)
        {
            SetLeft(parent, newNode);
        }
        else
        {
            SetRight(parent, newNode);
        }

        InsertFixup(newNode);
    }
    
    // Edge-based height 
    public int Height => GetHeight(Root);

    private static int GetHeight(TreeNodeBase<T, TNode>? node)
    {
        if (node == null) return -1;        
        
        var leftHeight = GetHeight(node.Left);
        var rightHeight = GetHeight(node.Right); 
        
        return Math.Max(leftHeight, rightHeight) + 1;
    }
}
