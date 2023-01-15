using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventSender
{
    IEventSender SetEventOwner(bool isAppsFlyer, bool isFirebase, bool isGameAnalytics, bool isFacebook);
    IEventSender SetEvent(string eventName);
    IEventSender SetEvent(string eventName, float value);
    void SendEvent();
}
