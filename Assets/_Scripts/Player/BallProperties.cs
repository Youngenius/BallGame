using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallProperties : MonoBehaviour
{
    private GridGenerator _gridGenerator;

    [SerializeField] private float _volumePerRow;
    [SerializeField] private float _extraVolumePercentage = 20;
    public static Vector3 Volume { get; set; }


    private void Awake()
    {
        Volume = FindVolume();
        
    }

    public Vector3 FindVolume()
    {
        _gridGenerator = FindObjectOfType<GridGenerator>().GetComponent<GridGenerator>();
        var volume = Vector3.one * _gridGenerator.Depth * _volumePerRow;
        var extraVolume = volume * _extraVolumePercentage * 0.01f;

        Volume = volume + extraVolume;
        return Volume;
    }

}
