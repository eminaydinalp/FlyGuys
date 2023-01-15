using System;
using DG.Tweening;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] private GameObject body;
    [SerializeField] private bool isPlaneOrBird;
    
    public float playerOffset;

    private void Start()
    {
        body.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.Instance.GameStatus.OnValueChange += CreateEnemies; 
    }

    private void Update()
    {
        if(!isPlaneOrBird) return;
        
        if (Player.Instance.transform.position.z + playerOffset > transform.position.z)
        {
            body.SetActive(true);
        }
        
        if (Player.Instance.transform.position.z > playerOffset + transform.position.z)
        {
            gameObject.SetActive(false);
        }
    }

    private void CreateEnemies()
    {
        DOVirtual.DelayedCall(0.01f, CallbackCreateObstacle);

    }

    private void CallbackCreateObstacle()
    {
        if (GameManager.Instance.CurrentGameState == GameState.Running && !isPlaneOrBird)
        {
            body.SetActive(true);
        }
    }
}
