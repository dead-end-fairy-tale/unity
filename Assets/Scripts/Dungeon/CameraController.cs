using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Player 자동 추적")]
    [SerializeField] private string playerTag = "Player";

    [Header("방 크기 (X: 너비, Y: 깊이)")]
    [SerializeField] private Vector2 roomSize;

    [Header("카메라 오프셋 (방 중앙 기준)")]
    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 10f, -10f);

    [Header("부드러운 이동 설정")]
    [SerializeField, Tooltip("작을수록 빠르게 따라갑니다.")] 
    private float moveSmoothTime = 0.3f;
    private Vector3 moveVelocity;

    [Header("방 전환 임계치 보정")]
    [SerializeField, Tooltip("각 축별 보정값. 양수면 더 빨리, 음수면 더 늦게 전환")]
    private Vector2 switchThresholdOffset = Vector2.zero;

    private Transform playerT;
    private Vector3 currentRoomCenter;
    private bool isInitialized = false;

    private void LateUpdate()
    {
        if (playerT == null)
        {
            var go = GameObject.FindWithTag(playerTag);
            if (go == null) return;
            playerT = go.transform;
        }

        if (!isInitialized)
        {
            UpdateRoomCenter();
            transform.position = currentRoomCenter + cameraOffset;
            transform.LookAt(currentRoomCenter);
            isInitialized = true;
            return;
        }

        UpdateRoomCenter();
        Vector3 targetPos = currentRoomCenter + cameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref moveVelocity, moveSmoothTime);
    }

    private void UpdateRoomCenter()
    {
        Vector3 p = playerT.position;
        float halfW = roomSize.x * 0.5f;
        float halfD = roomSize.y * 0.5f;

        // 1) 보정 없는 base 인덱스 계산
        int baseIx = Mathf.FloorToInt((p.x + halfW) / roomSize.x);
        int baseIz = Mathf.FloorToInt((p.z + halfD) / roomSize.y);

        // 2) 현재 방 안에서 플레이어의 상대 좌표 (0 ~ roomSize) 구하기
        float localX = (p.x + halfW) - baseIx * roomSize.x; 
        float localZ = (p.z + halfD) - baseIz * roomSize.y;

        // 3) 상대 위치가 절반을 넘었으면 +offset, 아니면 –offset
        float appliedOffsetX = (localX > halfW ?  switchThresholdOffset.x 
            : -switchThresholdOffset.x);
        float appliedOffsetZ = (localZ > halfD ?  switchThresholdOffset.y 
            : -switchThresholdOffset.y);

        // 4) 오프셋을 반영한 최종 인덱스
        int ix = Mathf.FloorToInt((p.x + halfW + appliedOffsetX) / roomSize.x);
        int iz = Mathf.FloorToInt((p.z + halfD + appliedOffsetZ) / roomSize.y);

        // 5) 최종 방 중심 계산
        currentRoomCenter = new Vector3(ix * roomSize.x, 0f, iz * roomSize.y);
    }

}
