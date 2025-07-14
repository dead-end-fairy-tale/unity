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

    private List<IBlockCommand> _uiCommands   = new List<IBlockCommand>();
    private List<IBlockCommand> _execCommands = new List<IBlockCommand>();

    private bool _isLooping     = false;
    private int  _loopStartIndex = 0;
    private int  _loopCount     = 0;

    private void Start()
    {
        uiManager.OnAddCommand += HandleAddCommand;
        uiManager.OnExecute    += () => TurnManager.Instance.StartTurn(this);
    }

    private void OnDestroy()
    {
        uiManager.OnAddCommand -= HandleAddCommand;
        uiManager.OnExecute    -= () => TurnManager.Instance.StartTurn(this);
    }

    private void HandleAddCommand(CommandType type)
    {
        IBlockCommand cmd = null;

        switch (type)
        {
            case CommandType.Attack:
                cmd = new AttackCommand(combatSystem);
                break;

            case CommandType.Defend:
                cmd = new DefendCommand(combatSystem);
                break;

            case CommandType.Move:
                cmd = new MoveCommand(movementSystem);
                break;

            case CommandType.RotateLeft:
                cmd = new RotateCommand(movementSystem, -movementSystem.rotationAngle);
                break;

            case CommandType.RotateRight:
                cmd = new RotateCommand(movementSystem,  movementSystem.rotationAngle);
                break;

            case CommandType.LoopStart:
                cmd = StartLoop();
                break;

            case CommandType.Loop:
                if (_isLooping)
                    cmd = EndLoop();
                break;
        }

        if (cmd == null)
            return;

        _uiCommands.Add(cmd);
        if (cmd.Type != CommandType.LoopStart)
            _execCommands.Add(cmd);

        uiManager.Refresh(_uiCommands);
    }

    private IBlockCommand StartLoop()
    {
        _isLooping       = true;
        _loopStartIndex  = _execCommands.Count;
        _loopCount       = 1;
        return new LoopStartCommand();
    }

    private IBlockCommand EndLoop()
    {
        int count = _execCommands.Count - _loopStartIndex;
        var inner = _execCommands.GetRange(_loopStartIndex, count);
        _execCommands.RemoveRange(_loopStartIndex, count);

        var loopCmd = new LoopCommand(inner, _loopCount);
        _execCommands.Add(loopCmd);
        _isLooping = false;
        return loopCmd;
    }

    public List<IBlockCommand> GetExecCommands()
    {
        return new List<IBlockCommand>(_execCommands);
    }

    public void ClearAllCommands()
    {
        _execCommands.Clear();
        _uiCommands.Clear();
        _isLooping = false;
        uiManager.Refresh(_uiCommands);
    }
}
