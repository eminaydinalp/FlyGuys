#if FIREBASE_API

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Rentire.Core;
using Rentire.Utils;
using UnityEngine;

public class RFirebaseAPI : Singleton<RFirebaseAPI> {

    public bool FirebaseReady;
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    protected bool isFirebaseInitialized = false;
    // Start is called before the first frame update
    void Start () {
        FirebaseReady = false;
        FirebaseApp.CheckAndFixDependenciesAsync ().ContinueWithOnMainThread (task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) {
                InitializeFirebase ();
            } else {
                Debug.LogError (
                    "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    void InitializeFirebase () {
        FirebaseAnalytics.SetAnalyticsCollectionEnabled (true);

        #region ANALYTICS

        FirebaseAnalytics.LogEvent (FirebaseAnalytics.EventAppOpen);
        FirebaseAnalytics.LogEvent (FirebaseAnalytics.EventLevelStart);
        #endregion

        #region REMOTE CONFIG
        Dictionary<string, object> defaults = new Dictionary<string, object> ();

        // These are the values that are used if we haven't fetched data from the
        // server
        // yet, or if we ask for values that the server doesn't have:
        defaults.Add ("MONEY_IDFA", "");
        defaults.Add ("remote_ads", 1);
        defaults.Add ("test_notification", 0);
        defaults.Add ("ad_image", 1);
        defaults.Add ("ad_interval", 0.5f);
        defaults.Add ("ad_testing", 0);
        defaults.Add ("tap_to_restart_duration", 2f);
        defaults.Add ("applovin", 1);
        defaults.Add ("cam_angle", "{\"posx\":7.65,\"posy\":12.83,\"posz\":-5.53,\"rotx\":71.2,\"roty\":320,\"rotz\":0,\"rotw\":-0.8}");

        Firebase.RemoteConfig.FirebaseRemoteConfig.SetDefaults (defaults);
        Log.Info ("RemoteConfig configured and ready!");
        isFirebaseInitialized = true;
        //Firebase is initialized, we fetch data
        FetchDataAsync ();
        #endregion
    }

    // Start a fetch request.
    public Task FetchDataAsync () {
        Log.Info ("Fetching data...");
        // FetchAsync only fetches new data if the current data is older than the provided
        // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
        // By default the timespan is 12 hours, and for production apps, this is a good
        // number.  For this example though, it's set to a timespan of zero, so that
        // changes in the console will always show up immediately.
        Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.FetchAsync (
            TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread (FetchComplete);
    }

    void FetchComplete (Task fetchTask) {
        if (fetchTask.IsCanceled) {
            Log.Info ("Fetch canceled.");
        } else if (fetchTask.IsFaulted) {
            Log.Info ("Fetch encountered an error.");
        } else if (fetchTask.IsCompleted) {
            Log.Info ("Fetch completed successfully!");
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.Info;
        switch (info.LastFetchStatus) {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched ();
                Log.Info (string.Format ("Remote data loaded and ready (last fetch time {0}).",
                    info.FetchTime));
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason) {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        Log.Info ("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        Log.Info ("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                Log.Info ("Latest Fetch call still pending.");
                break;
        }

        SetFetchedDataOnUserPrefs ();

    }

    void SetFetchedDataOnUserPrefs () {
        var remoteAdStatus = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue ("remote_ads").BooleanValue;
        var imageAdStatus = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue ("ad_image").BooleanValue;
        var adIntervalStatus = float.Parse (Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue ("ad_interval").DoubleValue.ToString ("0.0"));
        var adTestingStatus = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue ("ad_testing").BooleanValue;
        var tapToRestartDurationStatus = float.Parse (Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue ("tap_to_restart_duration").DoubleValue.ToString ("0.0"));
        var applovinStatus = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue ("applovin").BooleanValue;
        var camAngleStatus = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue ("cam_angle").StringValue;
        var moneyForIdfaStatus = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue ("MONEY_IDFA").StringValue;
        var test_notif = Firebase.RemoteConfig.FirebaseRemoteConfig.GetValue ("test_notification").BooleanValue;
        new LogBuilder ("remote_ads : " + remoteAdStatus).SetColor (LogColor.Green).Build ();
        new LogBuilder ("ad_image : " + remoteAdStatus).SetColor (LogColor.Green).Build ();
        new LogBuilder ("ad_interval : " + adIntervalStatus).SetColor (LogColor.Green).Build ();
        new LogBuilder ("ad_testing : " + adTestingStatus).SetColor (LogColor.Green).Build ();
        new LogBuilder ("tap_to_restart_duration : " + tapToRestartDurationStatus).SetColor (LogColor.Green).Build ();
        new LogBuilder ("applovin : " + applovinStatus).SetColor (LogColor.Green).Build ();
        new LogBuilder ("cam_angle : " + camAngleStatus).SetColor (LogColor.Green).Build ();
        new LogBuilder ("test_notif : " + test_notif).SetColor (LogColor.Green).Build ();

        UserPrefs.SetRemoteAdsOn (remoteAdStatus);
        UserPrefs.SetImageAdsOn (imageAdStatus);
        UserPrefs.SetAdInterval (adIntervalStatus);
        UserPrefs.SetAdTesting (adTestingStatus);
        UserPrefs.SetTapToRestartDuration (tapToRestartDurationStatus);
        UserPrefs.SetApplovin (applovinStatus);
        UserPrefs.SetCamAngle (camAngleStatus);
        UserPrefs.SetTestNotif (test_notif);

#if UNITY_IOS
        if (moneyForIdfaStatus.Equals (UnityEngine.iOS.Device.advertisingIdentifier)) {
            UserPrefs.SetTotalCoin (100000);
        }
#endif

        PlayerPrefs.Save ();

        RApplovinMax.Instance.InitializeAppLovin ();
    }

    public void SendEvent (RemoteEvents eventName) {
        FirebaseAnalytics.LogEvent (eventName.ToString ());
    }

    public void SendEvent (RemoteEvents eventName, string parameterName, int parameterValue) {
        if (eventName == RemoteEvents.RewardedWatched && UserPrefs.GetFirstRewardedWatched () == false) {
            UserPrefs.SetFirstRewardedWathced (true);
            FirebaseAnalytics.LogEvent (RemoteEvents.FirstRewarded.ToString (), parameterName, parameterValue);
        }
        FirebaseAnalytics.LogEvent (eventName.ToString (), parameterName, parameterValue);
    }

    public void SendEvent (RemoteEvents eventName, string parameterName, string parameterValue) {
        if (eventName == RemoteEvents.RewardedWatched && UserPrefs.GetFirstRewardedWatched () == false) {
            UserPrefs.SetFirstRewardedWathced (true);
            FirebaseAnalytics.LogEvent (RemoteEvents.FirstRewarded.ToString (), parameterName, parameterValue);
        }
        FirebaseAnalytics.LogEvent (eventName.ToString (), parameterName, parameterValue);
    }

    public void SendEvent (RemoteEvents eventName, params Parameter[] parameters) {
        FirebaseAnalytics.LogEvent (eventName.ToString (), parameters);
    }

}

#endif