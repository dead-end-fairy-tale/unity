using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
    public class MazeGenerator
    {
        private readonly Cell[,] _board;
        private readonly Vector2Int _size;
        private readonly int _maxVisits;

        public MazeGenerator(Cell[,] board, Vector2Int size, int maxVisits)
        {
            _board = board;
            _size = size;
            _maxVisits = maxVisits;
        }

        public void Carve(Vector2Int start)
        {
            var stack = new Stack<Vector2Int>();
            Vector2Int current = start;
            int visited = 0;

            while (visited < _maxVisits && (stack.Count > 0 || HasUnvisitedNeighbors(current)))
            {
                if (!_board[current.x, current.y].Visited)
                {
                    _board[current.x, current.y].Visited = true;
                    visited++;
                }

                var neighbors = GetUnvisitedNeighbors(current);
                if (neighbors.Count == 0)
                {
                    current = stack.Pop();
                }
                else
                {
                    stack.Push(current);
                    var next = neighbors[UnityEngine.Random.Range(0, neighbors.Count)];
                    ConnectCells(current, next);
                    current = next;
                }
            }
        }

        private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int pos)
        {
            var result = new List<Vector2Int>();
            int x = pos.x, y = pos.y;

            if (y + 1 < _size.y && !_board[x, y + 1].Visited) result.Add(new Vector2Int(x, y + 1));
            if (y - 1 >= 0     && !_board[x, y - 1].Visited) result.Add(new Vector2Int(x, y - 1));
            if (x + 1 < _size.x && !_board[x + 1, y].Visited) result.Add(new Vector2Int(x + 1, y));
            if (x - 1 >= 0     && !_board[x - 1, y].Visited) result.Add(new Vector2Int(x - 1, y));

            return result;
        }

        private bool HasUnvisitedNeighbors(Vector2Int pos) => GetUnvisitedNeighbors(pos).Count > 0;

        private void ConnectCells(Vector2Int a, Vector2Int b)
        {
            var dirToB = GetDirection(a, b);
            var dirToA = GetOpposite(dirToB);

            _board[a.x, a.y].Connections[(int)dirToB] = true;
            _board[b.x, b.y].Connections[(int)dirToA] = true;
        }

        private Direction GetDirection(Vector2Int from, Vector2Int to)
        {
            var d = to - from;
            if (d == Vector2Int.up)    return Direction.Up;
            if (d == Vector2Int.down)  return Direction.Down;
            if (d == Vector2Int.left)  return Direction.Left;
            if (d == Vector2Int.right) return Direction.Right;
            throw new ArgumentException($"Invalid neighbor diff: {d}");
        }

        private Direction GetOpposite(Direction dir) => dir switch
        {
            Direction.Up    => Direction.Down,
            Direction.Down  => Direction.Up,
            Direction.Left  => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
