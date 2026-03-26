using Algore.DataStructures.Tree.Base;
using Algore.Interfaces;
using Algore.Models.Tree;

namespace Algore.DataStructures.Tree.Implementations;

public class BinarySearchTree<T, TKey>(Func<T, TKey> predicate)
    : BinarySearchTreeBase<T, TKey, BstNode<T>>(predicate),
      IDataStructure<T, TKey>
    where TKey : IComparable<TKey>
{
    protected override TKey GetKey(BstNode<T> node) => Predicate(node.Value);
    protected override BstNode<T>? GetLeft(BstNode<T> node) => node.Left;
    protected override BstNode<T>? GetRight(BstNode<T> node) => node.Right;
    protected override void SetLeft(BstNode<T> node, BstNode<T>? child) => node.Left = child;
    protected override void SetRight(BstNode<T> node, BstNode<T>? child) => node.Right = child;
    protected override T GetValue(BstNode<T> node) => node.Value;
    protected override BstNode<T> CreateNode(T element) => new() { Value = element };

    protected override void InsertFixup(BstNode<T> node)
    {
        // Plain BST: no balancing needed.
    }
}
