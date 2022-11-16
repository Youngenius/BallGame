using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _radiusKoef = 0.1f;

    [SerializeField] private float _minVolume, _maxVolume;
    public float Speed;
    public float VolumeModifier;
    public float Volume;
    
    public float MinVolume { get => _minVolume; }
    public float MaxVolume { get => _maxVolume; }
    public float PoisonRadius => 
        Volume <= MinVolume ? Volume : Volume * 0.5f + Volume * 0.5f * _radiusKoef;

    //public static event Action OnShot;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            PoisonTargets();
            //OnShot?.Invoke();
        }

        if (!collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            if (!collision.gameObject.CompareTag("Obstacle"))
                GameFlowController.Instance.ChangeState(GameFlowController.State.Waiting);
        }
    }

    public void PoisonTargets()
    {
        foreach (var target in DetactTargets())
        {
            target.PoisonAffect();
        }
    }

    private IEnumerable<Obstacle> DetactTargets()
    {
        var detactedTargets = Physics.OverlapSphere(this.transform.position, PoisonRadius);
        
        foreach (var target in detactedTargets)
        {
            if (target.TryGetComponent<Obstacle>(out var obstacle))
                yield return obstacle;
        }
    }
}
