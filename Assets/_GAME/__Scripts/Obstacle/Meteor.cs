using System;
using UnityEngine;

namespace _GAME.__Scripts.Obstacle
{
    public class Meteor : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;


        private void Update()
        {
            if (GameManager.Instance.CurrentGameState == GameState.Running)
            {
                Move();
            }
        }

        private void Move()
        {
            transform.Translate(transform.forward * (_moveSpeed * Time.deltaTime));
        }
        
        
    }
}
