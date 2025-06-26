using UnityEngine;

public class SetNavDestination<TContext> : ActionNode<TContext>
    where TContext : PlayerContext
{
    protected override NodeState Execute(TContext context)
    {
        var dest = context.Blackboard.Get<Vector3>("destination");
        context.Agent.SetDestination(dest);
        return NodeState.Success;
    }
}