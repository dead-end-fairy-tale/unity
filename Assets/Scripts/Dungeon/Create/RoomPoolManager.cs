using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Unity.AI.Navigation;
using Dungeon;    // RoomData, RoomBehaviour, Direction

[RequireComponent(typeof(DungeonGenerator))]
public class RoomPoolManager : MonoBehaviour
{
    [Header("풀 사이즈 (현재 방 + 문으로 연결된 이웃)")]
    [SerializeField] private int poolSize = 5;

    [Header("Rooms 컨테이너")]
    [SerializeField] private Transform roomsParent;

    [Header("NavMesh Surface")]
    [SerializeField] private NavMeshSurface navMeshSurface;

    private DungeonGenerator dungeonGen;
    private CameraController cameraCtrl;

    private Queue<GameObject> roomPool = new Queue<GameObject>();
    private Dictionary<Vector2Int, GameObject> activeRooms = new Dictionary<Vector2Int, GameObject>();
    private List<RoomData> roomDatas;

    private void Awake()
    {
        dungeonGen   = GetComponent<DungeonGenerator>();
        cameraCtrl   = FindObjectOfType<CameraController>();

        if (roomsParent == null)
            Debug.LogError("[RoomPoolManager] roomsParent를 Inspector에 할당하세요.");
        if (navMeshSurface == null)
            navMeshSurface = FindObjectOfType<NavMeshSurface>();
        if (navMeshSurface == null)
            Debug.LogError("[RoomPoolManager] NavMeshSurface를 할당하거나 씬에 배치하세요.");
    }

    private void OnEnable()
    {
        dungeonGen.OnDataGenerated += HandleDataGenerated;
        cameraCtrl.OnRoomChanged   += HandleRoomChanged;
    }

    private void OnDisable()
    {
        dungeonGen.OnDataGenerated -= HandleDataGenerated;
        cameraCtrl.OnRoomChanged   -= HandleRoomChanged;
    }

    private void HandleDataGenerated(List<RoomData> datas)
    {
        roomDatas = datas;
        InitializePool();
    }

    private void InitializePool()
    {
        roomPool.Clear();
        activeRooms.Clear();

        Vector2Int startIdx = Vector2Int.zero;
        var field = typeof(CameraController)
            .GetField("prevRoomIndex", BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null)
            startIdx = (Vector2Int)field.GetValue(cameraCtrl);

        var toShow = GetReachableIndices(startIdx);
        SpawnRooms(toShow);

        navMeshSurface.BuildNavMesh();
    }

    private void HandleRoomChanged(Vector2Int oldIdx, Vector2Int newIdx)
    {
        var toShow = GetReachableIndices(newIdx);

        foreach (var kv in new Dictionary<Vector2Int, GameObject>(activeRooms))
        {
            if (!toShow.Contains(kv.Key))
            {
                kv.Value.SetActive(false);
                roomPool.Enqueue(kv.Value);
                activeRooms.Remove(kv.Key);
            }
        }

        SpawnRooms(toShow);

        navMeshSurface.BuildNavMesh();
    }

    private List<Vector2Int> GetReachableIndices(Vector2Int center)
    {
        var list = new List<Vector2Int> { center };
        var centerData = roomDatas.Find(d => d.Index == center);
        if (centerData == null)
            return list;
        
        if (centerData.Connections[(int)Direction.Up])
            list.Add(center + Vector2Int.up);
        
        if (centerData.Connections[(int)Direction.Down])
            list.Add(center + Vector2Int.down);
        
        if (centerData.Connections[(int)Direction.Left])
            list.Add(center + Vector2Int.left);
        
        if (centerData.Connections[(int)Direction.Right])
            list.Add(center + Vector2Int.right);

        return list;
    }

    private void SpawnRooms(List<Vector2Int> indices)
    {
        foreach (var idx in indices)
        {
            if (activeRooms.ContainsKey(idx)) continue;

            var data = roomDatas.Find(d => d.Index == idx);
            if (data == null) continue;

            GameObject go;
            if (roomPool.Count > 0)
            {
                go = roomPool.Dequeue();
                go.SetActive(true);
                go.transform.SetParent(roomsParent, false);
                go.transform.position = GetWorldPos(idx);
            }
            else
            {
                go = Instantiate(
                    data.Prefab,
                    GetWorldPos(idx),
                    Quaternion.identity,
                    roomsParent
                );
            }

            go.name = $"Room_{idx.x}_{idx.y}";
            go.GetComponent<RoomBehaviour>()?.UpdateRoom(data.Connections);
            activeRooms[idx] = go;
        }
    }

    private Vector3 GetWorldPos(Vector2Int idx)
    {
        var off = dungeonGen.Offset;
        return new Vector3(idx.x * off.x, 0f, idx.y * off.y);
    }
}
