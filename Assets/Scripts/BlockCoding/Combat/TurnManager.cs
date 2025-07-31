using UnityEngine;
using Cysharp.Threading.Tasks;
using AI;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [Header("Controllers")]
    public BlockCodingController playerController;
    public BlockCodingController aiController;
    public AIController         aiDecider;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartTurn(BlockCodingController caller)
    {
        if (caller != playerController) return;
        StartTurnAsync().Forget();
    }

    public async UniTaskVoid StartTurnAsync()
    {
        var playerCmds = playerController.GetExecCommands();
        var aiCmds     = aiDecider.DecideCommands();

        int maxCount = Mathf.Max(playerCmds.Count, aiCmds.Count);
        for (int i = 0; i < maxCount; i++)
        {
            if (i < playerCmds.Count && !IsDead(playerController))
                await playerCmds[i].ExecuteAsync();

            if (i < aiCmds.Count && !IsDead(aiController))
                await aiCmds[i].ExecuteAsync();

            if (IsDead(playerController) || IsDead(aiController))
                break;

            await UniTask.Delay(300);
        }

        playerController.ClearAllCommands();
        aiController.ClearAllCommands();
        // TODO: 승패 처리 UI 호출
    }

    private bool IsDead(BlockCodingController ctrl)
        => ctrl.combatSystem == null || ctrl.combatSystem.IsDead();
}