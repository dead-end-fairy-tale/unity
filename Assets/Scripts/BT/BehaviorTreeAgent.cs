using UnityEngine;

public abstract class BehaviorTreeAgent<TContext> : MonoBehaviour
    where TContext : IBehaviorContext
{
    public TContext Context;
    
    protected BehaviorTree<TContext> Tree;

    protected virtual void Awake()
    {
        Context = CreateContext();
        Tree = new BehaviorTreeBuilder<TContext>(Context)
            .Do(BuildBehaviorTree)
            .Build();
    }

    protected virtual void Update()
        => Tree.Tick();

    protected abstract TContext CreateContext();
    protected abstract void BuildBehaviorTree(BehaviorTreeBuilder<TContext> builder);
}