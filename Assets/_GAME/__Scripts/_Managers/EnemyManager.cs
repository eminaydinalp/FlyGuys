using System;
using System.Collections.Generic;
using System.Linq;
using _GAME.__Scripts.Stickman;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace _GAME.__Scripts._Managers
{
    public class EnemyManager : MonoBehaviour
    {
        public List<StickmanEnemyParent> StickmanPlayerEnemyParents = new();

        public StickmanEnemyParent FirstStickmanEnemyParent;
        
        public StickmanEnemyParent stickmanParent;

        public int totalEnemyCount;

        public List<StickmanData> stickmanDatas = new();

        public Transform parent;

        public int stickmanLevelCount;

        public TMP_Text levelNo;

        public SpriteRenderer spriteRenderer;

        public StickmanEnemy stickmanEnemy;

        public List<Difficulty> Diffulties = new List<Difficulty>();

        public EnemyDifficult enemyDifficult;

        public int minLevelCount;
        public int maxLevelCount;

        public float playerDistance;

        public bool isNear;

        Random _random = new();

        public List<StickmanEnemy> stickmanEnemies = new List<StickmanEnemy>();

        private void OnEnable()
        {
            GameManager.Instance.GameStatus.OnValueChange += CreateEnemies;
        }

        private void CreateEnemies()
        {
            //DOVirtual.DelayedCall(0.01f, CallbackCreateEnemy);
        }

        private void CallbackCreateEnemy()
        {
            if (GameManager.Instance.CurrentGameState == GameState.Running)
            {
                int stickmanCount = _random.Next(1, 5);


                for (int i = 0; i < stickmanDatas[stickmanCount - 1].positions.Count; i++)
                {
                    StickmanEnemy stickmanEnemy = Instantiate(this.stickmanEnemy);

                    stickmanEnemies.Add(stickmanEnemy);
                    stickmanEnemy.SetHandPosition(stickmanDatas[stickmanCount - 1].rightTargetPositions[i],
                        stickmanDatas[stickmanCount - 1].leftTargetPositions[i]);

                    stickmanEnemy.transform.SetParent(parent);
                    //stickmanEnemy.transform.parent = transform;

                    stickmanEnemy.transform.localPosition = stickmanDatas[stickmanCount - 1].positions[i];
                    stickmanEnemy.transform.localRotation =
                        Quaternion.Euler(stickmanDatas[stickmanCount - 1].rotations[i]);

                    stickmanEnemy.enemyManager = transform.GetComponent<EnemyManager>();
                }
            }
        }

        private void FixedUpdate()
        {
            FollowParentCar();
        }


        private void Update()
        {
            if (GameManager.Instance.CurrentGameState != GameState.Running) return;
            ChangeColor();

            if (Player.Instance.transform.position.z + playerDistance >= transform.position.z)
            {
                UpdateStickmanCount();
            }

            if (Player.Instance.transform.position.z > transform.position.z + playerDistance)
            {
                gameObject.SetActive(false);
            }

            GroupRotation();
            //
            // levelNo.transform.localPosition = parent.transform.localPosition;
            // spriteRenderer.transform.localPosition = parent.transform.localPosition;
        }

        private void SetLevelNoText()
        {
            spriteRenderer.gameObject.SetActive(true);
            levelNo.gameObject.SetActive(true);
            levelNo.text = stickmanLevelCount.ToString();
        }

        private void ChangeColor()
        {
            
            Color color = StickmanGroupManager.Instance.totalLevelCount >= stickmanLevelCount
                ? Color.green
                : Color.red;

            spriteRenderer.color = color;
            

            for (int i = 0; i < stickmanEnemies.Count; i++)
            {
                stickmanEnemies[i].SetColor(color);
            }
        }

        private void UpdateStickmanCount()
        {
            if (isNear) return;
            isNear = true;
            if (StickmanGroupManager.Instance.totalLevelCount >= maxLevelCount)
            {
                this.enemyDifficult = EnemyDifficult.Hard;
            }

            if (StickmanGroupManager.Instance.totalLevelCount > minLevelCount &&
                StickmanGroupManager.Instance.totalLevelCount < maxLevelCount)
            {
                this.enemyDifficult = EnemyDifficult.Medium;
            }

            if (StickmanGroupManager.Instance.totalLevelCount <= minLevelCount)
            {
                this.enemyDifficult = EnemyDifficult.Easy;
            }

            Difficulty difficulty = Diffulties.FirstOrDefault(x => x.enemyDifficult == this.enemyDifficult);
            stickmanLevelCount = _random.Next(Mathf.RoundToInt(difficulty.diffMultiplierAmount.x),
                Mathf.RoundToInt(difficulty.diffMultiplierAmount.y));

            int levelCount;
            if (stickmanLevelCount > 20)
            {
                levelCount = 20;
            }
            else
            {
                levelCount = stickmanLevelCount;
            }
            
            AddFirstControl(levelCount);

            SetLevelNoText();
        }

        private void GroupRotation()
        {
            FirstStickmanEnemyParent.transform.Rotate(Vector3.forward * (10 * Time.deltaTime));
        }

        private void FollowParentCar()
        {
            for (int i = 0; i < StickmanPlayerEnemyParents.Count; i++)
            {
                if (i == 0)
                {
                    StickmanPlayerEnemyParents[i].transform.localPosition =
                        new Vector3(FirstStickmanEnemyParent.transform.localPosition.x,
                            FirstStickmanEnemyParent.transform.localPosition.y,
                            FirstStickmanEnemyParent.transform.localPosition.z + 0.5f);

                    StickmanPlayerEnemyParents[i].transform.localRotation = 
                        Quaternion.Euler(FirstStickmanEnemyParent.transform.localEulerAngles.x,
                            FirstStickmanEnemyParent.transform.localEulerAngles.y,
                            (FirstStickmanEnemyParent.transform.localEulerAngles.z) - 20f);
                }
                else
                {
                    StickmanPlayerEnemyParents[i].transform.localPosition =
                        new Vector3(StickmanPlayerEnemyParents[i - 1].transform.localPosition.x,
                            StickmanPlayerEnemyParents[i - 1].transform.localPosition.y,
                            StickmanPlayerEnemyParents[i - 1].transform.localPosition.z + 0.5f);

                    StickmanPlayerEnemyParents[i].transform.localRotation = 
                        Quaternion.Euler(StickmanPlayerEnemyParents[i - 1].transform.localEulerAngles.x,
                            StickmanPlayerEnemyParents[i - 1].transform.localEulerAngles.y,
                            (StickmanPlayerEnemyParents[i - 1].transform.localEulerAngles.z) - 20f);
                }
                
            }
        }
        
        
        private void AddFirstControl(int enemyCount)
        {
            totalEnemyCount = enemyCount;
        
            if (totalEnemyCount <= 0)
            {
                return;
            }
        
            if (!FirstStickmanEnemyParent.isFull)
            {
                FirstStickmanEnemyParent.AddStickman(totalEnemyCount);
            }
        
            if (FirstStickmanEnemyParent.isFull && StickmanPlayerEnemyParents.Count == 0)
            {
                StickmanEnemyParent newStickmanPlayerManager = CreateStickmanParent();
                newStickmanPlayerManager.AddStickman(totalEnemyCount);
            }
        
            if (StickmanPlayerEnemyParents.Count > 0)
            {
                StickmanEnemyParent getStickmanPlayerManager = StickmanPlayerEnemyParents.FirstOrDefault(x => !x.isFull);
        
                if (getStickmanPlayerManager != null)
                {
                    getStickmanPlayerManager.AddStickman(totalEnemyCount);
                }
                else
                {
                    StickmanEnemyParent newStickmanPlayerManager = CreateStickmanParent();
                    newStickmanPlayerManager.AddStickman(totalEnemyCount);
                }
            }
        
            AddFirstControl(totalEnemyCount);
        }
        
        private StickmanEnemyParent CreateStickmanParent()
        {
            StickmanEnemyParent stickmanPlayerManager = Instantiate(this.stickmanParent, transform);
            StickmanPlayerEnemyParents.Add(stickmanPlayerManager);

            return stickmanPlayerManager;
        }
    }

    [Serializable]
    public class Difficulty
    {
        [MinMaxSlider(1, 100f, true)] public Vector2 diffMultiplierAmount;
        public EnemyDifficult enemyDifficult;
    }
}