using System.Collections.Generic;

public class StateCondition<TContext, TState> : ConditionNode<TContext>
    where TContext : IBehaviorContext
{
    private readonly string _key;
    private readonly TState _expected;

    public StateCondition(TState expected, string key = "state")
    {
        _expected = expected;
        _key      = key;
    }

    protected override bool Evaluate(TContext ctx)
    {
        var actual = ctx.Blackboard.Get<TState>(_key);
        return EqualityComparer<TState>.Default.Equals(actual, _expected);
    }
}