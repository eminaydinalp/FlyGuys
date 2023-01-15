using System;
using Rentire.Core;

public class FinalManager : Singleton<FinalManager>,IGameStateObserver
{
    private void Awake()
    {
        AddToGameObserverList();
    }

    void Start()
    {
        
    }

    void SetVictory()
    {
        gameManager.SetGameSuccess();
    }
    
    public void AddToGameObserverList()
    {
        gameManager.AddListener(this);
    }

    public void OnGameStateChanged()
    {
        if (gameState == GameState.Final)
        {
            CallMethodWithDelay(SetVictory,1f);
        }
    }
}
