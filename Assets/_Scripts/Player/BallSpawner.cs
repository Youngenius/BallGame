using DG.Tweening;
using System;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    private BallProperties _ballProperties;
    private BulletShooter _bulletShooter;

    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private Transform _spawnZone;
    [SerializeField] private Transform _parent;

    public event Action<Ball> OnBallSpawned;

    private void Awake()
    {
        _ballProperties = FindObjectOfType<BallProperties>().GetComponent<BallProperties>();
        _bulletShooter = GetComponent<BulletShooter>();
    }

    public Ball SpawnBall()
    {
        var ball = Instantiate(_ballPrefab, _spawnZone.position, Quaternion.identity);
        OnBallSpawned?.Invoke(ball);

        BallProperties.Volume = _ballProperties.FindVolume();
        ball.transform.DOScale(BallProperties.Volume, 0);
        ball.transform.position = _spawnZone.position;

        _bulletShooter.TrajectoryToZero();

        return ball;
    }

}
