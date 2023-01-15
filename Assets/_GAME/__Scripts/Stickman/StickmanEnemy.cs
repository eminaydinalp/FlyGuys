using _GAME.__Scripts._Managers;
using DG.Tweening;
using UnityEngine;

namespace _GAME.__Scripts.Stickman
{
    public class StickmanEnemy : MonoBehaviour
    {
        public EnemyManager enemyManager;
        
        [SerializeField] private Transform rightHandTarget;
        [SerializeField] private Transform leftHandTarget;

        public SkinnedMeshRenderer bodyMaterial;
        public void SetHandPosition(Vector3 rightPosition, Vector3 leftPosition)
        {
            rightHandTarget.localPosition = rightPosition;
            leftHandTarget.localPosition = leftPosition;
        }

        public void SetColor(Color color)
        {
            bodyMaterial.material.SetColor("_BaseColor", color);
        }
        
        public void ScaleUp()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.5f);
        }
        
        
    }
}
