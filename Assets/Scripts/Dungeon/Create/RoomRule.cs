using System;
using UnityEngine;

namespace Dungeon
{
    [Serializable]
    public class RoomRule
    {
        [Tooltip("방 프리팹")] public GameObject RoomPrefab;

        [Tooltip("룰 적용 최소 좌표")] public Vector2Int MinPosition;

        [Tooltip("룰 적용 최대 좌표")] public Vector2Int MaxPosition;

        [Tooltip("해당 영역 내에 반드시 스폰할지 여부")] public bool Obligatory;

        public SpawnStatus GetSpawnStatus(int x, int y)
        {
            bool inRange = x >= MinPosition.x && x <= MaxPosition.x
                                              && y >= MinPosition.y && y <= MaxPosition.y;
            if (!inRange) return SpawnStatus.None;
            return Obligatory ? SpawnStatus.Required : SpawnStatus.Possible;
        }
    }
}
