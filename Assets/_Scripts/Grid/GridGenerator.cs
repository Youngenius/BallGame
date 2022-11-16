using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private Transform _spawnZoneEdge_up_1, _spawnZoneEdge_up_2, _spawnZoneEdge_down;
    private float _spawnZoneWidth => Vector3.Distance(_spawnZoneEdge_up_1.position, _spawnZoneEdge_up_2.position);
    private float _spawnZoneDepth => Vector3.Distance(_spawnZoneEdge_up_1.position, _spawnZoneEdge_down.position);

    [SerializeField] private float _minOffset = 1;
    [SerializeField] private float _maxOffset = 3;
    [SerializeField] private int _width;
    [HideInInspector] public int Depth => FindDepth();

    private float _gridSide;

    [HideInInspector] public List<Grid> Grids;

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    foreach (var grid in EvaluateGridPoints())
    //    {
    //        Gizmos.DrawWireCube(grid.Pos, new Vector3(_gridSide, 1, _gridSide));
    //    }
    //}

    private int FindDepth()
    {
        _gridSide = _spawnZoneWidth / _width;
        var roughQuantity = _spawnZoneDepth / _gridSide;

        return Mathf.FloorToInt(roughQuantity);
    }

    public IEnumerable<Grid> EvaluateGridPoints()
    {
        var pointX = _spawnZoneEdge_down.position.x + _gridSide / 2;
        var pointZ_Start = _spawnZoneEdge_down.position.z + _gridSide / 2;
        var pointZ = pointZ_Start;

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < Depth; z++)
            {
                var isInEvenInRow = z % 2 == 0;
                var newGridPos = new Vector3(pointX, 0, pointZ);
                var newGrid = new Grid(newGridPos, isInEvenInRow, _gridSide);

                yield return newGrid;

                pointZ += _gridSide;
            }

            pointX += _gridSide;
            pointZ = pointZ_Start;
        }
    }

    public IEnumerable<Grid> ShuffleGrids()
    {
        var gridsInEvenRows = EvaluateGridPoints().Where(grid => grid.IsInEvenRow);
        var gridsInOddRows = EvaluateGridPoints().Where(grid => !grid.IsInEvenRow);
        var offset = Random.Range(_minOffset, _maxOffset);

        foreach (var grid in gridsInEvenRows)
        {
            grid.Pos.x += offset;

            yield return grid;
        }

        foreach (var grid in gridsInOddRows)
        {
            //grid.Pos.z += Random.Range(0.1f, 0.3f);

            yield return grid;
        }
    }

}
