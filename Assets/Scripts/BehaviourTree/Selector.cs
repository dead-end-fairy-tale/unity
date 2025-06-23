public class Selector : CompositeNode
{
    public Selector(params Node[] children) : base(children) { }

    public override NodeState Tick(Blackboard bb)
    {
        foreach (var child in _children)
        {
            var result = child.Tick(bb);
            if (result == NodeState.Running || result == NodeState.Success)
                return _state = result;
        }
        return _state = NodeState.Failure;
    }
}