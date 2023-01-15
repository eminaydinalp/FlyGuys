using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent : MonoBehaviour, ICollision
{
    [HideInInspector]
    public UnityEvent<Collision, string> Event_CollisionEnter = new UnityEvent<Collision, string>();
    [HideInInspector]
    public UnityEvent<Collision, string> Event_CollisionExit = new UnityEvent<Collision, string>();

    public string myTag { get; set; }

    private void Awake()
    {
        TriggerColliderManager.Instance.SubscribeToCollisions(this);
        myTag = gameObject.tag;
    }


    public void OnCollisionEnter(Collision other)
    {
        Event_CollisionEnter.Invoke(other, myTag);
    }

    public void OnCollisionExit(Collision other)
    {

        Event_CollisionExit.Invoke(other, myTag);
    }

}
