using System.Threading.Tasks;
using UnityEngine;


[RequireComponent(typeof(ColorChanger))]
public class Obstacle : MonoBehaviour
{
    private ObstacleSpawner _obstacleSpawner;
    private ColorChanger _colorChanger;

    [Range(0, 5f)]
    [SerializeField] private float _maxAddedHeight;

    public Material Material;

    public float AddedHeight
    {
        get
        {
            return Random.Range(0, _maxAddedHeight);
        }
    }

    private void Start()
    {
        _obstacleSpawner = FindObjectOfType<ObstacleSpawner>().GetComponent<ObstacleSpawner>();
        _colorChanger = GetComponent<ColorChanger>();
    }

    public async Task PoisonAffect()
    {
        await _colorChanger.ChangeColor(this);

        _obstacleSpawner.Release(this);
        _colorChanger.ReturnInitialColors();

        GameFlowController.Instance.ChangeState(GameFlowController.State.CheckForPath);
    }

}
