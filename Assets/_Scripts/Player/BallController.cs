using UnityEngine;
using DG.Tweening;
using System;
using System.Threading.Tasks;

[RequireComponent(typeof(BulletShooter), typeof(BallSpawner))]
public class BallController : MonoBehaviour
{
    private BallSpawner _ballSpawner;
    private BulletShooter _bulletShooter;
    private Transform _ball;
    

    [Header("Volume")]
    [SerializeField] private float _volumePerRow;
    [SerializeField] private float _minVolume;
    [SerializeField] private float _animationDuration;
    [SerializeField] private float _splushAnimationDuration;
    [SerializeField] private Vector3 _splushVolumeModifier;
    private Vector3 _volume => BallProperties.Volume;

    public event Action OnVolumeChanged;


    private void Start()
    {
        _ballSpawner = GetComponent<BallSpawner>();
        _bulletShooter = GetComponent<BulletShooter>();
        _bulletShooter.OnFinishedAiming += ReduceVolume;
        _ballSpawner.OnBallSpawned += OnBallSpawned;
        InputController.Instance.OnMouseButtonDown += OnMouseButtonDown;
        InputController.Instance.OnMouseButtonUp += OnMouseButtonUp;
    }

    private void OnBallSpawned(Ball ball)
    {
        _ball = ball.transform;
    }

    private void OnMouseButtonDown()
    {
        GameFlowController.Instance.ChangeState(GameFlowController.State.Aiming);
        _bulletShooter.Aim();
    }

    private void OnMouseButtonUp()
    {
        GameFlowController.Instance.ChangeState(GameFlowController.State.Shooting);
    }

    private async void ReduceVolume(float time)
    {
        var reduceVolume = time * _volumePerRow;
        var volumeAfterReduce = _volume - new Vector3(reduceVolume, reduceVolume, reduceVolume);

        if (reduceVolume > _volume.x || volumeAfterReduce.x < _minVolume)
        {
            GameFlowController.Instance.ChangeState(GameFlowController.State.Lose);
            return;
        }

        BallProperties.Volume = volumeAfterReduce;
        await SplushBall();

        _ball.DOScale(_volume, _animationDuration);
        _bulletShooter.ShootBullet(reduceVolume);
        _bulletShooter.Reset();

        OnVolumeChanged?.Invoke();
    }

    private async Task SplushBall()
    {
        var splushedVolume = Vector3.Scale(_volume, _splushVolumeModifier);

        await _ball.DOScale(splushedVolume, _splushAnimationDuration).AsyncWaitForCompletion();
    }

}
