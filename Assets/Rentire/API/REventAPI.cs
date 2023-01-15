

using System.Collections.Generic;
#if FIREBASE_API
using Firebase.Analytics;
#endif
public class REventAPI : IEventSender
{
    private bool _isGameAnalytics = false;

    private bool _isAppsFlyer = false;

    private bool _isFacebook = false;

    private bool _isFirebase = false;

    private string _eventName;

    private float? _value = null;
    // Start is called before the first frame update
    
    public IEventSender SetEventOwner(bool isAppsFlyer = false, bool isFirebase = true, bool isGameAnalytics = true, bool isFacebook = true)
    {
        _isAppsFlyer = isAppsFlyer;
        _isFirebase = isFirebase;
        _isFacebook = isFacebook;
        _isGameAnalytics = isGameAnalytics;
        return this;
    }
    
    public IEventSender SetEvent(string eventName)
    {
        _eventName = eventName;
        return this;
    }

    public IEventSender SetEvent(string eventName, float value)
    {
        _eventName = eventName;
        _value = value;
        return this;
    }

    public void SendEvent()
    {
        #if FACEBOOK_API
        if (_isFacebook)
        {
            Facebook.Unity.FB.LogAppEvent(_eventName, _value);
        }
        #endif
        #if FIREBASE_API
        if (_isFirebase)
        {
            FirebaseAnalytics.LogEvent(_eventName);
        }
        #endif
        #if RAppsFlyer_API
        if (_isAppsFlyer)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("","");
            AppsFlyer.trackRichEvent(_eventName, dict);
        }
        #endif
        #if GAMEANALYTICS_API
        if (_isGameAnalytics)
        {
           if(_value.HasValue) 
                GameAnalyticsSDK.GameAnalytics.NewDesignEvent(_eventName, _value.Value);
           else
           {
               GameAnalyticsSDK.GameAnalytics.NewDesignEvent(_eventName);
           }
        }
        #endif
    }

    public void ResetEventApi()
    {
        _isAppsFlyer = false;
        _isFirebase = false;
        _isFacebook = false;
        _isGameAnalytics = false;
        _value = null;
        _eventName = string.Empty;
    }
}
