#if APPLOVIN_API

using System;
using System.Collections;
using System.Collections.Generic;
using Rentire.Core;
using Rentire.Utils;
using UnityEngine;

public class RApplovinMax : Singleton<RApplovinMax> {

    public bool IsInitialized;
    public string SDK_KEY;
    public string interstitialAdUnitId;
    public string rewardedAdUnitId;
    public string bannerAdUnitId; // Retrieve the id from your account

    public bool UnityEditorReturnRewarded = true;
    private float startTime;

    void Start () {
        if (string.IsNullOrEmpty (SDK_KEY)) {
            Log.Warning ("APPLOVIN MAX SDK IS NOT ASSIGNED!");
            return;
        }

        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
            // AppLovin SDK is initialized, start loading ads
            IsInitialized = true;

            if (!UserPrefs.GetNoAds ()) {
                InitializeInterstitialAds ();
                InitializeBannerAds ();
            }
            InitializeRewardedAds ();

        };

    }

    public void InitializeAppLovin () {
        if (UserPrefs.GetApplovin () && Application.internetReachability != NetworkReachability.NotReachable) {
            MaxSdk.SetSdkKey (SDK_KEY);
            MaxSdk.InitializeSdk ();
            startTime = Time.time;
        }
    }

    #region Interstitial
    private bool CanInterstitialBeWatched () {
        if (Time.time < UserPrefs.GetAdInterval () * 60)
            return true;

        if (Time.time - startTime >= UserPrefs.GetAdInterval () * 60)
            return true;

        return false;
    }

    public void InitializeInterstitialAds () {
        // Attach callback
        MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent;

        // Load the first interstitial
        LoadInterstitial ();
    }

    private void LoadInterstitial () {
        MaxSdk.LoadInterstitial (interstitialAdUnitId);
    }

    private void OnInterstitialLoadedEvent (string adUnitId) {
        // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
    }

    private void OnInterstitialFailedEvent (string adUnitId, int errorCode) {
        // Interstitial ad failed to load. We recommend re-trying in 3 seconds.
        Invoke (nameof (LoadInterstitial), 3);
    }

    private void InterstitialFailedToDisplayEvent (string adUnitId, int errorCode) {
        // Interstitial ad failed to display. We recommend loading the next ad
        LoadInterstitial ();
        AudioController.Instance.MuteUnMuteAll (false);
    }

    private void OnInterstitialDismissedEvent (string adUnitId) {
        // Interstitial ad is hidden. Pre-load the next ad
        LoadInterstitial ();
        startTime = Time.time;
        AudioController.Instance.MuteUnMuteAll (false);
    }

    public void ShowInterstitial () {
        if (!IsInitialized || UserPrefs.GetNoAds ())
            return;
        if (MaxSdk.IsInterstitialReady (interstitialAdUnitId) && CanInterstitialBeWatched () && Application.internetReachability != NetworkReachability.NotReachable) {
            MaxSdk.ShowInterstitial (interstitialAdUnitId);
            if (AudioController.Instance) {
                AudioController.Instance.MuteUnMuteAll (true);
            }
        }
    }
    #endregion

    #region Rewarded
    public Action OnRewardedComplete,
    OnRewardedClosed;
    public void InitializeRewardedAds () {
        // Attach callback
        MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first RewardedAd
        LoadRewardedAd ();
    }

    private void LoadRewardedAd () {
        MaxSdk.LoadRewardedAd (rewardedAdUnitId);
    }

    private void OnRewardedAdLoadedEvent (string adUnitId) {
        // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
    }

    private void OnRewardedAdFailedEvent (string adUnitId, int errorCode) {
        // Rewarded ad failed to load. We recommend re-trying in 3 seconds.
        Invoke ("LoadRewardedAd", 3);
    }

    private void OnRewardedAdFailedToDisplayEvent (string adUnitId, int errorCode) {
        // Rewarded ad failed to display. We recommend loading the next ad
        LoadRewardedAd ();
        AudioController.Instance.MuteUnMuteAll (false);
    }

    private void OnRewardedAdDisplayedEvent (string adUnitId) { }

    private void OnRewardedAdClickedEvent (string adUnitId) { }

    private void OnRewardedAdDismissedEvent (string adUnitId) {
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd ();
        if (OnRewardedClosed != null) {
            OnRewardedClosed.Invoke ();
        }

        AudioController.Instance.MuteUnMuteAll (false);
    }

    private void OnRewardedAdReceivedRewardEvent (string adUnitId, MaxSdk.Reward reward) {
        // Rewarded ad was displayed and user should receive the reward
        if (OnRewardedComplete != null) {
            OnRewardedComplete.Invoke ();
        }
    }
    /// <summary>
    /// New rewarded ad is loaded on rewarded ad closed automatically.
    /// </summary>
    /// <param name="RewardedComplete"></param>
    /// <param name="RewardedClosed"></param>
    public void ShowRewardedAd (Action RewardedComplete = null, Action RewardedClosed = null) {
#if UNITY_EDITOR
        if (RewardedComplete != null) {
            RewardedComplete.Invoke ();
            Log.Info ("Rewarded Complete invoked! Unity Editor");
            if (RewardedClosed != null) {
                RewardedClosed.Invoke ();
            }
        }
        return;
#endif

        if (!IsInitialized)
            return;
        if (MaxSdk.IsRewardedAdReady (rewardedAdUnitId) && Application.internetReachability != NetworkReachability.NotReachable) {
            OnRewardedComplete = RewardedComplete;
            OnRewardedClosed = RewardedClosed;

            MaxSdk.ShowRewardedAd (rewardedAdUnitId);

            AudioController.Instance.MuteUnMuteAll (true);
        }
    }

    public bool IsRewardedAdLoaded () {
#if UNITY_EDITOR
        return UnityEditorReturnRewarded;
#endif
        if (!IsInitialized)
            return false;
        return MaxSdk.IsRewardedAdReady (rewardedAdUnitId);
    }

    #endregion

    #region Banner

    public void InitializeBannerAds () {
        // Banners are automatically sized to 320x50 on phones and 728x90 on tablets
        // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments
        MaxSdk.CreateBanner (bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor (bannerAdUnitId, Color.clear);
    }

    public void ShowBanner () {
        if (!IsInitialized || UserPrefs.GetNoAds ())
            return;
        MaxSdk.ShowBanner (bannerAdUnitId);
    }

    public void HideBanner () {
        if (!IsInitialized || UserPrefs.GetNoAds ())
            return;
        MaxSdk.HideBanner (bannerAdUnitId);
    }

    #endregion

    private void OnApplicationQuit () {
        Debug.LogWarning ("RAPPLOVINMAX IS DESTROYED");
        Debug.LogWarning ("APPLICATION QUIT");
    }

    private void OnDestroy () {
        Debug.Log ("RAPPLOVINMAX IS DESTROYED");
        if (IsInitialized) {
            CancelInvoke ();
            UnSubscribe ();
        }
    }

    private void UnSubscribe () {
        CancelInvoke ();
        MaxSdkCallbacks.OnRewardedAdLoadedEvent -= OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.OnRewardedAdLoadFailedEvent -= OnRewardedAdFailedEvent;
        MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent -= OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.OnRewardedAdDisplayedEvent -= OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.OnRewardedAdClickedEvent -= OnRewardedAdClickedEvent;
        MaxSdkCallbacks.OnRewardedAdHiddenEvent -= OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent;

        MaxSdkCallbacks.OnInterstitialLoadedEvent -= OnInterstitialLoadedEvent;
        MaxSdkCallbacks.OnInterstitialLoadFailedEvent -= OnInterstitialFailedEvent;
        MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent -= InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.OnInterstitialHiddenEvent -= OnInterstitialDismissedEvent;
    }

    private void Subscribe () {
        MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent;
    }
}

#endif