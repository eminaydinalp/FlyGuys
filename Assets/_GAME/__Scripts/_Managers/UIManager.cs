using System;
using Rentire.Core;

public class UIManager : Singleton<UIManager>,IGameStateObserver
{
    private void Awake()
    {
        AddToGameObserverList();
    }
    

    #region Level Buttons

    public void Button_NextLevel()
    {
        LevelManager.Instance.NextLevel();
    }

    public void Button_RestartLevel()
    {
        LevelManager.Instance.RestartLevel();
    }

    #endregion


    public void AddToGameObserverList()
    {
        gameManager.AddListener(this);
    }

    public void OnGameStateChanged()
    {
        
    }
}
