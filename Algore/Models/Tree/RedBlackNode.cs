using Algore.DataStructures.Tree;
using Algore.DataStructures.Tree.Base;
using Algore.Enums.Tree;

namespace Algore.Models.Tree;

public sealed class RedBlackNode<T> : TreeNodeBase<T, RedBlackNode<T>>
{
    public NodeColor Color { get; set; } = NodeColor.Red;
    public RedBlackNode<T>? Parent { get; set; }
}