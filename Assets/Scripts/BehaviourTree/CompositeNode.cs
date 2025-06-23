using System.Collections.Generic;

public abstract class CompositeNode : Node
{
    protected readonly List<Node> _children = new List<Node>();

    protected CompositeNode(params Node[] children)
    {
        _children.AddRange(children);
    }
}