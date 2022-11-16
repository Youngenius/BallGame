using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObstacleSpawner : MonoBehaviour
{
    private Material _materialPref;
    [SerializeField] private Obstacle _obstaclePref;
    private float _defaultHeight => _obstaclePref.transform.lossyScale.y;

    [Range(0, 1)] 
    [SerializeField] private float _spawnProbability = 0.7f;
    [SerializeField] private float _minDistanceFromGridBorder = 0.05f;
    [SerializeField] private float _maxDistanceFromGridBorder = 0.1f;
    private float _distanceFromGridBorderX, _distanceFromGridBorderZ;

    [Range(0, 360)]
    [SerializeField] private float _maxRotation;
    [SerializeField] private bool _rotate;

    private GridGenerator[] _gridGenerators;
    private List<List<Grid>> _grids = new List<List<Grid>>();
    private List<Obstacle> _activeObstacles = new List<Obstacle>();
    private ObjectPool<Obstacle> _obstaclePool;

    private void Awake()
    {
        _gridGenerators = FindObjectsOfType<GridGenerator>();

        foreach (var gridGenerators in _gridGenerators)
        {
            List<Grid> grids = new List<Grid>(gridGenerators.ShuffleGrids());

            _grids.Add(grids);
        }
        #region Pool
        _obstaclePool = new ObjectPool<Obstacle>(() =>
        {
            return Instantiate(_obstaclePref);
        }, particle =>
        {
            particle.gameObject.SetActive(true);
        }, partcle =>
        {
            partcle.gameObject.SetActive(false);
        }, particle => {
            Destroy(particle.gameObject);
        }, false, 100);
        #endregion
    }

    public void SpawnObstacles()
    {
        foreach (var grids in _grids)
        {
            foreach (var grid in grids)
            {
                _distanceFromGridBorderX = Random.Range(_minDistanceFromGridBorder, _maxDistanceFromGridBorder);
                _distanceFromGridBorderZ = Random.Range(_minDistanceFromGridBorder, _maxDistanceFromGridBorder);

                if (Random.Range(0f, 1f) < _spawnProbability)
                {
                    var obstacle = _obstaclePool.Get();
                    var obstScale = obstacle.transform.localScale;
                    var meshRenderer = obstacle.GetComponent<MeshRenderer>();

                    _activeObstacles.Add(obstacle);

                    // asigning new material to obstacle, so that material props could be regulated separetely
                    // from each other

                     _materialPref = meshRenderer.sharedMaterial;
                     meshRenderer.sharedMaterial = new Material(_materialPref);
                     obstacle.Material = meshRenderer.sharedMaterial;
                                      

                    obstacle.transform.localScale =
                        new Vector3(grid.SideLength - _distanceFromGridBorderX, _defaultHeight + obstacle.AddedHeight, grid.SideLength - _distanceFromGridBorderZ);
                    obstacle.transform.position =
                        new Vector3(grid.Pos.x, grid.Pos.y + obstScale.y / 2, grid.Pos.z);

                    if (_rotate)
                        obstacle.transform.Rotate
                            (Quaternion.identity.x, Random.Range(-_maxRotation, _maxRotation), Quaternion.identity.z);
                }
            }
        }
    }

    public void RespawnObstacles()
    {
        foreach (var obstacle in _activeObstacles.ToList())
            Release(obstacle);

        SpawnObstacles();
    }

    public void Release(Obstacle obstacle)
    {
        _obstaclePool.Release(obstacle);
        _activeObstacles.Remove(obstacle);
    }
}
