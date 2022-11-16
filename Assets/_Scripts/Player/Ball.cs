using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Ball : MonoBehaviour 
{
    [SerializeField] private BallStruct _ballStruct;

    public Transform Target;
    public Transform BulletSpawn;
    public Transform Trajectory;

    public void Init()
    {
        this.Target = _ballStruct.Target;
        this.BulletSpawn = _ballStruct.BulletSpawn;
        this.Trajectory = _ballStruct.Trajectory;
    }
}

[Serializable]
public struct BallStruct
{
    public Transform Target;
    public Transform BulletSpawn;
    public Transform Trajectory;
}
