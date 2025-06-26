public abstract class ConditionNode<TContext> : LeafNode<TContext>
    where TContext : IBehaviorContext
{
    protected abstract bool Evaluate(TContext context);
    public sealed override NodeState Tick(TContext context)
    {
        _state = Evaluate(context) ? NodeState.Success : NodeState.Failure;
        return _state;
    }
}