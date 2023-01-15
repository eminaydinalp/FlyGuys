using System;
using System.Collections;
using DG.Tweening;
using Lean.Pool;
using MoreMountains.Tools;
using UnityEngine;

namespace _GAME.__Scripts.Controller
{
    public class CloudController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer[] _spriteRenderers;

        [SerializeField] private float _moveSpeed;
        private void OnEnable()
        {
            ZeroAlpha();
            SpriteFade();
        }

        private void Update()
        {
            if(GameManager.Instance.CurrentGameState != GameState.WaitingToStart) return;
            transform.Translate(transform.forward * (_moveSpeed * Time.deltaTime));
            transform.position = transform.position.WithX(0);
        }

        private void SpriteFade()
        {
            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                _spriteRenderers[i].DOFade(0.5f, 1);
            }
        }

        private void ZeroAlpha()
        {
            for (int i = 0; i < _spriteRenderers.Length; i++)
            {
                _spriteRenderers[i].color = new Color(_spriteRenderers[i].color.r, _spriteRenderers[i].color.g,
                    _spriteRenderers[i].color.b, 0);
            }
        }
    }
}
