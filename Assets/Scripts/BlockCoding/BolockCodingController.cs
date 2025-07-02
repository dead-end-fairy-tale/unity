using System.Collections;
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

    private bool _isLooping = false;
    private int  _loopStartIndex = 0;
    private int  _loopCount = 0;

    private void Start()
    {
        uiManager.OnAddCommand += HandleAddCommand;
        uiManager.OnExecute    += HandleExecute;
    }

    private void OnDestroy()
    {
        uiManager.OnAddCommand -= HandleAddCommand;
        uiManager.OnExecute    -= HandleExecute;
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
                cmd = new RotateCommand(movementSystem, movementSystem.rotationAngle);
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
        _isLooping = true;
        _loopStartIndex = _execCommands.Count;
        _loopCount = 1;
        return new LoopStartCommand();
    }

    private IBlockCommand EndLoop()
    {
        int count = _execCommands.Count - _loopStartIndex;
        List<IBlockCommand> innerCommands = _execCommands.GetRange(_loopStartIndex, count);
        _execCommands.RemoveRange(_loopStartIndex, count);

        LoopCommand loopCmd = new LoopCommand(innerCommands, _loopCount);
        _execCommands.Add(loopCmd);
        _isLooping = false;
        return loopCmd;
    }

    private void HandleExecute()
    {
        if (_execCommands.Count == 0)
            return;

        StartCoroutine(RunCommands());
    }

    private IEnumerator RunCommands()
    {
        foreach (var cmd in _execCommands)
        {
            yield return cmd.Execute();
            yield return new WaitForSecondsRealtime(0.5f);
        }

        _execCommands.Clear();
        _uiCommands.Clear();
        _isLooping = false;
        uiManager.Refresh(_uiCommands);
    }
}
