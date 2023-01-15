using System;
using System.Collections.Generic;
using Rentire.Core;
using UnityEngine;

public class PlayerStateManager : RMonoBehaviour, IGameStateObserver
{
    public PlayerState currentPlayerState;
    private readonly ObservedValue<PlayerState> playerState = new(PlayerState.Idle);
    public IList<IPlayerStateObserver> playerObserverList;

    private void Awake()
    {
        AddToGameObserverList();
    }

    private void OnEnable()
    {
        playerState.OnValueChange += PlayerStateChanged;
    }

    private void OnDisable()
    {
        playerState.OnValueChange -= PlayerStateChanged;
    }

    public void AddListener(IPlayerStateObserver observer)
    {
        if (playerObserverList == null)
            playerObserverList = new List<IPlayerStateObserver>();

        playerObserverList.Add(observer);
    }

    public void SetPlayerRunning()
    {
        playerState.Value = PlayerState.Running;
    }

    public void SetPlayerIdle()
    {
        playerState.Value = PlayerState.Idle;
    }

    public void SetPlayerSlowDown()
    {
        playerState.Value = PlayerState.SlowingDown;
    }

    public void SetPlayerVictory()
    {
        playerState.Value = PlayerState.Win;
    }

    private void PlayerStateChanged()
    {
        currentPlayerState = playerState.Value;
        Log.Info("Player state changed : " + currentPlayerState);
        if (playerObserverList != null)
            for (var i = 0; i < playerObserverList.Count; i++)
                if (playerObserverList[i] != null)
                    playerObserverList[i].OnPlayerStateChanged();


    }


    public void AddToGameObserverList()
    {
        gameManager.AddListener(this);
    }

    public void OnGameStateChanged()
    {
        if (gameState == GameState.Running)
            SetPlayerRunning();
        else if (gameState == GameState.Success)
        {
            SetPlayerVictory();
        }
    }
}