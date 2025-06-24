public class HasDestination<TContext> : ConditionNode<TContext>
    where TContext : PlayerContext
{
    protected override bool Evaluate(TContext context)
        => context.Blackboard.HasKey("destination");
}