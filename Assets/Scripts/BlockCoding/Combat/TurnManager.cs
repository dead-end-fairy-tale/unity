using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using BlockCoding;
using AI;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

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

        // 1) TurnManager 파괴 시
        var turnToken = this.GetCancellationTokenOnDestroy();

        // 2) 각 적 오브젝트 파괴 시
        var enemyTokens = enemyControllers
            .Where(e => e != null)
            .Select(e => e.GetCancellationTokenOnDestroy());

        // 3) 두 토큰을 묶은 Linked CTS 생성
        var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
            new[] { turnToken }.Concat(enemyTokens).ToArray()
        );

        // 4) 연결된 토큰을 이용해 라운드 실행
        NextRoundAsync(linkedCts.Token)
            .AttachExternalCancellation(linkedCts.Token)
            .Forget();
    }

    async UniTask NextRoundAsync(CancellationToken token)
    {
        try
        {
            for (int i = 0; i < _battleCommands.Count; i++)
            {
                int round = i + 1;
                Debug.Log($"------ 라운드 {round} ------");

                // 플레이어 명령
                Debug.Log($"플레이어 명령 {round} 시작");
                if (token.IsCancellationRequested) break;
                await _battleCommands[i]
                    .ExecuteAsync()
                    .AttachExternalCancellation(token);
                Debug.Log($"플레이어 명령 {round} 끝");

                // 적 명령
                for (int j = 0; j < enemyControllers.Count; j++)
                {
                    var enemy = enemyControllers[j];
                    if (enemy == null)   // 이미 파괴된 적은 건너뛰기
                        continue;

                    var cmd = enemy.DecideNextCommand();
                    Debug.Log($"적 {j + 1} ({enemy.name}) 시작");
                    if (cmd != null)
                        await cmd
                            .ExecuteAsync()
                            .AttachExternalCancellation(token);
                    Debug.Log($"적 {j + 1} ({enemy.name}) 끝");
                }

                await UniTask
                    .Delay(200)
                    .AttachExternalCancellation(token);
            }
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("[TurnManager] 전투가 취소되었습니다.");
        }
        finally
        {
            Debug.Log("전투 종료!");
        }
    }
}
