using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using DG.Tweening;


public class BulletShooter : MonoBehaviour
{
    private BallSpawner _ballSpawner;
    private Ball _ball;

    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _spawn;
    [SerializeField] private Transform _trajectory;
    [SerializeField] private Transform _target;

    [SerializeField] private float _shotHeight = 1.5f;
    [SerializeField] private float _timeBeforeMovingTarget;
    [SerializeField] private float _moveTargetBy;
    [SerializeField] private Bullet _instantiatedBullet;

    private Coroutine _extendLineCoroutine;
    private Coroutine _trajectoryToZeroCoroutine;

    public event Action<float> OnFinishedAiming;


    private void Start()
    {
        _ballSpawner = FindObjectOfType<BallSpawner>().GetComponent<BallSpawner>();

        _ballSpawner.OnBallSpawned += OnBallSpawned;
        GameFlowController.Instance.OnStateSwitched += OnStateSwitched;
    }

    private void OnBallSpawned(Ball ball)
    {
        _ball = ball;
        _spawn = _ball.BulletSpawn;
        _trajectory = _ball.Trajectory;
        _target = _ball.Target;
    }

    private void OnStateSwitched(GameFlowController.State state)
    {
        if (state == GameFlowController.State.Moving)
        {
            _trajectoryToZeroCoroutine = StartCoroutine(TrajectoryToZeroCoroutine());
            
        }
    }

    public void Aim()
    {
        _extendLineCoroutine = StartCoroutine(StretchTrajectoryCoroutine());
    }


    public void Reset()
    {
        _target.position = _trajectory.position;
    } 

    public void ShootBullet(float volume)
    {
        var bulletVolume = volume * _bullet.VolumeModifier;
        var spawnPos = new Vector3(_spawn.position.x, _shotHeight, _spawn.position.z);

        _instantiatedBullet = Instantiate(_bullet, spawnPos, Quaternion.identity);

        bulletVolume = bulletVolume > _bullet.MinVolume ? bulletVolume : _bullet.MinVolume;
        _instantiatedBullet.Volume = bulletVolume > _bullet.MaxVolume ? _bullet.MaxVolume : bulletVolume;
        _instantiatedBullet.transform.DOScale(_instantiatedBullet.Volume, 0);

        _instantiatedBullet.GetComponent<Rigidbody>().AddForce(_spawn.forward * _bullet.Speed * Time.deltaTime, ForceMode.Impulse);
    }

    public void TrajectoryToZero() =>
        _target.position = _ball.transform.position;

    private IEnumerator StretchTrajectoryCoroutine()
    {
        var time = 0f;

        while (GameFlowController.Instance.CurrentState == GameFlowController.State.Aiming)
        {
            yield return new WaitForSecondsRealtime(_timeBeforeMovingTarget);

            _target.position += _ball.transform.forward * _moveTargetBy;
            time += _timeBeforeMovingTarget;
        }

        OnFinishedAiming?.Invoke(time);
        if (_extendLineCoroutine != null)
            StopCoroutine(_extendLineCoroutine);
    }

    private IEnumerator TrajectoryToZeroCoroutine()
    {
        while (GameFlowController.Instance.CurrentState == GameFlowController.State.Moving)
        {
            TrajectoryToZero();
            yield return null;
        }

        StopCoroutine(_trajectoryToZeroCoroutine);
    }

}
