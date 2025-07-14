public class PlayerIdleAction : ActionNode<PlayerContext>
{
    protected override NodeState Execute(PlayerContext context)
    {
        return NodeState.Running;
    }
}