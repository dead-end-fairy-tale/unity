using UnityEngine;

[RequireComponent(typeof(BT_Player))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator  _animator;
    private BT_Player _bt;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _bt       = GetComponent<BT_Player>();
    }

    private void OnEnable()
    {
        if (_bt != null)
            _bt.Context.Blackboard.OnValueChanged += OnBlackboardStateChanged;
    }

    private void OnDisable()
    {
        if (_bt != null)
            _bt.Context.Blackboard.OnValueChanged -= OnBlackboardStateChanged;
    }

    private void OnBlackboardStateChanged(string key, object val)
    {
        if (key != "state") return;
    
        var state = (PlayerState)val;
        _animator.SetBool(PlayerAnimationHashes.isWalk, state == PlayerState.Move);
    }
}