using System;
using UnityEngine;

public class GameFlowController : Singleton<GameFlowController>
{
    private ObstacleSpawner _obstacleSpawner;
    private BallSpawner _ballSpawner;
    private Ball _ball;

    public event Action<State> OnStateSwitched;

    [HideInInspector] public State CurrentState;

    public enum State
    {
        Waiting,
        Aiming,
        Shooting,
        Moving,
        CheckForPath,
        Win,
        Lose,
    }

    private void Start()
    {
        _ballSpawner = FindObjectOfType<BallSpawner>().GetComponent<BallSpawner>();
        _obstacleSpawner = FindObjectOfType<ObstacleSpawner>().GetComponent<ObstacleSpawner>();

        _ball = _ballSpawner.SpawnBall();
        _obstacleSpawner.SpawnObstacles();
        

        ChangeState(State.Waiting);
    }

    public void ChangeState(State state)
    {
        CurrentState = state;

        switch (state)
        {
            case State.Waiting:
                break;
            case State.Aiming:
                break;
            case State.Shooting:
                break;
            case State.Moving:
                break;
            case State.CheckForPath:
                break;

            case State.Win:
            case State.Lose:
                var go = new GameObject();
                var endGameConroller = go.AddComponent<EndGameController>().GetComponent<EndGameController>();

                switch (state)
                {
                    case State.Win:
                        endGameConroller.HandleEnd(EndGameBy.Win);
                        break;
                    case State.Lose:
                        endGameConroller.HandleEnd(EndGameBy.Lose);
                        break;
                }
                break;
        }

        OnStateSwitched?.Invoke(CurrentState);
    }

    public void Restart()
    {
        ChangeState(State.Waiting);
        _obstacleSpawner.RespawnObstacles();

        Destroy(_ball.gameObject);
        _ball = _ballSpawner.SpawnBall();
    }
}
