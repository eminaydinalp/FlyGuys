#if GAMEANALYTICS_API

using GameAnalyticsSDK;
using Rentire.Core;
using Rentire.Utils;
using UnityEngine;

public class RGameAnalyticsAPI : Singleton<RGameAnalyticsAPI>
{
    // Start is called before the first frame update
    void Start()
    {
        GameAnalytics.Initialize();
        
        GameAnalytics.OnRemoteConfigsUpdatedEvent += RemoteConfigsUpdated;

        InvokeRepeating(nameof(FetchAndSetData), 1f, 5f);
    }

    void FetchAndSetData()
    {
        if(GameAnalytics.IsRemoteConfigsReady())
        {
            float ad_interval = GameAnalytics.GetRemoteConfigsValueAsString("ad_interval", "0.25").ToFloat(0.25f);
            bool remoteAds = GameAnalytics.GetRemoteConfigsValueAsString("remote_ads", "0").ToBool(false);

            UserPrefs.SetAdInterval(ad_interval);
            UserPrefs.SetRemoteAdsOn(remoteAds);

            CancelInvoke(nameof(FetchAndSetData));

            //RAdmobAPI.Instance.Initialize();
        }
    }

    private void RemoteConfigsUpdated()
    {
        // REMOTE ADS CONFIG
        FetchAndSetData();
    }

    /// <summary>
    /// Event isimleri ':' ile ayrılabilir. Level1:Win:100, Level2:Lose gibi
    /// </summary>
    /// <param name="eventName"></param>
    public void LogDesignEvent(string eventName)
    {
        GameAnalytics.NewDesignEvent(eventName);
    }


    public void LogLevelStartEvent(int stageNo, int levelNo)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, stageNo.ToString(), levelNo.ToString());
    }

    public void LogLevelFailedEvent(int stageNo, int levelNo)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, stageNo.ToString(), levelNo.ToString());
    }

    public void LogLevelCompleteEvent(int stageNo, int levelNo)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, stageNo.ToString(), levelNo.ToString());
    }

    private void OnDestroy()
    {
        GameAnalytics.OnRemoteConfigsUpdatedEvent -= RemoteConfigsUpdated;
    }

}
#endif
