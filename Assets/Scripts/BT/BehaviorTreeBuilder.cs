using System;
using System.Collections.Generic;

public class BehaviorTreeBuilder<TContext>
    where TContext : IBehaviorContext
{
    private Node<TContext> _root;
    private readonly TContext _context;
    private readonly List<Node<TContext>> _children = new List<Node<TContext>>();

    public BehaviorTreeBuilder(TContext context)
        => _context = context;

    public BehaviorTreeBuilder<TContext> Do(Action<BehaviorTreeBuilder<TContext>> build)
    {
        var builder = new BehaviorTreeBuilder<TContext>(_context);
        build(builder);
        var node = builder._root ?? (Node<TContext>)new Sequence<TContext>(builder._children.ToArray());
        _children.Add(node);
        return this;
    }

    public BehaviorTreeBuilder<TContext> Sequence(Action<BehaviorTreeBuilder<TContext>> build)
        => Do(build);

    public BehaviorTreeBuilder<TContext> Selector(Action<BehaviorTreeBuilder<TContext>> build)
    {
        var builder = new BehaviorTreeBuilder<TContext>(_context);
        build(builder);
        _children.Add(new Selector<TContext>(builder._children.ToArray()));
        return this;
    }

    public BehaviorTreeBuilder<TContext> Condition(ConditionNode<TContext> node)
    {
        _children.Add(node);
        return this;
    }

    public BehaviorTreeBuilder<TContext> Action(ActionNode<TContext> node)
    {
        _children.Add(node);
        return this;
    }

    public BehaviorTreeBuilder<TContext> Decorator(DecoratorNode<TContext> node)
    {
        _children.Add(node);
        return this;
    }

    public BehaviorTree<TContext> Build()
    {
        var root = _root ?? (Node<TContext>)new Sequence<TContext>(_children.ToArray());
        return new BehaviorTree<TContext>(root, _context);
    }
}