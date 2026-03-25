namespace Algore.Interfaces;

public class Node<T, TKey> where TKey : IComparable<TKey>
{
    public Node<T, TKey>? Left { get; set; }
    public Node<T, TKey>? Right { get; set; }
    public required T Value { get; set; }
}