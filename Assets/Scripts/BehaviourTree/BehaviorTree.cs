public class BehaviorTree
{
    private readonly Node      _root;
    public          Blackboard Blackboard { get; }

    public BehaviorTree(Node root, Blackboard bb)
    {
        _root      = root;
        Blackboard = bb;
    }

    public void Tick() => _root.Tick(Blackboard);
}