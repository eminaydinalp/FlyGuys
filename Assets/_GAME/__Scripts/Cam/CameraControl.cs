using System;
using System.Collections;
using _GAME.__Scripts._Managers;
using Cinemachine;
using DG.Tweening;
using Rentire.Core;
using UnityEngine;

namespace _GAME.__Scripts.Cam
{
    public class CameraControl : Singleton<CameraControl>
    {
        public Camera mainCam;
        
        public CinemachineVirtualCamera winCam;
        public CinemachineVirtualCamera winCamMiddle;
        public CinemachineVirtualCamera playCam2;

        public Transform playerFollow;
        public Transform playerLookAt;

        public GameObject particul1;
        public GameObject particul2;

        public float cameraFollow;

        private CinemachineTransposer _transposer;

        private void Awake()
        {
            _transposer = playCam2.GetCinemachineComponent<CinemachineTransposer>();
        }

        private void Start()
        {
            cameraFollow = -10;
        }

        private void OnEnable()
        {
            GameManager.Instance.GameStatus.OnValueChange += GameStatusOnOnValueChangeRunning;
        }

        private void GameStatusOnOnValueChangeRunning()
        {
            DOVirtual.DelayedCall(0.5f, OpenPlayCam2);
            
        }

        private void FixedUpdate()
        {
            if(StickmanGroupManager.Instance.stickmanPlayerManagers.Count > 0) 
                playerFollow.position = Vector3.Lerp(playerFollow.position, StickmanGroupManager.Instance.stickmanPlayerManagers[0].transform.position, Time.deltaTime * 10);
        }

        private void Update()
        {
            cameraFollow = StickmanGroupManager.Instance.totalLevelCount switch
            {
                < 20 => -10,
                > 20 and < 30 => -11,
                > 30 and < 40 => -11,
                > 40 => -11,
                _ => cameraFollow
            };

            _transposer.m_FollowOffset = Vector3.Lerp(_transposer.m_FollowOffset,
                new Vector3(_transposer.m_FollowOffset.x, _transposer.m_FollowOffset.y, cameraFollow),
                Time.deltaTime * 0.5f);
        }


        public void OpenWinCam()
        {
            winCamMiddle.Follow = playerFollow;
            winCamMiddle.LookAt = playerLookAt;

            winCamMiddle.Priority = 15;

            StartCoroutine(OpenWinCamAsync());
        }

        IEnumerator OpenWinCamAsync()
        {
            yield return new WaitForSeconds(0.4f);
            
            winCam.Follow = playerFollow;
            winCam.LookAt = playerLookAt;

            winCam.Priority = 20;
        }
        private void OpenPlayCam2()
        {
            if (GameManager.Instance.CurrentGameState == GameState.Running)
            {
                //mainCam.DOShakeRotation(1, Vector3.one * 10f, vibrato:1).SetLoops(-1);
                playCam2.Priority = 12;

                //playCam2.GetComponent<CinemachineTransposer>().m_FollowOffset;
            }
        }

        public void CloseSpeedParticul()
        {
            particul1.SetActive(false);
            particul2.SetActive(false);
        }
    }
}