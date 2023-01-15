using System;
using DG.Tweening;
using UnityEngine;

namespace _GAME.__Scripts.Effect
{
    public class SmokeEffect : MonoBehaviour
    {
        public ParticleSystem[] smokes;

        private void OnEnable()
        {
            GameManager.Instance.GameStatus.OnValueChange += OpenSmoke;
        }
        
        private void OnDisable()
        {
            //GameManager.Instance.GameStatus.OnValueChange -= OpenSmoke;
        }

        private void OpenSmoke()
        {
            if (GameManager.Instance.CurrentGameState == GameState.Running)
            {
                DOVirtual.DelayedCall(1f, OpenSmokeAsync);

            }
            
        }

        private void OpenSmokeAsync()
        {
            for (int i = 0; i < smokes.Length; i++)
            {
                smokes[i].Play();
            }
        }
    }
}
