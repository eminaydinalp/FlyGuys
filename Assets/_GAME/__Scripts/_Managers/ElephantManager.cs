using System;
using UnityEngine;
using ElephantSDK;
using Rentire.Core;

public class ElephantManager : Singleton<ElephantManager>,IGameStateObserver
{
    GameState previousState = GameState.WaitingToStart;
    int currentLevelNo => LevelManager.Instance.CurrentLevelNo;

    private void Awake()
    {
        AddToGameObserverList();
    }

    #region Level Events

    public void LevelCompleted(int _level)
    {
        Elephant.LevelCompleted(_level);
    }
    public void LevelStarted(int _level)
    {
        Elephant.LevelStarted(_level);
    }
    public void LevelFailed(int _level)
    {
        Elephant.LevelFailed(_level);
    }

    #endregion

    #region Enable / Disable / State Control

    public void AddToGameObserverList()
    {
        gameManager.AddListener(this);
    }

    public void OnGameStateChanged()
    {
        var state = gameState;

        if (state == GameState.Running)
        {
            LevelStarted(currentLevelNo);
        }

        else if (state == GameState.Success)
        {
            LevelCompleted(currentLevelNo);
        }

        else if (state == GameState.Fail)
        {
            LevelFailed(currentLevelNo);
        }

        previousState = state;
    }




    

    #endregion


}