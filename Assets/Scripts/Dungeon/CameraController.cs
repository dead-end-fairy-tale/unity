using UnityEngine;
using System;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public event Action<Vector2Int, Vector2Int> OnRoomChanged;

    [Header("Player 자동 추적")]
    [SerializeField] private string playerTag = "Player";

    [Header("방 크기 (X: 너비, Y: 깊이)")]
    [SerializeField] private Vector2 roomSize;

    [Header("카메라 오프셋 (방 중앙 기준)")]
    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 10f, -10f);

    [Header("부드러운 이동 설정")]
    [SerializeField] private float moveSmoothTime = 0.3f;
    private Vector3 moveVelocity;

    private Transform playerT;
    private Vector3 currentRoomCenter;
    private Vector2Int prevRoomIndex;
    private bool isInitialized = false;

    private void LateUpdate()
    {
        if (playerT == null)
        {
            var go = GameObject.FindWithTag(playerTag);
            if (go == null) return;
            playerT = go.transform;
        }

        var newRoomIndex = CalculateRoomIndex();

        if (!isInitialized)
        {
            prevRoomIndex = newRoomIndex;
            UpdateRoomCenter(newRoomIndex);
            transform.position = currentRoomCenter + cameraOffset;
            transform.LookAt(currentRoomCenter);
            isInitialized = true;
            return;
        }

        if (newRoomIndex != prevRoomIndex)
        {
            OnRoomChanged?.Invoke(prevRoomIndex, newRoomIndex);
            prevRoomIndex = newRoomIndex;
        }

        UpdateRoomCenter(newRoomIndex);
        Vector3 targetPos = currentRoomCenter + cameraOffset;
        transform.position = Vector3.SmoothDamp(
            transform.position, targetPos, ref moveVelocity, moveSmoothTime);
    }

    private Vector2Int CalculateRoomIndex()
    {
        Vector3 p = playerT.position;
        float halfW = roomSize.x * 0.5f;
        float halfD = roomSize.y * 0.5f;

        int ix = Mathf.FloorToInt((p.x + halfW) / roomSize.x);
        int iz = Mathf.FloorToInt((p.z + halfD) / roomSize.y);
        return new Vector2Int(ix, iz);
    }

    private void UpdateRoomCenter(Vector2Int idx)
    {
        currentRoomCenter = new Vector3(idx.x * roomSize.x, 0f, idx.y * roomSize.y);
    }
}
