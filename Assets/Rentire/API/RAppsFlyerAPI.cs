
using System;
#if RAppsFlyer_API
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RAppsFlyerAPI : MonoBehaviour
{
    private void Start()
    {
        /* Mandatory - set your AppsFlyer’s Developer key. */
        AppsFlyer.setAppsFlyerKey("ypcS8ggQbnbywLsXrZG9xP");
        /* For detailed logging */
        /* AppsFlyer.setIsDebug (true); */
        #if UNITY_IOS
        /* Mandatory - set your apple app ID
         NOTE: You should enter the number only and not the "ID" prefix */
        AppsFlyer.setAppID ("1486617404");
        AppsFlyer.trackAppLaunch ();
        #elif UNITY_ANDROID
        /* Mandatory - set your Android package name */
        AppsFlyer.setAppID ("com.twodegames.snowball");
        /* For getting the conversion data in Android, you need to add the "AppsFlyerTrackerCallbacks" listener.*/
        AppsFlyer.init ("ypcS8ggQbnbywLsXrZG9xP","AppsFlyerTrackerCallbacks");
        #endif
    }

   
}
#endif
