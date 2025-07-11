using UnityEngine;

public class RoomVisibilityController : MonoBehaviour
{
    [Header("던전 생성 완료 이벤트를 제공하는 DungeonGenerator")]
    [SerializeField] private DungeonGenerator dungeonGenerator;

    [Header("던전의 방들이 모두 자식으로 붙어 있는 부모 Transform")]
    [SerializeField] private Transform dungeonParent;

    [Header("방 크기 (X: 너비, Y: 깊이)")]
    [SerializeField] private Vector2 roomSize = new Vector2(20f, 20f);

    private CameraController camCtrl;
    private Transform playerT;

    private void Awake()
    {
        // 던전 부모 자동 할당
        if (dungeonParent == null)
        {
            var gen = FindObjectOfType<DungeonGenerator>();
            dungeonParent = gen != null ? gen.transform : null;
        }

        if (dungeonParent == null)
        {
            Debug.LogError("[RoomVisibility] DungeonGenerator를 찾을 수 없습니다.");
            enabled = false;
            return;
        }

        // CameraController 자동 탐색
        camCtrl = FindObjectOfType<CameraController>();
        if (camCtrl == null)
        {
            Debug.LogError("[RoomVisibility] CameraController를 찾을 수 없습니다.");
            enabled = false;
            return;
        }

        // DungeonGenerator 이벤트 구독
        if (dungeonGenerator == null)
            dungeonGenerator = FindObjectOfType<DungeonGenerator>();

        if (dungeonGenerator != null)
            dungeonGenerator.OnCompleted += OnDungeonGenerated;
        else
            Debug.LogError("[RoomVisibility] Inspector나 FindObject에서 DungeonGenerator 할당 실패.");
    }

    private void OnEnable()
    {
        camCtrl.OnRoomChanged += HandleRoomChanged;
    }

    private void OnDisable()
    {
        camCtrl.OnRoomChanged   -= HandleRoomChanged;
        if (dungeonGenerator != null)
            dungeonGenerator.OnCompleted -= OnDungeonGenerated;
    }

    // 던전 생성 완료 시점에 호출: 초기 방 가시성 설정
    private void OnDungeonGenerated()
    {
        playerT = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerT == null)
        {
            Debug.LogWarning("[RoomVisibility] Player를 찾을 수 없어 초기 방 표시를 스킵합니다.");
            return;
        }

        Vector2Int idx = CalculateRoomIndex(playerT.position);
        HandleRoomChanged(idx, idx);
    }

    private void HandleRoomChanged(Vector2Int oldIdx, Vector2Int newIdx)
    {
        foreach (Transform room in dungeonParent)
        {
            bool show = false;
            string name = room.name;

            // 일반 방: "Room_<x>_<y>"
            if (name.StartsWith("Room_"))
            {
                var parts = name.Split('_');
                if (parts.Length == 3
                    && int.TryParse(parts[1], out int x)
                    && int.TryParse(parts[2], out int y))
                {
                    show = (x == newIdx.x && y == newIdx.y);
                }
            }
            // 보스 방 처리 예시
            else if (name == "BossRoom")
            {
                show = false;
            }

            room.gameObject.SetActive(show);
        }
    }

    private Vector2Int CalculateRoomIndex(Vector3 pos)
    {
        int ix = Mathf.FloorToInt(pos.x / roomSize.x);
        int iz = Mathf.FloorToInt(pos.z / roomSize.y);
        return new Vector2Int(ix, iz);
    }
}
