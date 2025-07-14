public class BehaviorTree<TContext>
    where TContext : IBehaviorContext
{
    private readonly Node<TContext> _root;
    private readonly TContext _context;

    public BehaviorTree(Node<TContext> root, TContext context)
    {
        _root = root;
        _context = context;
    }

    public void Tick() => _root.Tick(_context);
}