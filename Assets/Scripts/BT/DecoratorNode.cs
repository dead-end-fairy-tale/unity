public abstract class DecoratorNode<TContext> : Node<TContext>
    where TContext : IBehaviorContext
{
    protected readonly Node<TContext> _child;
    protected DecoratorNode(Node<TContext> child) => _child = child;
}