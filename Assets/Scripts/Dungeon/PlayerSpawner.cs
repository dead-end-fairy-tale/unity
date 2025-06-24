using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField, Tooltip("던전 생성기 (OnGenerationComplete 이벤트를 구독)")]
    private DungeonGenerator dungeonGenerator;

    [SerializeField, Tooltip("스폰될 위치의 Transform")]
    private Transform spawnPoint;

    [SerializeField, Tooltip("플레이어 Prefab (씬에 미리 배치된 경우 null 허용)")]
    private GameObject playerPrefab;

    private async UniTaskVoid Start()
    {
        if (dungeonGenerator == null)
        {
            Debug.LogError("[PlayerSpawner] DungeonGenerator가 할당되지 않았습니다.");
            return;
        }

        await WaitForGenerationComplete();
        SpawnPlayer();
    }

    private UniTask WaitForGenerationComplete()
    {
        var tcs = new UniTaskCompletionSource();

        void OnDone()
        {
            dungeonGenerator.OnGenerationComplete -= OnDone;
            tcs.TrySetResult();
        }

        dungeonGenerator.OnGenerationComplete += OnDone;
        return tcs.Task;
    }

    private void SpawnPlayer()
    {
        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            var existing = GameObject.FindWithTag("Player");
            if (existing != null)
            {
                existing.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                Debug.LogWarning("[PlayerSpawner] 스폰할 플레이어 오브젝트를 찾을 수 없습니다.");
            }
        }
    }
}