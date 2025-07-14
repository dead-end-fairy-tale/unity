using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class BT_Player : BehaviorTreeAgent<PlayerContext>
{
    private Action<Vector3> _onTapCallback;

    protected override PlayerContext CreateContext()
    {
        var bb = new Blackboard();
        bb.Set("state", PlayerState.Idle);
        return new PlayerContext(this, bb);
    }

    protected override void Awake()
    {
        base.Awake();

        var input = Context.Input;
        if (input == null)
            Debug.LogError("PlayerContext.InputReceiver가 없습니다.");

        _onTapCallback = pos => Context.Blackboard.Set("destination", pos);
        input.OnTapPosition += _onTapCallback;
    }

    protected override void BuildBehaviorTree(BehaviorTreeBuilder<PlayerContext> bt)
    {
        bt.Selector(sel =>
        {
            sel.Sequence(idle =>
            {
                // idle.Action(new SetState<PlayerContext, PlayerState>(PlayerState.Idle));

                idle.Decorator(new AbortIf<PlayerContext>(
                    new HasDestination<PlayerContext>(),
                    new PlayerIdleAction()
                ));
            });

            sel.Sequence(move =>
            {
                move.Condition(new HasDestination<PlayerContext>());

                move.Action(new SetState<PlayerContext, PlayerState>(PlayerState.Move));
                move.Action(new SetNavDestination<PlayerContext>());
                move.Action(new WaitUntilArrived<PlayerContext>());
                move.Action(new RemoveDestination<PlayerContext>());
                move.Action(new SetState<PlayerContext, PlayerState>(PlayerState.Idle));
            });
        });
    }

    protected void OnDestroy()
    {
        if (_onTapCallback != null && Context.Input != null)
            Context.Input.OnTapPosition -= _onTapCallback;
    }
}