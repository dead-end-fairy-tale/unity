using UnityEngine;
using UnityEngine.AI;

public class SetNavDestination<TContext> : ActionNode<TContext>
    where TContext : PlayerContext
{
    protected override NodeState Execute(TContext context)
    {
        var agent = context.Agent;
        var dest = context.Blackboard.Get<Vector3>("destination");

        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("[SetNavDestination] Agent가 NavMesh 위에 있지 않습니다. 샘플링 시도...");
            
            if (NavMesh.SamplePosition(dest, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
                Debug.Log("[SetNavDestination] 에이전트 워프 완료: " + hit.position);
            }
            else
            {
                Debug.LogError("[SetNavDestination] NavMesh를 찾을 수 없어 목적지 설정 실패");
                return NodeState.Failure;
            }
        }

        if (agent.SetDestination(dest))
            return NodeState.Success;
        else
        {
            Debug.LogError("[SetNavDestination] SetDestination 호출 실패");
            return NodeState.Failure;
        }
    }
}