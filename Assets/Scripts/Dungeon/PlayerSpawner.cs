using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    public static event Action<Transform> OnPlayerSpawned;

    [SerializeField] private DungeonGenerator dungeonGenerator;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        if (dungeonGenerator == null)
        {
            Debug.LogError("PlayerSpawner: DungeonGenerator 누락");
            return;
        }

        dungeonGenerator.OnCompleted += SpawnPlayer;
    }

    private void OnDestroy()
    {
        if (dungeonGenerator != null)
            dungeonGenerator.OnCompleted -= SpawnPlayer;
    }

    private void SpawnPlayer()
    {
        dungeonGenerator.OnCompleted -= SpawnPlayer;

        Transform spawnedTransform = null;
        GameObject player;

        if (playerPrefab != null)
        {
            player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            spawnedTransform = player.transform;
            
            Debug.Log("[PlayerSpawner] Instantiate 완료");
        }
        else
        {
            player = GameObject.FindWithTag("Player");
            
            if (player != null)
            {
                player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
                spawnedTransform = player.transform;
                
                Debug.Log("[PlayerSpawner] 기존 Player 위치 이동");
            }
            else
            {
                Debug.LogWarning("[PlayerSpawner] 스폰할 플레이어를 찾을 수 없습니다.");
            }
        }
        
        var dungeonScene = SceneManager.GetSceneByName("DungeonScene");
        if (dungeonScene.IsValid())
            SceneManager.MoveGameObjectToScene(player, dungeonScene);

        if (spawnedTransform != null)
            OnPlayerSpawned?.Invoke(spawnedTransform);

        enabled = false;
    }
}