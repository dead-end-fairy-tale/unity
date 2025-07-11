using UnityEngine;
using UnityEngine.AI;

public class PlayerTransitionHandler : MonoBehaviour
{
    [SerializeField] private Vector2 roomSize = new Vector2(20f, 20f);
    [SerializeField] private float doorOffset = 0.3f;

    private CameraController camCtrl;
    private PlayerContext context;
    private NavMeshAgent agent;

    private void Awake()
    {
        camCtrl = GetComponent<CameraController>();
    }

    private void OnEnable()
    {
        PlayerSpawner.OnPlayerSpawned += OnPlayerSpawned;
        camCtrl.OnRoomChanged   += OnRoomChanged;
    }

    private void OnDisable()
    {
        PlayerSpawner.OnPlayerSpawned -= OnPlayerSpawned;
        camCtrl.OnRoomChanged   -= OnRoomChanged;
    }

    private void OnPlayerSpawned(Transform spawned)
    {
        var bt = spawned.GetComponent<BT_Player>();
        if (bt == null) return;
        context = bt.Context;
        agent   = context.Agent;
    }

    private void OnRoomChanged(Vector2Int oldIdx, Vector2Int newIdx)
    {
        if (context == null) return;

        float halfW = roomSize.x * 0.5f;
        float halfD = roomSize.y * 0.5f;
        Vector3 center = new Vector3(newIdx.x * roomSize.x, 0f, newIdx.y * roomSize.y);

        Vector3 doorPos = center;
        Vector3 forward = Vector3.zero;

        if (newIdx.x > oldIdx.x)
        {
            doorPos.x -= halfW - doorOffset;
            forward = Vector3.right;
        }
        else if (newIdx.x < oldIdx.x)
        {
            doorPos.x += halfW - doorOffset;
            forward = Vector3.left;
        }
        else if (newIdx.y > oldIdx.y)
        {
            doorPos.z -= halfD - doorOffset;
            forward = Vector3.forward;
        }
        else if (newIdx.y < oldIdx.y)
        {
            doorPos.z += halfD - doorOffset;
            forward = Vector3.back;
        }

        if (agent != null && agent.isOnNavMesh)
        {
            agent.Warp(doorPos);
            agent.ResetPath();
            agent.transform.rotation = Quaternion.LookRotation(forward);
        }
        else
        {
            context.Transform.position = doorPos;
            context.Transform.rotation = Quaternion.LookRotation(forward);
        }

        context.Blackboard.Set("destination", doorPos);
    }
}

