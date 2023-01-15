#if FACEBOOK_API
using System.Collections.Generic;
using Facebook.Unity;
using Rentire.Core;
using UnityEngine;

public class RFacebookAPI : Singleton<RFacebookAPI> {

    // Awake function from Unity's MonoBehavior
    void Start () {
        if (!FB.IsInitialized) {
            // Initialize the Facebook SDK
            FB.Init (InitCallback, OnHideUnity);
        } else {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp ();
        }
    }

    private void InitCallback () {
        if (FB.IsInitialized) {

            FB.ActivateApp ();

        } else {
            Debug.Log ("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity (bool isGameShown) {
        if (!isGameShown) {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        } else {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }

    }

    public void SendEvent (string eventName) {
        if (FB.IsInitialized)
            FB.LogAppEvent (eventName);
    }

    public void SendEvent (string eventName, string parameterName, object parameterValue) {

        var eventToSend = new Dictionary<string, object> ();
        eventToSend.Add(parameterName, parameterValue);
   
        if (FB.IsInitialized)
            FB.LogAppEvent (eventName, null, eventToSend);
    }

}
#endif