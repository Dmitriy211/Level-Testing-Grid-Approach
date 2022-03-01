using UnityEngine;

public static class GridTools
{
    public static Vector2Int IntToCoord(int input, Vector2Int gridSize)
    {
        Vector2Int coord = Vector2Int.zero;
        coord.x = input % gridSize.x;
        coord.y = input / gridSize.y;
        return coord;
    }

    public static int CoordToInt(Vector2Int input, Vector2Int gridSize) => 
        input.x + input.y * gridSize.x;
}
