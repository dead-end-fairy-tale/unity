using UnityEngine;
using UnityEngine.AI;

public class PlayerContext : IBehaviorContext
{
    #region IBehaviorContext
    public Blackboard Blackboard { get; }
    #endregion
    
    public Transform Transform { get; }
    public NavMeshAgent Agent { get; }
    public InputReceiver Input { get; }

    public PlayerContext(MonoBehaviour host, Blackboard blackboard)
    {
        Transform = host.transform;
        Agent = host.GetComponent<NavMeshAgent>();
        Input = host.GetComponent<InputReceiver>();
        Blackboard = blackboard;
    }
}