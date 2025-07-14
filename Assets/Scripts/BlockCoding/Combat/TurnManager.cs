// Scripts/Turn/TurnManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockCoding;
using AI;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [Header("Controllers")]
    public BlockCodingController playerController;
    public BlockCodingController aiController;
    public AIController        aiDecider;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>Execute 버튼 클릭 시 호출됩니다.</summary>
    public void StartTurn(BlockCodingController caller)
    {
        if (caller != playerController) return;

        var playerCmds = playerController.GetExecCommands();
        var aiCmds     = aiDecider.DecideCommands();

        StartCoroutine(RunTurnSequence(playerCmds, aiCmds));
    }

    private IEnumerator RunTurnSequence(
        List<IBlockCommand> playerCmds,
        List<IBlockCommand> aiCmds)
    {
        int maxCount = Mathf.Max(playerCmds.Count, aiCmds.Count);

        for (int i = 0; i < maxCount; i++)
        {
            if (i < playerCmds.Count && !IsDead(playerController))
                yield return playerCmds[i].Execute();

            if (i < aiCmds.Count && !IsDead(aiController))
                yield return aiCmds[i].Execute();

            if (IsDead(playerController) || IsDead(aiController))
                break;

            yield return new WaitForSecondsRealtime(0.3f);
        }

        playerController.ClearAllCommands();
        aiController.ClearAllCommands();
        // TODO: 승패 처리 UI 호출
    }

    private bool IsDead(BlockCodingController ctrl)
        => ctrl.combatSystem == null || ctrl.combatSystem.IsDead();
}