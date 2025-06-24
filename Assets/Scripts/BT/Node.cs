public abstract class Node<TContext>
    where TContext : IBehaviorContext
{
    protected NodeState _state;
    public NodeState State => _state;
    public abstract NodeState Tick(TContext context);
}