using System.Collections.Generic;

public abstract class CompositeNode<TContext> : Node<TContext>
    where TContext : IBehaviorContext
{
    protected readonly List<Node<TContext>> _children = new List<Node<TContext>>();

    protected CompositeNode(params Node<TContext>[] children)
        => _children.AddRange(children);
}