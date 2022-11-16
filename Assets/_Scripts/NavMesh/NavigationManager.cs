using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavigationManager : MonoBehaviour
{
    private BallSpawner _ballSpawner;

    [SerializeField] private float _distToStopAwayTheTarget;
    [SerializeField] private Transform _target;
    private NavMeshAgent _agent;
    private NavMeshSurface _surface;
    private NavMeshPath _path;

    private Coroutine _stopAwayFromTargetCoroutine;

    private void Awake()
    {
        _ballSpawner = FindObjectOfType<BallSpawner>().GetComponent<BallSpawner>();
        _surface = GetComponent<NavMeshSurface>();
        BuildNavMesh();
        
        _path = new NavMeshPath();

        _ballSpawner.OnBallSpawned += OnBallSpawned;
        GameFlowController.Instance.OnStateSwitched += OnStateSwitched;
    }

    private void OnBallSpawned(Ball ball)
    {
        _agent = ball.GetComponent<NavMeshAgent>();
    }

    private void OnStateSwitched(GameFlowController.State state)
    {
        if (state == GameFlowController.State.CheckForPath)
            TryReachTheTarget();
    }

    private void TryReachTheTarget()
    {
        
        BuildNavMesh();
        if (CanReachArea(_target.position))
        {
            _agent.SetDestination(_target.position);
            GameFlowController.Instance.ChangeState(GameFlowController.State.Moving);
            _stopAwayFromTargetCoroutine = StartCoroutine(StopAwayFromTargetCoroutine());

            return;
        }

        GameFlowController.Instance.ChangeState(GameFlowController.State.Waiting);
    }

    public bool CanReachArea(Vector3 target)
    {
        return _agent.CalculatePath(target, _path) && _path.status == NavMeshPathStatus.PathComplete;        
    }

    private void BuildNavMesh()
    {
        _surface.BuildNavMesh();
    }

    private IEnumerator StopAwayFromTargetCoroutine()
    {
        while (Vector3.Distance(_agent.transform.position, _target.position) > _distToStopAwayTheTarget)
        {
            yield return null;
        }

        _agent.ResetPath();
        GameFlowController.Instance.ChangeState(GameFlowController.State.Win);

        if (_stopAwayFromTargetCoroutine != null)
            StopCoroutine(_stopAwayFromTargetCoroutine);
    }
        
}
