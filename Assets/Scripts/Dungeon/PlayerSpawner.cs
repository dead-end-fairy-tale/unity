using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] DungeonGenerator dungeonGenerator;
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject playerPrefab;

    private void Awake()
    {
        if (dungeonGenerator == null)
        {
            Debug.LogError("DungeonGenerator 누락");
            return;
        }

        dungeonGenerator.OnCompleted += SpawnPlayer;
    }

    private void OnDestroy()
    {
        dungeonGenerator.OnCompleted -= SpawnPlayer;
    }

    private void SpawnPlayer()
    {
        dungeonGenerator.OnCompleted -= SpawnPlayer;

        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("[PlayerSpawner] Instantiate 완료");
        }
        else
        {
            GameObject existing = GameObject.FindWithTag("Player");
            if (existing != null)
            {
                existing.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
                Debug.Log("[PlayerSpawner] 기존 Player 위치 이동");
            }
            else
            {
                Debug.LogWarning("[PlayerSpawner] 스폰할 플레이어를 찾을 수 없습니다.");
            }
        }

        enabled = false;
    }

}