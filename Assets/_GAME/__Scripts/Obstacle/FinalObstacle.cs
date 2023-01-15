using UnityEngine;
using Random = UnityEngine.Random;

namespace _GAME.__Scripts.Obstacle
{
    public class FinalObstacle : MonoBehaviour
    {
        [SerializeField] private FinalObstacleParent _finalObstacleParent;
        [SerializeField] private Rigidbody _rigidbody;
        public bool isTrigger;

        private void Awake()
        {
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }

        public void DoBreakup()
        {
            Debug.Log("Breakup Obstacle");
            isTrigger = true;
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = true;
            _rigidbody.AddExplosionForce(3, transform.position, 2);
            _rigidbody.AddForce(new Vector3(Random.Range(-10, 10), 10, 0), ForceMode.Impulse);
        }
    }
}