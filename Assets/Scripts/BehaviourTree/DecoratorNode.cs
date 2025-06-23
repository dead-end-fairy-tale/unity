public abstract class DecoratorNode : Node
{
    protected readonly Node _child;

    protected DecoratorNode(Node child)
    {
        _child = child;
    }
}