public class SetState<TContext, TState> : ActionNode<TContext>
    where TContext : IBehaviorContext
{
    private readonly string _key;
    private readonly TState _newState;

    public SetState(TState newState, string key = "state")
    {
        _newState = newState;
        _key      = key;
    }

    protected override NodeState Execute(TContext ctx)
    {
        ctx.Blackboard.Set(_key, _newState);
        return NodeState.Success;
    }
}