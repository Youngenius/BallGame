using UnityEngine;

public class Grid
{
    public Vector3 Pos;
    public float SideLength;
    public int Row;
    public bool IsInEvenRow;

    public Grid(Vector3 position, bool isInEvenRow, float sideLength)
    {
        Pos = position;
        IsInEvenRow = isInEvenRow;
        SideLength = sideLength;
    }
}
