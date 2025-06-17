// DungeonGenerator.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("맵 사이즈 및 시작 좌표")]
    public Vector2Int Size;
    [Tooltip("시작 셀 좌표 (x, y)")]
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
    [Tooltip("보스룸으로 사용할 프리팹")]
    public GameObject BossRoomPrefab;

    private Cell[,] board;
    private int maxVisits;

    void Start()
    {
        InitializeBoard();
        CalculateMaxVisits();
        CarveMaze();
        PlaceRooms();
    }

    private void InitializeBoard()
    {
        board = new Cell[Size.x, Size.y];
        for (int x = 0; x < Size.x; x++)
            for (int y = 0; y < Size.y; y++)
                board[x, y] = new Cell();
    }

    private void CalculateMaxVisits()
    {
        int total = Size.x * Size.y;
        maxVisits = (TargetRoomCount > 0)
            ? Mathf.Clamp(TargetRoomCount, 1, total)
            : Mathf.Clamp(Mathf.RoundToInt(total * CoverageRatio), 1, total);
    }

    private void CarveMaze()
    {
        var stack = new Stack<Vector2Int>();
        Vector2Int current = StartCoord;
        int visited = 0;

        while (visited < maxVisits && (stack.Count > 0 || HasUnvisitedNeighbors(current)))
        {
            if (!board[current.x, current.y].Visited)
            {
                board[current.x, current.y].Visited = true;
                visited++;
            }

            var neighbors = GetUnvisitedNeighbors(current);
            if (neighbors.Count == 0)
                current = stack.Pop();
            else
            {
                stack.Push(current);
                Vector2Int next = neighbors[UnityEngine.Random.Range(0, neighbors.Count)];
                ConnectCells(current, next);
                current = next;
            }
        }
    }

    /// <summary>
    /// StartCoord에서 BFS 탐색으로 가장 멀리 떨어진 **방문된** 셀 좌표를 반환
    /// </summary>
    private Vector2Int FindBossRoomCoord()
    {
        var queue = new Queue<Vector2Int>();
        var dist  = new Dictionary<Vector2Int,int>();

        queue.Enqueue(StartCoord);
        dist[StartCoord] = 0;

        Vector2Int farthest = StartCoord;
        int maxDist = 0;

        while (queue.Count > 0)
        {
            var cur = queue.Dequeue();
            int cd = dist[cur];
            if (cd > maxDist)
            {
                maxDist = cd;
                farthest = cur;
            }

            // 연결된 이웃으로만 확장, 그리고 그 셀도 반드시 Visited 여야 함
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (!board[cur.x, cur.y].Connections[(int)dir])
                    continue;

                Vector2Int next = dir switch
                {
                    Direction.Up    => new Vector2Int(cur.x, cur.y + 1),
                    Direction.Down  => new Vector2Int(cur.x, cur.y - 1),
                    Direction.Left  => new Vector2Int(cur.x - 1, cur.y),
                    Direction.Right => new Vector2Int(cur.x + 1, cur.y),
                    _               => cur
                };

                // 방문된 셀만 추적
                if (!board[next.x, next.y].Visited)
                    continue;

                if (!dist.ContainsKey(next))
                {
                    dist[next] = cd + 1;
                    queue.Enqueue(next);
                }
            }
        }

        return farthest;
    }

    private void PlaceRooms()
    {
        // 가장 먼 '방문된' 셀을 보스방으로 지정
        Vector2Int bossCoord = FindBossRoomCoord();
        Debug.Log($"BossRoom at {bossCoord}");

        for (int x = 0; x < Size.x; x++)
        for (int y = 0; y < Size.y; y++)
        {
            var cell = board[x, y];
            if (!cell.Visited) continue;

            bool isBoss = (x == bossCoord.x && y == bossCoord.y);

            // 연결 정보 복사 및 필터링
            var filtered = (bool[])cell.Connections.Clone();
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                Vector2Int neighbor = dir switch
                {
                    Direction.Up    => new Vector2Int(x, y + 1),
                    Direction.Down  => new Vector2Int(x, y - 1),
                    Direction.Left  => new Vector2Int(x - 1, y),
                    Direction.Right => new Vector2Int(x + 1, y),
                    _               => throw new ArgumentOutOfRangeException()
                };

                bool hasNeighbor =
                    neighbor.x >= 0 && neighbor.x < Size.x &&
                    neighbor.y >= 0 && neighbor.y < Size.y &&
                    board[neighbor.x, neighbor.y].Visited;

                if (!hasNeighbor)
                    filtered[(int)dir] = false;
            }

            // 보스방이면 BossRoomPrefab, 아니면 룰에 따라 일반 프리팹
            GameObject prefab = isBoss
                ? BossRoomPrefab
                : Rules[ChooseRule(x, y)].RoomPrefab;

            var roomObj = Instantiate(
                prefab,
                new Vector3(x * Offset.x, 0f, y * Offset.y),
                Quaternion.identity,
                transform
            ).GetComponent<RoomBehaviour>();

            // 문·벽 상태 업데이트 (보스방도 포함)
            roomObj.UpdateRoom(filtered);
            roomObj.name = isBoss ? "BossRoom" : $"Room_{x}_{y}";
        }
    }

    private void ConnectCells(Vector2Int aPos, Vector2Int bPos)
    {
        Direction dirToB = GetDirection(aPos, bPos);
        Direction dirToA = GetOpposite(dirToB);

        board[aPos.x, aPos.y].Connections[(int)dirToB] = true;
        board[bPos.x, bPos.y].Connections[(int)dirToA] = true;
    }

    private Direction GetDirection(Vector2Int from, Vector2Int to)
    {
        Vector2Int d = to - from;
        if (d == Vector2Int.up)    return Direction.Up;
        if (d == Vector2Int.down)  return Direction.Down;
        if (d == Vector2Int.left)  return Direction.Left;
        if (d == Vector2Int.right) return Direction.Right;
        throw new ArgumentException($"Invalid neighbor diff: {d}");
    }

    private Direction GetOpposite(Direction dir) => dir switch
    {
        Direction.Up    => Direction.Down,
        Direction.Down  => Direction.Up,
        Direction.Left  => Direction.Right,
        Direction.Right => Direction.Left,
        _               => throw new ArgumentException($"Unknown dir: {dir}")
    };

    private bool HasUnvisitedNeighbors(Vector2Int pos)
        => GetUnvisitedNeighbors(pos).Count > 0;

    private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int pos)
    {
        var list = new List<Vector2Int>();
        int x = pos.x, y = pos.y;

        if (y + 1 < Size.y && !board[x, y + 1].Visited)
            list.Add(new Vector2Int(x, y + 1));
        if (y - 1 >= 0     && !board[x, y - 1].Visited)
            list.Add(new Vector2Int(x, y - 1));
        if (x + 1 < Size.x && !board[x + 1, y].Visited)
            list.Add(new Vector2Int(x + 1, y));
        if (x - 1 >= 0     && !board[x - 1, y].Visited)
            list.Add(new Vector2Int(x - 1, y));

        return list;
    }

    private int ChooseRule(int x, int y)
    {
        var candidates = new List<int>();
        for (int i = 0; i < Rules.Length; i++)
        {
            var status = Rules[i].GetSpawnStatus(x, y);
            if (status == SpawnStatus.Required)
                return i;
            if (status == SpawnStatus.Possible)
                candidates.Add(i);
        }
        return (candidates.Count > 0)
            ? candidates[UnityEngine.Random.Range(0, candidates.Count)]
            : 0;
    }
}
