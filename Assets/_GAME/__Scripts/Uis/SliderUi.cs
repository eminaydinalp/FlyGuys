using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME.__Scripts.Uis
{
    public class SliderUi : MonoBehaviour
    {
        public Slider slider;

        public Transform startPoint;
        public Transform endPoint;
        private float currentDistance, totalDistance = 0;

        private bool isStart;

        private void OnEnable()
        {
            GameManager.Instance.GameStatus.OnValueChange += SetDelay;
        }

        private void SetDelay()
        {
            DOVirtual.DelayedCall(0.01f, SetPos);
        }

        private void SetPos()
        {
            if (GameManager.Instance.CurrentGameState == GameState.Running)
            {
                startPoint = Player.Instance.transform;
                endPoint = GameObject.FindGameObjectWithTag("Final").transform;
                totalDistance = Vector3.Distance(startPoint.position, endPoint.position);
                isStart = true;
            }
        }
        
        void Update()
        {
            if(!isStart) return;
            currentDistance = Vector3.Distance(Player.Instance.transform.position, endPoint.transform.position);
            slider.value = 1 - (currentDistance / totalDistance);
        }
    }
}