using System;
using UnityEngine;

namespace Dungeon
{
    public class RoomBehaviour : MonoBehaviour
    {
        public GameObject[] walls;
        public GameObject[] doors;

        public void UpdateRoom(bool[] connections)
        {
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                int idx = (int)dir;
                doors[idx].SetActive(connections[idx]);
                walls[idx].SetActive(!connections[idx]);
            }
        }
    }
}