using UnityEngine;
using Cysharp.Threading.Tasks;
using BlockCoding;
using AI;
using System.Collections.Generic;
using System.Threading;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [Header("플레이어 블록 코딩 컨트롤러")]
    public BlockCodingController playerController;

    [Header("적 AI 컨트롤러들")]
    public List<AIController> enemyControllers;

    private List<IBlockCommand> _battleCommands;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void OnEnable()
    {
        if (playerController?.uiManager != null)
            playerController.uiManager.OnExecute += StartBattle;
    }

    void OnDisable()
    {
        if (playerController?.uiManager != null)
            playerController.uiManager.OnExecute -= StartBattle;
    }

    public void StartBattle()
    {
        _battleCommands = playerController.GetExecCommands();

        Debug.Log($"[TurnManager] 전투용 명령 개수: {_battleCommands.Count}");

        playerController.ClearAllCommands();
        
        var token = this.GetCancellationTokenOnDestroy();
        NextRoundAsync(token).AttachExternalCancellation(token).Forget();
    }

    async UniTask NextRoundAsync(CancellationToken token)
    {
        for (int i = 0; i < _battleCommands.Count; i++)
        {
            int round = i + 1;
            Debug.Log($"------ 라운드 {round} ------");

            Debug.Log($"플레이어 명령 {round} 시작");
            if (token.IsCancellationRequested)
                break;
            await _battleCommands[i].ExecuteAsync().AttachExternalCancellation(token);
            Debug.Log($"플레이어 명령 {round} 끝");

            for (int j = 0; j < enemyControllers.Count; j++)
            {
                var enemy = enemyControllers[j];
                var cmd   = enemy.DecideNextCommand();

                Debug.Log($"적 {j + 1} ({enemy.name}) 시작");
                if (cmd != null) await cmd.ExecuteAsync();
                Debug.Log($"적 {j + 1} ({enemy.name}) 끝");
            }

            await UniTask.Delay(200);
        }

        Debug.Log("전투 종료!");
    }
}
