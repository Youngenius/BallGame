using System.Collections;
using UnityEngine;

public class TrajectoryDrawer : MonoBehaviour
{
    private BallSpawner _ballSpawner;
    private Transform _ball;

    [SerializeField] private float _yPos;
    [SerializeField] private Transform _target;
    private LineRenderer _lineRenderer;
    
    private float _startVolume;

    private void Awake()
    {
        _startVolume = BallProperties.Volume.x;
        _lineRenderer = GetComponent<LineRenderer>();
        _ballSpawner = FindObjectOfType<BallSpawner>().GetComponent<BallSpawner>();
        _ballSpawner.OnBallSpawned += OnBallSpawned;
    }

    private void OnBallSpawned(Ball ball)
    {
        _ball = ball.transform;
        _target = ball.Target;
    }

    private void Update()
    {
        if (_ball != null)       
            SetTrajectory();
    }
    
    private void SetTrajectory()
    {
        var ballPos = _ball.position;
        var targetPos = _target.position;

        _lineRenderer.SetPosition(0, new Vector3(ballPos.x, _yPos, ballPos.z));
        _lineRenderer.SetPosition(1, new Vector3(targetPos.x, _yPos, targetPos.z));
    }

    public void SetLineWidth()
    {
        var width = BallProperties.Volume.x * _lineRenderer.widthMultiplier / _startVolume;
        _lineRenderer.startWidth = width;
        _lineRenderer.endWidth = width;
    }

}
