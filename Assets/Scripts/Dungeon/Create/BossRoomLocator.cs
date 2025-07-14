using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public static class BossRoomLocator
    {
        public static Vector2Int FindFarthest(Cell[,] board, Vector2Int start, Vector2Int size)
        {
            var queue = new Queue<Vector2Int>();
            var dist  = new Dictionary<Vector2Int, int>();

            queue.Enqueue(start);
            dist[start] = 0;

            Vector2Int farthest = start;
            int maxDist = 0;

            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                int cd = dist[cur];
                if (cd > maxDist)
                {
                    maxDist = cd;
                    farthest = cur;
                }

                foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
                {
                    if (!board[cur.x, cur.y].Connections[(int)dir]) continue;

                    var next = dir switch
                    {
                        Direction.Up    => new Vector2Int(cur.x, cur.y + 1),
                        Direction.Down  => new Vector2Int(cur.x, cur.y - 1),
                        Direction.Left  => new Vector2Int(cur.x - 1, cur.y),
                        Direction.Right => new Vector2Int(cur.x + 1, cur.y),
                        _               => cur
                    };

                    if (!board[next.x, next.y].Visited || dist.ContainsKey(next)) continue;

                    dist[next] = cd + 1;
                    queue.Enqueue(next);
                }
            }

            return farthest;
        }
    }
}