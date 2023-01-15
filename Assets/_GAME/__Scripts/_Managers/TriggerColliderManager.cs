using System.Collections;
using System.Collections.Generic;
using Rentire.Core;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class TriggerColliderManager : Singleton<TriggerColliderManager>
{
    void PlayerTriggerEntered(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            gameManager.SetGameFinal();
        }
    }

    void CoinTriggerEntered(Collider other)
    {

    }

    #region Trigger / Collision Definers

    void TriggerEnter(Collider other, string tag)
    {
        if (tag.Equals(Tags.PLAYER))
        {
            PlayerTriggerEntered(other);
        }
        if (tag.Equals(Tags.COIN))
        {
            CoinTriggerEntered(other);
        }
    }

    void TriggerExit(Collider other, string tag)
    {

    }

    void CollisionEnter(Collision collision, string tag)
    {

    }
    void CollisionExit(Collision collision, string tag)
    {

    }

    #endregion

    #region Subscription

    public void SubscribeToTriggers(TriggerEvent trigger)
    {
        trigger.Event_TriggerEnter.AddListener(TriggerEnter);
        trigger.Event_TriggerExit.AddListener(TriggerExit);
    }

    public void SubscribeToCollisions(CollisionEvent colEvent)
    {
        colEvent.Event_CollisionEnter.AddListener(CollisionEnter);
        colEvent.Event_CollisionExit.AddListener(CollisionExit);
    }

    #endregion
}
