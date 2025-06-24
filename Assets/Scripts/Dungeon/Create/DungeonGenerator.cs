using System;
using UnityEngine;
using Dungeon;
using Unity.AI.Navigation;

public class DungeonGenerator : MonoBehaviour
{
    [Header("맵 사이즈 및 시작 좌표")]
    public Vector2Int Size;
    [Tooltip("시작 셀 좌표")]
    public Vector2Int StartCoord = Vector2Int.zero;

    [Header("방 개수 제어")]
    [Range(0f, 1f)]
    [Tooltip("전체 셀 대비 방문 비율")]
    public float CoverageRatio = 0.5f;
    [Tooltip("절대 방 개수 (0이면 CoverageRatio 사용)")]
    public int TargetRoomCount = 0;

    [Header("룸 프리팹과 룰 배열")]
    public RoomRule[] Rules;

    [Header("룸 배치 오프셋")]
    public Vector2 Offset;

    [Header("보스방 설정")]
    public GameObject BossRoomPrefab;
    
    public event Action OnCompleted;
    
    private NavMeshSurface navMeshSurface;

    private void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
    }

    private void Start()
    {
        try
        {
            var boardMgr = new BoardManager(Size);
            boardMgr.Initialize();

            int totalCells = Size.x * Size.y;
            int maxVisits = TargetRoomCount > 0
                ? Mathf.Clamp(TargetRoomCount, 1, totalCells)
                : Mathf.Clamp(Mathf.RoundToInt(totalCells * CoverageRatio), 1, totalCells);

            var mazeGen = new MazeGenerator(boardMgr.Board, Size, maxVisits);
            mazeGen.Carve(StartCoord);

            var bossCoord = BossRoomLocator.FindFarthest(boardMgr.Board, StartCoord, Size);
            var placer = new RoomPlacer(boardMgr.Board, Size, Offset, Rules, BossRoomPrefab, transform);
            placer.PlaceAll(bossCoord);

            if (navMeshSurface != null)
            {
                navMeshSurface.BuildNavMesh();
                Debug.Log("[DungeonGenerator] NavMesh 빌드 완료");
            }
            else
            {
                Debug.LogWarning("[DungeonGenerator] NavMeshSurface가 할당되지 않아 런타임 베이크를 건너뜁니다.");
            }

            OnCompleted?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[DungeonGenerator] 오류 발생: {ex.GetType().Name} – {ex.Message}");
        }
    }
}
