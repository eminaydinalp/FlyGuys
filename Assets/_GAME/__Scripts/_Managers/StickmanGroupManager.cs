using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _GAME.__Scripts.Stickman;
using DG.Tweening;
using Rentire.Core;
using TMPro;
using UnityEngine;

namespace _GAME.__Scripts._Managers
{
    public class StickmanGroupManager : Singleton<StickmanGroupManager>
    {
        public PlayerMover playerMover;
        [SerializeField] private Transform _localMover;
        public TMP_Text levelNo;
        public TMP_Text ftNo;
        public SpriteRenderer levelSprite;

        public float ftNumber;

        public List<StickmanPlayerManager> stickmanPlayerManagers = new();

        public StickmanPlayerManager firstStickmanPlayerManager;

        public GameObject stickmanParent;

        public int totalLevelCount;

        public List<StickmanPlayer> allStickman = new();

        public int totalEnemyCount;

        public int leaveCount;
        public int currentLeaveCount;

        public bool isTrigger;

        private void Awake()
        {
            playerMover = GetComponentInParent<PlayerMover>();
        }

        private void OnEnable()
        {
            GameManager.Instance.GameStatus.OnValueChange += SetLevelNoTextInitial;
        }

        private void OnDisable()
        {
            //GameManager.Instance.GameStatus.OnValueChange -= SetLevelNoTextInitial;
        }

        private void Start()
        {
            totalLevelCount = firstStickmanPlayerManager.stickmanCount;
            //SetLevelNoText();
        }

        private void Update()
        {
            if (ftNo.gameObject.activeSelf)
            {
                ftNumber -= Time.deltaTime * 150;
                ftNo.text = ftNumber.ToString("0") + "FT";
            }
        }

        private void FixedUpdate()
        {
            if (allStickman.Count == 0)
            {
                GameManager.Instance.SetGameFail();
                levelNo.gameObject.SetActive(false);
                levelSprite.gameObject.SetActive(false);
                ftNo.gameObject.SetActive(false);
                return;
            }
            FollowParentCar();

            ArrangeStickmanManager();
            
            levelNo.gameObject.transform.localPosition = firstStickmanPlayerManager.transform.localPosition.AddY(1.5f).AddX(0.1f);
            levelSprite.gameObject.transform.localPosition = firstStickmanPlayerManager.transform.localPosition.AddY(1.2f);
            ftNo.gameObject.transform.localPosition = firstStickmanPlayerManager.transform.localPosition.AddY(1.1f).AddX(0.3f);
        }


        private void FollowParentCar()
        {
            for (int i = 0; i < stickmanPlayerManagers.Count; i++)
            {
                if (i == 0)
                {
                    stickmanPlayerManagers[i].transform.localPosition = Vector3.Lerp(
                        stickmanPlayerManagers[i].transform.localPosition,
                        new Vector3(_localMover.localPosition.x, _localMover.localPosition.y,
                            _localMover.localPosition.z - 0.5f), Time.deltaTime * 15f);
                    
                    stickmanPlayerManagers[i].transform.localRotation = Quaternion.Lerp(
                        stickmanPlayerManagers[i].transform.localRotation, 
                        Quaternion.Euler(_localMover.localEulerAngles.x, _localMover.localEulerAngles.y, (_localMover.localEulerAngles.z)  - 20f), 
                        Time.deltaTime * 15f);
                    
                }
                else
                {
                    stickmanPlayerManagers[i].transform.localPosition = Vector3.Lerp(
                        stickmanPlayerManagers[i].transform.localPosition,
                        new Vector3(stickmanPlayerManagers[i - 1].transform.localPosition.x,
                            _localMover.localPosition.y,
                            stickmanPlayerManagers[i - 1].transform.localPosition.z - 0.5f), Time.deltaTime * 15f);
                    
                    stickmanPlayerManagers[i].transform.localRotation = Quaternion.Lerp(
                        stickmanPlayerManagers[i].transform.localRotation, 
                        Quaternion.Euler(stickmanPlayerManagers[i - 1].transform.localEulerAngles.x, stickmanPlayerManagers[i - 1].transform.localEulerAngles.y, 
                            (stickmanPlayerManagers[i - 1].transform.localEulerAngles.z)  - 20f), 
                        Time.deltaTime * 15f);
                }
            }
        }

        private void AddFirstControl(int enemyCount)
        {
            totalEnemyCount = enemyCount;

            if (totalLevelCount > 50)
            {
                totalLevelCount += enemyCount;
                SetLevelNoText();
                StartCoroutine(SurgeEffect());
                
                return;
            }

            if (totalEnemyCount <= 0)
            {
                StartCoroutine(SurgeEffect());
                return;
            }

            if (!firstStickmanPlayerManager.isFull)
            {
                firstStickmanPlayerManager.AddStickman(totalEnemyCount);
            }

            if (firstStickmanPlayerManager.isFull && stickmanPlayerManagers.Count == 0)
            {
                StickmanPlayerManager newStickmanPlayerManager = CreateStickmanParent();
                newStickmanPlayerManager.AddStickman(totalEnemyCount);
            }

            if (stickmanPlayerManagers.Count > 0)
            {
                StickmanPlayerManager getStickmanPlayerManager = stickmanPlayerManagers.FirstOrDefault(x => !x.isFull);

                if (getStickmanPlayerManager != null)
                {
                    getStickmanPlayerManager.AddStickman(totalEnemyCount);
                }
                else
                {
                    StickmanPlayerManager newStickmanPlayerManager = CreateStickmanParent();
                    newStickmanPlayerManager.AddStickman(totalEnemyCount);
                }
            }

            AddFirstControl(totalEnemyCount);
        }

        private IEnumerator SurgeEffect()
        {
            if (stickmanPlayerManagers.Count > 0)
            {
                for (int i = 0; i < firstStickmanPlayerManager.stickmans.Count; i++)
                {
                    firstStickmanPlayerManager.stickmans[i].transform.DOPunchScale(Vector3.one * 0.2f, 0.2f);
                }

                yield return new WaitForSeconds(0.1f);

                for (int k = 0; k < stickmanPlayerManagers.Count; k++)
                {
                    for (int j = 0; j < stickmanPlayerManagers[k].stickmans.Count; j++)
                    {
                        stickmanPlayerManagers[k].stickmans[j].transform.DOPunchScale(Vector3.one * 0.2f, 0.2f);
                    }

                    yield return new WaitForSeconds(0.1f);

                    // if (k == stickmanPlayerManagers.Count - 1)
                    // {
                    //     SetDefaultScale();
                    // }
                }
            }
        }

        private void SetDefaultScale()
        {
            for (int i = 0; i < firstStickmanPlayerManager.stickmans.Count; i++)
            {
                firstStickmanPlayerManager.stickmans[i].transform.localScale = Vector3.one;
            }

            for (int i = 0; i < stickmanPlayerManagers.Count; i++)
            {
                for (int j = 0; j < stickmanPlayerManagers[i].stickmans.Count; j++)
                {
                    stickmanPlayerManagers[i].stickmans[j].transform.localScale = Vector3.one;
                }
            }
        }

        private StickmanPlayerManager CreateStickmanParent()
        {
            StickmanPlayerManager stickmanPlayerManager = Instantiate(this.stickmanParent, transform).GetComponentInChildren<StickmanPlayerManager>();
            stickmanPlayerManagers.Add(stickmanPlayerManager);

            return stickmanPlayerManager;
        }

        public void GeneralControl(int enemyCount)
        {
            if (isTrigger) return;

            isTrigger = true;

            DOVirtual.DelayedCall(0.5f, () => isTrigger = false);

            if (totalLevelCount >= enemyCount)
            {
                AddFirstControl(enemyCount);
            }
            else
            {
                ShakeManager.Instance.CameraShake(ShakeType.ObstacleCollision);
                currentLeaveCount = leaveCount;
                LeaveControl();
            }
        }

        private void LeaveControl()
        {
            if (totalLevelCount > 35)
            {
                totalLevelCount -= currentLeaveCount;
                SetLevelNoText();
                return;
            }
            if (currentLeaveCount <= 0 || allStickman.Count == 0) return;

            if (stickmanPlayerManagers.Count == 0 && firstStickmanPlayerManager.stickmanCount > 0)
            {
                firstStickmanPlayerManager.RemoveStickman(currentLeaveCount);
            }
            else
            {
                stickmanPlayerManagers[^1].RemoveStickman(currentLeaveCount);
            }

            LeaveControl();
        }

        private void SetLevelNoTextInitial()
        {
            
            if (GameManager.Instance.CurrentGameState == GameState.Running)
            {
                levelNo.text = "Lv. " + totalLevelCount;
                levelNo.gameObject.SetActive(true);
                levelSprite.gameObject.SetActive(true);
                ftNo.gameObject.SetActive(true);
                
            }
        }

        public void SetLevelNoText()
        {
            levelNo.text = "Lv. " + totalLevelCount;
        }

        private void ChangePlaceStickmansAsync()
        {
            if (stickmanPlayerManagers.Count > 0)
            {
                int totalCount = allStickman.Count;
                
                totalLevelCount = allStickman.Count;
                SetLevelNoText();
              
                firstStickmanPlayerManager.stickmans.Clear();
                firstStickmanPlayerManager.stickmanCount = 0;

                for (int i = 0; i < 5; i++)
                {
                    if (totalCount == 0) return;
                    firstStickmanPlayerManager.AddStickmanFromBack(allStickman[totalCount - 1]);
                    totalCount--;
                    if (firstStickmanPlayerManager.stickmanCount == 5) firstStickmanPlayerManager.isFull = true;
                }


                if (totalCount == 0) return;

                ClearStickmanManager();

                for (int i = 0; i < stickmanPlayerManagers.Count; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (totalCount == 0) return;
                        stickmanPlayerManagers[i].AddStickmanFromBack(allStickman[totalCount - 1]);
                        totalCount--;
                        if (stickmanPlayerManagers[i].stickmanCount == 5) stickmanPlayerManagers[i].isFull = true;
                    }
                }

            }
        }

        public void ChangePlaceStickmans()
        {
            CancelInvoke(nameof(ChangePlaceStickmansAsync));
            Invoke(nameof(ChangePlaceStickmansAsync), 0.2f);
        }

        private void ClearStickmanManager()
        {
            for (int i = 0; i < stickmanPlayerManagers.Count; i++)
            {
                stickmanPlayerManagers[i].stickmans.Clear();
                stickmanPlayerManagers[i].stickmanCount = 0;
                stickmanPlayerManagers[i].isFull = false;
            }
        }

        private void ArrangeStickmanManager()
        {
            for (int i = 0; i < stickmanPlayerManagers.Count; i++)
            {
                if (stickmanPlayerManagers[i].stickmanCount <= 0)
                {
                    stickmanPlayerManagers.Remove(stickmanPlayerManagers[i]);
                }
            }
        }
    }
}