public abstract class ActionNode<TContext> : LeafNode<TContext>
    where TContext : IBehaviorContext
{
    protected abstract NodeState Execute(TContext context);
    public sealed override NodeState Tick(TContext context)
    {
        _state = Execute(context);
        return _state;
    }
}