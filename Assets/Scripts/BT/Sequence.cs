public class Sequence<TContext> : CompositeNode<TContext>
    where TContext : IBehaviorContext
{
    public Sequence(params Node<TContext>[] children) : base(children) { }

    public override NodeState Tick(TContext context)
    {
        foreach (var child in _children)
        {
            var result = child.Tick(context);
            if (result == NodeState.Running || result == NodeState.Failure)
                return _state = result;
        }
        return _state = NodeState.Success;
    }
}