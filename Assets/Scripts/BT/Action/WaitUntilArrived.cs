public class WaitUntilArrived<TContext> : ActionNode<TContext>
    where TContext : PlayerContext
{
    protected override NodeState Execute(TContext context)
    {
        var agent = context.Agent;
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            return NodeState.Success;
        return NodeState.Running;
    }
}