using System.Collections.Generic;
using _GAME.__Scripts.Controller;
using _GAME.__Scripts.Stickman;
using UnityEngine;

namespace _GAME.__Scripts._Managers
{
    public class StickmanPlayerManager : MonoBehaviour
    {
        public List<StickmanData> stickmanDatas = new();
        public List<StickmanPlayer> stickmans = new();

        public int stickmanCount;
        

        public StickmanPlayer stickmanPlayer;

        public bool isFull;

        public bool isFirst;

        public bool isTriggerFinalObstacle;


        private void Start()
        {
            FirstCreateStickmans();
        }

        private void FixedUpdate()
        {
            SetPosAndRot();
            
            ChildControl();
            
            SetHandTargetPositions();
            
            GroupRotation();
            
            UpdateColor();
        }

        private void SetPosAndRot()
        {
            if (stickmanCount <= 0) return;

            for (int i = 0; i < stickmanDatas[stickmanCount - 1].positions.Count; i++)
            {
                stickmans[i].transform.localPosition = Vector3.Lerp(stickmans[i].transform.localPosition,
                    stickmanDatas[stickmanCount - 1].positions[i], Time.deltaTime * 4f);

                stickmans[i].transform.localRotation = Quaternion.Lerp(stickmans[i].transform.localRotation,
                    Quaternion.Euler(stickmanDatas[stickmanCount - 1].rotations[i]), Time.deltaTime * 4f);
            }
        }

        private void ChildControl()
        {
            if (transform.childCount == 0)
            {
                stickmanCount = 0;
                stickmans.Clear();
            }
        }

        private void SetHandTargetPositions()
        {
            if (stickmanCount <= 0) return;

            for (int i = 0; i < stickmanDatas[stickmanCount - 1].rightTargetPositions.Count; i++)
            {
                stickmans[i].SetHandPosition(stickmanDatas[stickmanCount - 1].rightTargetPositions[i], stickmanDatas[stickmanCount - 1].leftTargetPositions[i]);
            }
        }

        private void GroupRotation()
        {
            if(!isFirst || GameManager.Instance.CurrentGameState != GameState.Running) return;

            transform.Rotate(Vector3.forward * (10 * Time.deltaTime));
        }


        private void FirstCreateStickmans()
        {
            if (!isFirst) return;
            for (int i = 0; i < stickmanCount; i++)
            {
                StickmanPlayer stickmanPlayer = Instantiate(this.stickmanPlayer);
                stickmanPlayer.stickmanPlayerManager = this;
                stickmans.Add(stickmanPlayer);
                StickmanGroupManager.Instance.allStickman.Add(stickmanPlayer);
                stickmanPlayer.transform.SetParent(transform);

                stickmanPlayer.transform.localPosition = stickmanDatas[i].positions[i];
                stickmanPlayer.transform.localRotation = Quaternion.Euler(stickmanDatas[i].rotations[i]);
            }
        }

        public void AddStickman(int stickmanCountValue)
        {
            for (int i = 0; i < stickmanCountValue; i++)
            {
                if (isFull) break;

                if (stickmanCount >= 5)
                {
                    isFull = true;
                    break;
                }
                
                StickmanPlayer stickmanPlayer = Instantiate(this.stickmanPlayer);
                stickmanPlayer.stickmanPlayerManager = this;
                stickmans.Add(stickmanPlayer);
                StickmanGroupManager.Instance.allStickman.Add(stickmanPlayer);
                stickmanCount++;
                StickmanGroupManager.Instance.totalEnemyCount--;
                StickmanGroupManager.Instance.totalLevelCount++;
                StickmanGroupManager.Instance.SetLevelNoText();
                stickmanPlayer.transform.SetParent(transform);
                stickmanPlayer.transform.localPosition = stickmanDatas[stickmanCount - 1].positions[^1];
                stickmanPlayer.transform.localRotation =
                    Quaternion.Euler(stickmanDatas[stickmanCount - 1].rotations[^1]);
                stickmanPlayer.ScaleUp();
            }
        }

        public void RemoveStickman(int leaveCount)
        {
            for (int i = 0; i < leaveCount; i++)
            {
                StickmanGroupManager.Instance.currentLeaveCount--;
                StickmanGroupManager.Instance.allStickman.Remove(stickmans[^1]);
                stickmans[^1].SelfDestroy();
                stickmans.Remove(stickmans[^1]);
                stickmanCount--;
                StickmanGroupManager.Instance.totalLevelCount--;
                StickmanGroupManager.Instance.SetLevelNoText();
                isFull = false;
                if (stickmanCount <= 0)
                {
                    if (isFirst)
                    {
                        Debug.Log("Fail");
                        StickmanGroupManager.Instance.levelNo.gameObject.SetActive(false);
                        StickmanGroupManager.Instance.levelSprite.gameObject.SetActive(false);
                        StickmanGroupManager.Instance.ftNo.gameObject.SetActive(false);
                        GameManager.Instance.SetGameFail();
                    }
                    else
                    {
                        StickmanGroupManager.Instance.stickmanPlayerManagers.Remove(this);
                        //gameObject.SetActive(false);
                    }

                    break;
                }
            }
        }

        public void TriggerObstacle(StickmanPlayer stickmanPlayer)
        {
            stickmans.Remove(stickmanPlayer);
            StickmanGroupManager.Instance.allStickman.Remove(stickmanPlayer);
            stickmanCount--;
            StickmanGroupManager.Instance.totalLevelCount--;
            StickmanGroupManager.Instance.SetLevelNoText();
            
            isFull = false;
 
            if (StickmanGroupManager.Instance.allStickman.Count <= 0)
            {
                GameManager.Instance.SetGameFail();
                StickmanGroupManager.Instance.levelNo.gameObject.SetActive(false);
                StickmanGroupManager.Instance.levelSprite.gameObject.SetActive(false);
                StickmanGroupManager.Instance.ftNo.gameObject.SetActive(false);
            }


            StickmanGroupManager.Instance.ChangePlaceStickmans();
        }

        public void AddStickmanFromBack(StickmanPlayer stickmanPlayer)
        {
            stickmanPlayer.transform.localScale = Vector3.one;
            stickmanPlayer.stickmanPlayerManager = this;
            stickmans.Add(stickmanPlayer);
            stickmanCount++;
            stickmanPlayer.transform.SetParent(transform);
            stickmanPlayer.transform.localPosition = stickmanDatas[stickmanCount - 1].positions[^1];
            stickmanPlayer.transform.localRotation = Quaternion.Euler(stickmanDatas[stickmanCount - 1].rotations[^1]);
            //stickmanPlayer.ScaleUp();
        }

        public void UpdateColor()
        {
            if (isFirst)
            {
                for (int i = 0; i < stickmans.Count; i++)
                {
                    stickmans[i].SetColor(ColorController.Instance.colors[0]);
                }
            }
            else
            {
                for (int i = 0; i < stickmans.Count; i++)
                {
                    stickmans[i].SetColor(ColorController.Instance.colors[StickmanGroupManager.Instance.stickmanPlayerManagers.IndexOf(this) + 1]);
                }
            }
            // if (isFirst)
            // {
            //     for (int i = 0; i < stickmans.Count; i++)
            //     {
            //         stickmans[i].SetColor(1);
            //     }
            // }
            // else
            // {
            //     for (int i = 0; i < stickmans.Count; i++)
            //     {
            //         stickmans[i].SetColor(CreateEvaluateValue());
            //     }
            // }
        }

        private float CreateEvaluateValue()
        {
            int totalStickmanGroup = StickmanGroupManager.Instance.stickmanPlayerManagers.Count + 1;

            float distance = (1 / (float)totalStickmanGroup);
            
            float result = 1 - (distance * (StickmanGroupManager.Instance.stickmanPlayerManagers.IndexOf(this) + 1));
            
            return result;

        }
    }


    [System.Serializable]
    public class StickmanData
    {
        [Header("Stickman Data")] public List<Vector3> positions;
        public List<Vector3> rotations;
        public List<Vector3> rightTargetPositions;
        public List<Vector3> leftTargetPositions;
    }
}