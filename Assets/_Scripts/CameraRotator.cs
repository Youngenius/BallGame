using UnityEngine;

[RequireComponent(typeof(BallSpawner))]
public class CameraRotator : MonoBehaviour
{
    private BallSpawner _ballSpawner;
    private Transform _ball;

    [SerializeField] private Camera _camera;
    private Quaternion _initialRotation;
    private Vector3 _initialPos;

    [SerializeField] private float _rotationConstraint;
    [SerializeField] private float _rotationSpeed;

    private float _angleRotated;

    private void Start()
    {
        _initialRotation = _camera.transform.rotation;
        _initialPos = _camera.transform.position;

        _ballSpawner = GetComponent<BallSpawner>();
        _ballSpawner.OnBallSpawned += OnBallSpawned;
    }

    private void OnBallSpawned(Ball ball)
    {
        _ball = ball.transform;

        _camera.transform.rotation = _initialRotation;
        _camera.transform.position = _initialPos;
        _angleRotated = 0;
    }

    public void RotateRight()
    {
        if (_angleRotated < _rotationConstraint)       
            Rotate(true);
    }

    public void RotateLeft()
    {
        if (_angleRotated > -_rotationConstraint)
            Rotate(false);
    }

    private void Rotate(bool rotateRight)
    {
        var rotateSide = rotateRight ? 1 : -1;
        var angleToRotate = rotateSide * _rotationSpeed * Time.deltaTime;

        _angleRotated += angleToRotate;
        _ball.Rotate(0, angleToRotate, 0);
        _camera.transform.RotateAround(_ball.position, Vector3.up, angleToRotate);
    }
}
