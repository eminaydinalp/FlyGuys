using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameStateObserver
{
    void AddToGameObserverList();
    void OnGameStateChanged();
}
