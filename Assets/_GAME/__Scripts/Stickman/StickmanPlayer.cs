using System.Collections;
using _GAME.__Scripts._Managers;
using _GAME.__Scripts.Cam;
using _GAME.__Scripts.Controller;
using _GAME.__Scripts.Obstacle;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Random = System.Random;

namespace _GAME.__Scripts.Stickman
{
    public class StickmanPlayer : MonoBehaviour
    {
        public StickmanPlayerManager stickmanPlayerManager;
        public BoxCollider boxCollider;
        public Rigidbody[] rigidbodies;
        private StickmanEnemy _stickmanEnemy;
        public bool _isFall;
        
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform rightHandTarget;
        [SerializeField] private Transform leftHandTarget;

        [SerializeField] private RigBuilder _rigBuilder;

        [SerializeField] private Renderer mesh;
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                FeedbackManager.Instance.Vibrate(HapticTypes.Warning);
                TriggerEnemy(other);
            }
            

            if (other.CompareTag("Obstacle"))
            {
                FeedbackManager.Instance.Vibrate(HapticTypes.Warning);
                TriggerObstacle();
            }

            if (other.CompareTag("Final"))
            {
                StickmanGroupManager.Instance.totalLevelCount = StickmanGroupManager.Instance.allStickman.Count;
                CameraControl.Instance.CloseSpeedParticul();
                FeedbackManager.Instance.Vibrate(HapticTypes.Success);

                StickmanGroupManager.Instance.levelNo.gameObject.SetActive(false);
                StickmanGroupManager.Instance.levelSprite.gameObject.SetActive(false);
                StickmanGroupManager.Instance.ftNo.gameObject.SetActive(false);
                GameManager.Instance.isFinal = true;
                StickmanGroupManager.Instance.playerMover.splineFollower.followSpeed = 5;
                CameraControl.Instance.OpenWinCam();
            }

            if (other.CompareTag("FinalObstacle"))
            {
                if(stickmanPlayerManager.isTriggerFinalObstacle) return;
                stickmanPlayerManager.isTriggerFinalObstacle = true;
                FeedbackManager.Instance.Vibrate(HapticTypes.Success);

                FinalObstacleParent finalObstacle = other.GetComponent<FinalObstacleParent>();

                
                Physics.gravity = Vector3.down * 9.81f;
                finalObstacle.DoBreakupParent();
                FinalObstacleTrigger();
                
                if(StickmanGroupManager.Instance.stickmanPlayerManagers.Count <= 1) return;
                StickmanGroupManager.Instance.stickmanPlayerManagers.Remove(StickmanGroupManager.Instance
                    .stickmanPlayerManagers[0]);
            }
        }
        
        public void ScaleUp()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.5f);
            StartCoroutine(SetScaleDefault());
        }

        private IEnumerator SetScaleDefault()
        {
            yield return new WaitForSeconds(1f);
            transform.localScale = Vector3.one;
        }
        
        public void SelfDestroy()
        {
            boxCollider.enabled = false;
            
            var random = new Random();

            foreach (var rigidbody in rigidbodies)
            {
                _animator.enabled = false;
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                
                rigidbody.AddExplosionForce(10, transform.localPosition, 2);
                rigidbody.AddForce(new Vector3(random.Next(-5, 5), 10, 0), ForceMode.Impulse);
            }
            
            transform.parent = null;

            _isFall = true;

            DOVirtual.DelayedCall(2, () => gameObject.SetActive(false));

        }

        private void TriggerObstacle()
        {
            stickmanPlayerManager.TriggerObstacle(this);
            
            SelfDestroy();
        }

        private void TriggerEnemy(Collider other)
        {
            if (_isFall)
            {
                return;
            }
                
            _stickmanEnemy = other.GetComponent<StickmanEnemy>();
                
            StickmanGroupManager.Instance.GeneralControl(_stickmanEnemy.enemyManager.stickmanLevelCount);
                
            _stickmanEnemy.enemyManager.gameObject.SetActive(false);
        }

        public void SetHandPosition(Vector3 rightPosition, Vector3 leftPosition)
        {
            rightHandTarget.localPosition = rightPosition;
            leftHandTarget.localPosition = leftPosition;
        }

        public void SetColor(Color color)
        {
            //Color color =  ColorController.Instance.gradient.Evaluate(evaluate);

            mesh.material.SetColor(BaseColor, color);
        }

        private void FinalObstacleTrigger()
        {
            for (int i = 0; i < stickmanPlayerManager.stickmans.Count; i++)
            {
                stickmanPlayerManager.stickmans[i].SelfDestroy2();
            }
        }
        public void SelfDestroy2()
        {
            if(_isFall) return;
            _isFall = true;

            Debug.Log("self destroy2");
            boxCollider.enabled = false;
            
            var random = new Random();

            foreach (var rigidbody in rigidbodies)
            {
                _animator.enabled = false;
                _rigBuilder.enabled = false;
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                
                rigidbody.AddExplosionForce(3, transform.localPosition, 2);
                rigidbody.AddForce(new Vector3(random.Next(-10, 10), 10, -10), ForceMode.Impulse);
            }
            
            //transform.parent = null;

            _isFall = true;
            
            StickmanGroupManager.Instance.totalLevelCount--;
            
            if (StickmanGroupManager.Instance.totalLevelCount <= 0)
            {
                StickmanGroupManager.Instance.playerMover.splineFollower.enabled = false;
                StickmanGroupManager.Instance.playerMover.Rigidbody.isKinematic = true;
                StickmanGroupManager.Instance.levelNo.gameObject.SetActive(false);
                StickmanGroupManager.Instance.levelSprite.gameObject.SetActive(false);
                StickmanGroupManager.Instance.playerMover.isStop = true;
                StickmanGroupManager.Instance.playerMover.splineFollower.followSpeed = 0;
                
                FinalController.Instance.DoFinalTextFade();
                
                StartCoroutine(Win());
            }

            DOVirtual.DelayedCall(2, () => gameObject.SetActive(false));

        }

        private IEnumerator Win()
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("Success");
            GameManager.Instance.SetGameSuccess();
        }


    }
}