using System;
using DG.Tweening;
using UnityEngine;

namespace _GAME.__Scripts.Final___Finish
{
    public class FinalTexture : MonoBehaviour
    {
        [SerializeField] private Material _material;
        private bool isFade;

        private void Start()
        {
            _material.DOFade(0, 1);
        }

        private void Update()
        {
            if (isFade) this.enabled = false;
            if (Player.Instance.transform.position.z + 40 > transform.parent.position.z)
            {
                isFade = true;
                _material.DOFade(0.35f, 2f);
            }
        }
    }
}