public class RemoveDestination<TContext> : ActionNode<TContext>
    where TContext : IBehaviorContext
{
    protected override NodeState Execute(TContext ctx)
    {
        ctx.Blackboard.Remove("destination");
        return NodeState.Success;
    }
}