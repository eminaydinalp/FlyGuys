using System;
using DG.Tweening;
using UnityEngine;

public class ObstacleAnim : MonoBehaviour
{
    [SerializeField] private DOTweenAnimation _doTweenAnimation;
    public float zMaxValue;
    public float zMinValue;

    private void Start()
    {
        _doTweenAnimation.DOPlay();
    }

    private void Update()
    {
        if (transform.localPosition.z > zMaxValue)
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, 180, transform.localRotation.z);
        }
        
        if (transform.localPosition.z < zMinValue)
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, 0, transform.localRotation.z);
        }

    }
}
