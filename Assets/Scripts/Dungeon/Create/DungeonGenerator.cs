using System;
using System.Collections.Generic;
using UnityEngine;
using Dungeon;
using Unity.AI.Navigation;

public class DungeonGenerator : MonoBehaviour
{
    [Header("맵 사이즈 및 시작 좌표")]
    public Vector2Int Size;
    public Vector2Int StartCoord = Vector2Int.zero;

    [Header("방 개수 제어")]
    [Range(0f, 1f)] public float CoverageRatio = 0.5f;
    public int TargetRoomCount = 0;

    [Header("룸 프리팹과 룰 배열")]
    public RoomRule[] Rules;

    [Header("룸 배치 오프셋")]
    public Vector2 Offset;

    [Header("보스방 설정")]
    public GameObject BossRoomPrefab;
    
    public event Action<List<RoomData>> OnDataGenerated;
    public event Action OnCompleted;

    private NavMeshSurface navMeshSurface;
    private Cell[,] _board;

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
            _board = boardMgr.Board;

            int total = Size.x * Size.y;
            int maxVisits = (TargetRoomCount > 0)
                ? Mathf.Clamp(TargetRoomCount, 1, total)
                : Mathf.Clamp(Mathf.RoundToInt(total * CoverageRatio), 1, total);

            var mazeGen = new MazeGenerator(_board, Size, maxVisits);
            mazeGen.Carve(StartCoord);

            var bossCoord = BossRoomLocator.FindFarthest(_board, StartCoord, Size);

            var roomDatas = new List<RoomData>();
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    var cell = _board[x, y];
                    
                    if (!cell.Visited)
                        continue;

                    bool isBoss = (x == bossCoord.x && y == bossCoord.y);
                    int ruleIdx = ChooseRule(x, y);
                    GameObject prefab = isBoss ? BossRoomPrefab : Rules[ruleIdx].RoomPrefab;
                    bool[] conns = FilterConnections(cell, x, y);

                    roomDatas.Add(new RoomData(new Vector2Int(x, y), prefab, conns));
                }
            }

            OnDataGenerated?.Invoke(roomDatas);

            if (navMeshSurface != null)
            {
                navMeshSurface.BuildNavMesh();
                Debug.Log("[DungeonGenerator] NavMesh 빌드 완료");
            }
            else
            {
                Debug.LogWarning("[DungeonGenerator] NavMeshSurface 미할당, 런타임 베이크 생략");
            }

            OnCompleted?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[DungeonGenerator] 오류: {ex.GetType().Name} – {ex.Message}");
        }
    }

    private bool[] FilterConnections(Cell cell, int x, int y)
    {
        var copy = (bool[])cell.Connections.Clone();
        
        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            var n = dir switch
            {
                Direction.Up    => new Vector2Int(x, y + 1),
                Direction.Down  => new Vector2Int(x, y - 1),
                Direction.Left  => new Vector2Int(x - 1, y),
                Direction.Right => new Vector2Int(x + 1, y),
                _               => new Vector2Int(-1, -1)
            };

            bool valid = n.x >= 0 && n.x < Size.x && n.y >= 0 && n.y < Size.y && _board[n.x, n.y].Visited;
            
            if (!valid)
                copy[(int)dir] = false;
        }
        return copy;
    }

    private int ChooseRule(int x, int y)
    {
        var list = new List<int>();
        
        for (int i = 0; i < Rules.Length; i++)
        {
            var status = Rules[i].GetSpawnStatus(x, y);
            
            if (status == SpawnStatus.Required)
                return i;
            
            if (status == SpawnStatus.Possible)
                list.Add(i);
        }
        return (list.Count > 0) ? UnityEngine.Random.Range(0, list.Count) : 0;
    }
}
