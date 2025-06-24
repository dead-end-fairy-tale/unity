using UnityEngine;

namespace Dungeon
{
    public class BoardManager
    {
        public Cell[,] Board { get; }
        public Vector2Int Size { get; }

        public BoardManager(Vector2Int size)
        {
            Size = size;
            Board = new Cell[size.x, size.y];
        }

        public void Initialize()
        {
            for (int x = 0; x < Size.x; x++)
            for (int y = 0; y < Size.y; y++)
                Board[x, y] = new Cell();
        }
    }
}