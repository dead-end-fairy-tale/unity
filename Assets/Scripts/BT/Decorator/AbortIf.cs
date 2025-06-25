public class AbortIf<TContext> : DecoratorNode<TContext>
    where TContext : IBehaviorContext
{
    private readonly ConditionNode<TContext> _condition;

    public AbortIf(ConditionNode<TContext> condition, Node<TContext> child)
        : base(child)
    {
        _condition = condition;
    }

    public override NodeState Tick(TContext context)
    {
        var condState = _condition.Tick(context);

        if (condState == NodeState.Success)
            return _state = NodeState.Failure;

        return _state = _child.Tick(context);
    }
}