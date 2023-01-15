using System;
using System.Collections;
using System.Collections.Generic;
using Rentire.Core;
using Sirenix.OdinInspector;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    public readonly ObservedValue<GameState> GameStatus = new ObservedValue<GameState>(GameState.WaitingToStart);
    public GameState CurrentGameState = GameState.WaitingToStart;
    public IList<IGameStateObserver> gameStateObserverList;

    public bool isFinal;
    
    private void Start()
    {
        Physics.gravity = Vector3.back * 9.81f;
        CurrentGameState = GameState.WaitingToStart;
    }

    private void Update()
    {
        if (CurrentGameState == GameState.Running && Time.frameCount % 15 == 0)
        {
            ShakeManager.Instance.CameraShake(ShakeType.Light);
        }
    }

    public void AddListener(IGameStateObserver gameStateObserver)
    {
        gameStateObserverList ??=new List<IGameStateObserver>();
        if(!gameStateObserverList.Contains(gameStateObserver))
            gameStateObserverList.Add(gameStateObserver);
    }

    /// <summary>
    /// Change game state to Running
    /// </summary>
    public void SetGameRunning()
    {
        SetGameState(GameState.Running);
    }

    /// <summary>
    /// Change Game state to Success
    /// </summary>
    public void SetGameSuccess()
    {
        
        SetGameState(GameState.Success);

    }
    /// <summary>
    /// Change Game state to Final
    /// </summary>
    public void SetGameFinal()
    {
        if (gameState != GameState.Running)
            return;
        SetGameState(GameState.Final);
    }

    /// <summary>
    /// Change Game state to Fail
    /// </summary>
    public void SetGameFail()
    {
        if (gameState != GameState.Running)
        {
            return;
        }

        SetGameState(GameState.Fail);
    }

    public void SetGameState(GameState gameState)
    {
        GameStatus.Value = gameState;

        Log.Info("Current game state is set to " + gameState);
    }


    private void GameStateChanged()
    {
        CurrentGameState = GameStatus.Value;
        if(gameStateObserverList != null)
            for (int i = 0; i < gameStateObserverList.Count; i++)
            {
                if(gameStateObserverList[i] != null)
                    gameStateObserverList[i].OnGameStateChanged();
            }
        if (CurrentGameState == GameState.Success)
        {
            LevelManager.Instance.IncreaseLevelNo();

        }
    }

    private void OnEnable()
    {
        GameStatus.OnValueChange += GameStateChanged;
    }

    private void OnDisable()
    {
        GameStatus.OnValueChange -= GameStateChanged;
    }

}
