using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : RMonoBehaviour, ITrigger
{
    [HideInInspector]
    public UnityEvent<Collider, string> Event_TriggerEnter = new UnityEvent<Collider, string>();
    [HideInInspector]
    public UnityEvent<Collider, string> Event_TriggerExit = new UnityEvent<Collider, string>();

    public string myTag { get; set; }

    private void Awake()
    {
        myTag = gameObject.tag;
        TriggerColliderManager.Instance.SubscribeToTriggers(this);
    }

    public void OnTriggerEnter(Collider other)
    {
        Event_TriggerEnter.Invoke(other, myTag);
    }

    public void OnTriggerExit(Collider other)
    {
        Event_TriggerExit.Invoke(other, myTag);
    }
}
