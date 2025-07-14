using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width  = 8;
    public int height = 8;
    public float tileSize = 1f;

    public bool IsInside(Vector2Int coord)
        => coord.x >= 0 && coord.x < width
                        && coord.y >= 0 && coord.y < height;

    public bool IsWalkable(Vector2Int coord)
    {
        // TODO: 벽이나 다른 오브젝트가 있는 칸을 막으려면 여기에 로직 추가
        return IsInside(coord);
    }

    public Vector3 CoordToWorld(Vector2Int coord)
    {
        return new Vector3(coord.x * tileSize, 0f, coord.y * tileSize);
    }

    public Vector2Int WorldToCoord(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x / tileSize),
            Mathf.RoundToInt(worldPos.z / tileSize)
        );
    }
}