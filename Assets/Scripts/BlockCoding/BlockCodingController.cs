using System.Collections.Generic;
using UnityEngine;
using BlockCoding;
using Movement;
using Combat;

public class BlockCodingController : MonoBehaviour
{
    [Header("UI Manager")]
    public UI.BlockUIManager uiManager;

    [Header("Combat System")]
    public CombatSystem combatSystem;

    [Header("Movement System")]
    public MovementSystem movementSystem;

    [Header("이 컨트롤러가 플레이어용인가?")]
    public bool isPlayerController = false;

    private List<IBlockCommand> _uiCommands   = new List<IBlockCommand>();
    private List<IBlockCommand> _execCommands = new List<IBlockCommand>();

    private bool _isLooping      = false;
    private int  _loopStartIndex = 0;
    private int  _loopCount      = 0;

    void Start()
    {
        if (!isPlayerController) 
            return;

        // 1) 플레이어만 블록 추가 이벤트 구독
        uiManager.OnAddCommand += HandleAddCommand;
    }

    void OnDestroy()
    {
        if (!isPlayerController) 
            return;

        uiManager.OnAddCommand -= HandleAddCommand;
    }

    private void HandleAddCommand(CommandType type)
    {
        IBlockCommand cmd = type switch
        {
            CommandType.Attack      => new AttackCommand(combatSystem),
            CommandType.Defend      => new DefendCommand(combatSystem),
            CommandType.Move        => new MoveCommand(movementSystem),
            CommandType.RotateLeft  => new RotateCommand(movementSystem, -movementSystem.rotationAngle),
            CommandType.RotateRight => new RotateCommand(movementSystem,  movementSystem.rotationAngle),
            CommandType.LoopStart   => StartLoop(),
            CommandType.Loop        => _isLooping ? EndLoop() : null,
            _                       => null
        };
        if (cmd == null) return;

        _uiCommands.Add(cmd);
        if (cmd.Type != CommandType.LoopStart)
            _execCommands.Add(cmd);

        uiManager.Refresh(_uiCommands);
    }

    private IBlockCommand StartLoop()
    {
        _isLooping      = true;
        _loopStartIndex = _execCommands.Count;
        _loopCount      = 1;
        return new LoopStartCommand();
    }

    private IBlockCommand EndLoop()
    {
        int count = _execCommands.Count - _loopStartIndex;
        var inner = _execCommands.GetRange(_loopStartIndex, count);
        _execCommands.RemoveRange(_loopStartIndex, count);

        _isLooping = false;
        var loopCmd = new LoopCommand(inner, _loopCount);
        _execCommands.Add(loopCmd);
        return loopCmd;
    }

    public List<IBlockCommand> GetExecCommands()
        => new List<IBlockCommand>(_execCommands);

    public void ClearAllCommands()
    {
        _execCommands.Clear();
        _uiCommands.Clear();
        _isLooping = false;
        uiManager.Refresh(_uiCommands);
    }
}
