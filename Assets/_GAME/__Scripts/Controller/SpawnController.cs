using System;
using Lean.Pool;
using UnityEngine;

namespace _GAME.__Scripts.Controller
{
    public class SpawnController : MonoBehaviour
    {
        public float spawnTime;
        public float addZ;
        public float withY;
        [SerializeField] private Camera _camera;
        private float _currentTime;

        public bool isRunning;

       

        private void Start()
        {
            Spawn();
        }

        private void Update()
        {
            if(GameManager.Instance.CurrentGameState != GameState.WaitingToStart && GameManager.Instance.CurrentGameState != GameState.Running) return;
            _currentTime += Time.deltaTime;

            if (_currentTime >= spawnTime)
            {
                _currentTime = 0;
                
                Spawn();
            }
        }

        public void Spawn()
        {
            GameObject cloud = PoolManager.Instance.Spawn_Object(PoolsEnum.Cloud, _camera.transform.position.AddZ(addZ).WithY(withY), 
                Quaternion.identity, 3f);

            cloud.transform.forward = -_camera.transform.forward;
        }

    }
}
