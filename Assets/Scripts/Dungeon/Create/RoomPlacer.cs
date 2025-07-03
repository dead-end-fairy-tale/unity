using System;
using UnityEngine;

namespace Dungeon
{
    public class RoomPlacer
    {
        private readonly Cell[,] _board;
        private readonly Vector2Int _size;
        private readonly Vector2 _offset;
        private readonly RoomRule[] _rules;
        private readonly GameObject _bossPrefab;
        private readonly Transform _parent;

        public RoomPlacer(Cell[,] board, Vector2Int size, Vector2 offset, RoomRule[] rules, GameObject bossPrefab, Transform parent)
        {
            _board     = board;
            _size      = size;
            _offset    = offset;
            _rules     = rules;
            _bossPrefab= bossPrefab;
            _parent    = parent;
        }

        public void PlaceAll(Vector2Int bossCoord)
        {
            for (int x = 0; x < _size.x; x++)
            for (int y = 0; y < _size.y; y++)
            {
                var cell = _board[x, y];
                if (!cell.Visited) continue;

                bool isBoss = (x == bossCoord.x && y == bossCoord.y);
                var pos3D = new Vector3(x * _offset.x, 0f, y * _offset.y);
                GameObject instance;

                if (isBoss)
                {
                    instance = UnityEngine.Object.Instantiate(_bossPrefab, pos3D, Quaternion.identity, _parent);
                    instance.name = "BossRoom";
                }
                else
                {
                    int idx = ChooseRule(x, y);
                    var rule = _rules[idx];
                    instance = UnityEngine.Object.Instantiate(rule.RoomPrefab, pos3D, Quaternion.identity, _parent);
                    instance.name = $"Room_{x}_{y}";
                }

                var behaviour = instance.GetComponent<RoomBehaviour>();
                behaviour?.UpdateRoom(FilterConnections(cell, x, y));
            }
        }

        private bool[] FilterConnections(Cell cell, int x, int y)
        {
            var copy = (bool[])cell.Connections.Clone();
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                var n = dir switch
                {
                    Direction.Up    => new Vector2Int(x, y + 1),
                    Direction.Down  => new Vector2Int(x, y - 1),
                    Direction.Left  => new Vector2Int(x - 1, y),
                    Direction.Right => new Vector2Int(x + 1, y),
                    _               => new Vector2Int(-1, -1)
                };

                bool valid = n.x >= 0 && n.x < _size.x && n.y >= 0 && n.y < _size.y && _board[n.x, n.y].Visited;
                if (!valid) copy[(int)dir] = false;
            }
            return copy;
        }

        private int ChooseRule(int x, int y)
        {
            var candidates = new System.Collections.Generic.List<int>();
            for (int i = 0; i < _rules.Length; i++)
            {
                var status = _rules[i].GetSpawnStatus(x, y);
                if (status == SpawnStatus.Required) return i;
                if (status == SpawnStatus.Possible) candidates.Add(i);
            }
            return candidates.Count > 0
                ? UnityEngine.Random.Range(0, candidates.Count)
                : 0;
        }
    }
}
