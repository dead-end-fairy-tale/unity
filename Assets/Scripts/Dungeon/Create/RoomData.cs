using UnityEngine;

namespace Dungeon
{
    public class RoomData
    {
        public Vector2Int Index { get; private set; }
        public GameObject Prefab { get; private set; }
        public bool[] Connections { get; private set; }

        public RoomData(Vector2Int index, GameObject prefab, bool[] connections)
        {
            Index = index;
            Prefab = prefab;
            Connections = connections;
        }
    }
}