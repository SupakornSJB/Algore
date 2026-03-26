namespace Algore.DataStructures.Tree.Base;

public abstract class TreeNodeBase<T, TNode> 
    where TNode: TreeNodeBase<T, TNode>
{
    public required T Value { get; init; }
    public TNode? Left { get; set; }
    public TNode? Right { get; set; } 
}