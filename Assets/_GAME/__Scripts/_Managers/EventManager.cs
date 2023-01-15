using System.Collections;
using System.Collections.Generic;
using Rentire.Core;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    #region Events / Delegates
    
    public delegate void ScoreUpdate(int score);
    public event ScoreUpdate Event_ScoreUpdated;
    
    #endregion
    
    #region Methods

    public void Invoke_ScoreUpdate(int score)
    {
        if (Event_ScoreUpdated != null)
            Event_ScoreUpdated(score);
    }

    #endregion

    #region Collection Updater

    public delegate void CollectionUpdated();

    public event CollectionUpdated event_CollectionUpdated;

    public void Invoke_CollectionUpdated()
    {
        event_CollectionUpdated?.Invoke();
    }

    #endregion
}
