using Algore.DataStructures.Tree.Base;
using Algore.Enums.Tree;
using Algore.Interfaces;
using Algore.Models.Tree;

namespace Algore.DataStructures.Tree.Implementations;

public class RedBlackTree<T, TKey>(Func<T, TKey> predicate)
    : BinarySearchTreeBase<T, TKey, RedBlackNode<T>>(predicate),
      IDataStructure<T, TKey>
    where TKey : IComparable<TKey>
{
    protected override TKey GetKey(RedBlackNode<T> node) => Predicate(node.Value);
    protected override RedBlackNode<T>? GetLeft(RedBlackNode<T> node) => node.Left;
    protected override RedBlackNode<T>? GetRight(RedBlackNode<T> node) => node.Right;
    protected override void SetLeft(RedBlackNode<T> node, RedBlackNode<T>? child)
    {
        node.Left = child;
        if (child != null) child.Parent = node;
    }

    protected override void SetRight(RedBlackNode<T> node, RedBlackNode<T>? child)
    {
        node.Right = child;
        if (child != null) child.Parent = node;
    }

    protected override T GetValue(RedBlackNode<T> node) => node.Value;
    protected override RedBlackNode<T> CreateNode(T element) => new() { Value = element };

    protected override void InsertFixup(RedBlackNode<T> node)
    {
        while (node.Parent?.Color == NodeColor.Red)
        {
            var parent = node.Parent;
            var grandParent = parent.Parent;

            if (grandParent == null) break;

            if (parent == grandParent.Left)
            {
                var uncle = grandParent.Right;

                if (uncle?.Color == NodeColor.Red)
                {
                    parent.Color = NodeColor.Black;
                    uncle.Color = NodeColor.Black;
                    grandParent.Color = NodeColor.Red;
                    node = grandParent;
                }
                else
                {
                    if (node == parent.Right)
                    {
                        node = parent;
                        RotateLeft(node);
                        parent = node.Parent;
                        grandParent = parent?.Parent;
                    }

                    if (parent != null && grandParent != null)
                    {
                        parent.Color = NodeColor.Black;
                        grandParent.Color = NodeColor.Red;
                        RotateRight(grandParent);
                    }
                }
            }
            else
            {
                var uncle = grandParent.Left;

                if (uncle?.Color == NodeColor.Red)
                {
                    parent.Color = NodeColor.Black;
                    uncle.Color = NodeColor.Black;
                    grandParent.Color = NodeColor.Red;
                    node = grandParent;
                }
                else
                {
                    if (node == parent.Left)
                    {
                        node = parent;
                        RotateRight(node);
                        parent = node.Parent;
                        grandParent = parent?.Parent;
                    }

                    if (parent != null && grandParent != null)
                    {
                        parent.Color = NodeColor.Black;
                        grandParent.Color = NodeColor.Red;
                        RotateLeft(grandParent);
                    }
                }
            }
        }

        if (Root != null)
        {
            Root.Color = NodeColor.Black;
        }
    }

    private void RotateLeft(RedBlackNode<T> node)
    {
        var pivot = node.Right;
        if (pivot == null) return;

        node.Right = pivot.Left;
        if (pivot.Left != null)
        {
            pivot.Left.Parent = node;
        }

        pivot.Parent = node.Parent;

        if (node.Parent == null)
        {
            Root = pivot;
        }
        else if (node == node.Parent.Left)
        {
            node.Parent.Left = pivot;
        }
        else
        {
            node.Parent.Right = pivot;
        }

        pivot.Left = node;
        node.Parent = pivot;
    }

    private void RotateRight(RedBlackNode<T> node)
    {
        var pivot = node.Left;
        if (pivot == null) return;

        node.Left = pivot.Right;
        if (pivot.Right != null)
        {
            pivot.Right.Parent = node;
        }

        pivot.Parent = node.Parent;

        if (node.Parent == null)
        {
            Root = pivot;
        }
        else if (node == node.Parent.Right)
        {
            node.Parent.Right = pivot;
        }
        else
        {
            node.Parent.Left = pivot;
        }

        pivot.Right = node;
        node.Parent = pivot;
    }
}
