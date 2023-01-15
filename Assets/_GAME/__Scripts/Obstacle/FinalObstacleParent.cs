using System;
using _GAME.__Scripts.Controller;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _GAME.__Scripts.Obstacle
{
    public class FinalObstacleParent : MonoBehaviour
    {
        [SerializeField] private FinalObstacle[] finalObstacles;
        [SerializeField] private BoxCollider boxCollider;

        [SerializeField] private TMP_Text finalText;

        [SerializeField] private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponentInParent<Renderer>();
        }

        public void DoBreakupParent()
        {
            FinalController.Instance.finalObstacleParent = this;
            
            boxCollider.enabled = false;
            
            for (int i = 0; i < finalObstacles.Length; i++)
            {
                finalObstacles[i].DoBreakup(); 
            }
        }

        public void FinalTextFade()
        {
            // finalText.color = Color.green;
            // finalText.DOFade(0.5f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            // finalText.transform.DOScale(Vector3.one * 1.2f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            //_renderer.material.DOFade(0.5f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
            _renderer.material.DOFade(0.5f, 1f)
                .OnComplete((() => _renderer.material.DOFade(1f, 1f)));

            //_renderer.transform.DOScale(Vector3.one * 1.2f, 1f).OnComplete((() => _renderer.transform.DOScale(Vector3.one , 1f)));
            _renderer.transform.DOPunchScale(Vector3.one * 0.2f, 2);
        }
    }
}