using System;
using System.Collections.Generic;
using _GAME.__Scripts._Managers;
using UnityEngine;

namespace _GAME.__Scripts.Stickman
{
    public class StickmanEnemyParent : MonoBehaviour
    {
        public EnemyManager enemyManager;
        public List<StickmanData> stickmanDatas = new();
        public List<StickmanEnemy> stickmans = new();

        public int stickmanCount;
        

        public StickmanEnemy stickmanEnemy;

        public bool isFull;
        
        public bool isFirst;

        private void Awake()
        {
            enemyManager = GetComponentInParent<EnemyManager>();
        }

        private void FixedUpdate()
        {
            SetPosAndRot();
            
            SetHandTargetPositions(); // bura teklide yapılabilir
            
            GroupRotation();
        }

        private void SetPosAndRot()
        {
            if (stickmanCount <= 0) return;

            for (int i = 0; i < stickmanDatas[stickmanCount - 1].positions.Count; i++)
            {
                stickmans[i].transform.localPosition = stickmanDatas[stickmanCount - 1].positions[i];

                stickmans[i].transform.localRotation = Quaternion.Euler(stickmanDatas[stickmanCount - 1].rotations[i]);
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
                
                StickmanEnemy stickmanEnemy = Instantiate(this.stickmanEnemy);
                stickmanEnemy.ScaleUp();
                stickmanEnemy.enemyManager = enemyManager;
                stickmans.Add(stickmanEnemy);
                stickmanCount++;
                enemyManager.totalEnemyCount--;
                enemyManager.stickmanEnemies.Add(stickmanEnemy);
                stickmanEnemy.transform.SetParent(transform);
            }
        }


    }
}