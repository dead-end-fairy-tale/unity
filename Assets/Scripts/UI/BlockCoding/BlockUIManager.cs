using System;
using System.Collections.Generic;
using UnityEngine;
using BlockCoding;

namespace UI
{
    public class BlockUIManager : MonoBehaviour
    {
        [Header("Panel & Prefabs")]
        public Transform contentPanel;
        public GameObject attackPrefab;
        public GameObject defendPrefab;
        public GameObject movePrefab;
        public GameObject rotateLeftPrefab;
        public GameObject rotateRightPrefab;
        public GameObject loopStartPrefab;
        public GameObject loopEndPrefab;
        public GameObject executeButton;

        public event Action<CommandType> OnAddCommand;
        public event Action OnExecute;

        public void AddAttack()      => OnAddCommand?.Invoke(CommandType.Attack);
        public void AddDefend()      => OnAddCommand?.Invoke(CommandType.Defend);
        public void AddMove()        => OnAddCommand?.Invoke(CommandType.Move);
        public void AddRotateLeft()  => OnAddCommand?.Invoke(CommandType.RotateLeft);
        public void AddRotateRight() => OnAddCommand?.Invoke(CommandType.RotateRight);
        public void AddLoopStart()   => OnAddCommand?.Invoke(CommandType.LoopStart);
        public void AddLoopEnd()     => OnAddCommand?.Invoke(CommandType.Loop);
        public void Execute()        => OnExecute?.Invoke();

        public void Refresh(List<IBlockCommand> commands)
        {
            foreach (Transform t in contentPanel)
            {
                Destroy(t.gameObject);
            }

            foreach (var cmd in commands)
            {
                GameObject prefab = null;
                switch (cmd.Type)
                {
                    case CommandType.Attack:      prefab = attackPrefab;       break;
                    case CommandType.Defend:      prefab = defendPrefab;       break;
                    case CommandType.Move:        prefab = movePrefab;         break;
                    case CommandType.RotateLeft:  prefab = rotateLeftPrefab;   break;
                    case CommandType.RotateRight: prefab = rotateRightPrefab;  break;
                    case CommandType.LoopStart:   prefab = loopStartPrefab;    break;
                    case CommandType.Loop:        prefab = loopEndPrefab;      break;
                }
                
                if (prefab == null)
                    continue;

                GameObject go = Instantiate(prefab, contentPanel);
                CommandButton button = go.GetComponent<CommandButton>();
                if (button != null)
                {
                    button.Initialize(cmd.Type);
                }
            }
        }
    }
}
