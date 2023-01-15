#if ADMOB_API
using System;
using GoogleMobileAds.Api;
using Rentire.Core;
using Rentire.Utils;
using UnityEngine;

public class RAdmobAPI : Singleton<RAdmobAPI> {
    public bool IsTesting;

    enum AdType {
        Banner,
        Interstitial,
        Rewarded
    }

    [Header ("IOS")]
    public string IosBannerUnitId;
    public string IosInterstitialUnitId;
    public string IosRewardedUnitId;
    [Header ("ANDROID")]
    public string AndroidBannerUnitId;
    public string AndroidInterstitialUnitId;
    public string AndroidRewardedUnitId;

    private string TestAndroidBannerUnitId = "ca-app-pub-3940256099942544/6300978111";
    private string TestAndroidInterstitialUnitId = "ca-app-pub-3940256099942544/1033173712";
    private string TestAndroidRewardedUnitId = "ca-app-pub-3940256099942544/5224354917";

    private string TestIosBannerUnitId = "ca-app-pub-3940256099942544/2934735716";
    private string TestIosInterstitialUnitId = "ca-app-pub-3940256099942544/4411468910";
    private string TestIosRewardedUnitId = "ca-app-pub-3940256099942544/1712485313";

    private bool isAdsInitialized;

    private float interstitialInterval;
    private float startTime;


    void AdsInitialization (InitializationStatus status) {

        new LogBuilder ("Ads initialized").SetColor (LogColor.Green).Build ();
        isAdsInitialized = true;
        interstitialInterval = UserPrefs.GetAdInterval ();
    }

    public void Initialize()
    {
        new LogBuilder("RAdmobAPI is starting").Bold().SetColor(LogColor.Blue).Build();
        MobileAds.Initialize(AdsInitialization);
        startTime = Time.time;
    }

    #region BANNER
    private BannerView bannerView;

    public void RequestAndDisplayBanner (AdPosition position = AdPosition.Bottom) {
        RequestBanner (position, true);
    }

    public void RequestBanner (AdPosition position = AdPosition.Bottom, bool displayWhenLoaded = false) {
        if (!isAdsInitialized)
            return;
        var adUnitId = GetUnitId (AdType.Banner);
        bannerView = new BannerView (adUnitId, AdSize.Banner, position);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder ().Build ();

        if (displayWhenLoaded)
            bannerView.OnAdLoaded += Banne_Display_WhenLoaded;

        // Load the banner with the request.
        bannerView.LoadAd (request);
    }

    private void Banne_Display_WhenLoaded (object sender, EventArgs e) {
        new LogBuilder ("Banner Display is called").SetColor (LogColor.Green).Build ();
        if (UserPrefs.GetRemoteAdsOn ()) {
            new LogBuilder ("Banner Display is shown").SetColor (LogColor.Green).Build ();
            bannerView.Show ();
        } else
            bannerView.Hide ();
    }

    public void Banner_Display () {
        new LogBuilder ("Banner Display is called").SetColor (LogColor.Green).Build ();
        if (!isAdsInitialized)
            return;
        if (UserPrefs.GetRemoteAdsOn ())
            bannerView.Show ();
        else
            bannerView.Hide ();
    }
    public void Banner_Destroy () {
        //If banner is not destroyed memory leaks can occur
        if (bannerView != null)
            bannerView.Destroy ();
    }
    #endregion

    #region INTERSTITIAL
    private InterstitialAd interstitial;
    
    public void RequestInterstitial (EventHandler<EventArgs> HandleOnAdClosed = null) {
        if (!isAdsInitialized)
            return;
        try {
            if (interstitial != null)
                Interstitial_Destroy ();

            var adUnitId = GetUnitId (AdType.Interstitial);
            // Initialize an InterstitialAd.
            interstitial = new InterstitialAd (adUnitId);

            // Called when the ad is closed.
            this.interstitial.OnAdClosed += HandleOnAdClosed;
            interstitial.OnAdClosed += InterstitialWatched;

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder ().Build ();
            // Load the interstitial with the request.
            this.interstitial.LoadAd (request);
        } catch {
            Log.Error ("Can not request interstitial");
        }
    }

    private void InterstitialWatched (object sender, EventArgs e) {
        startTime = Time.time;
        RequestInterstitial ();

    }


    private bool CanInterstitialBeWatched () {
        if (Time.time < UserPrefs.GetAdInterval () * 60)
            return true;

        if (Time.time - startTime >= UserPrefs.GetAdInterval () * 60)
            return true;

        return false;
    }

    public void Interstitial_Display () {
        if (interstitial == null)
            return;
        if (interstitial.IsLoaded () && UserPrefs.GetRemoteAdsOn () && CanInterstitialBeWatched ()) {
            interstitial.Show ();
        }
    }

    public void Interstitial_Destroy () {
        interstitial?.Destroy ();
    }


    #endregion

    #region REWARDED
    private RewardedAd rewardedAd;

    public void CreateAndLoadRewardedAd () {
        if (!isAdsInitialized)
            return;
        try {
            var adUnitId = GetUnitId (AdType.Rewarded);
            rewardedAd = new RewardedAd (adUnitId);

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder ().Build ();
            // Load the rewarded ad with the request.
            rewardedAd.LoadAd (request);
            new LogBuilder ("Rewarded ad loaded").SetColor (LogColor.Green).Build ();
        } catch {
            Log.Error ("Rewarded can not be created");
        }
    }

    public void Rewarded_Display (EventHandler<Reward> handleUserEarnedReward = null, EventHandler<EventArgs> handleRewardedAdClosed = null) {
#if UNITY_EDITOR
        if (IsTesting) {
            handleUserEarnedReward.Invoke ("sender", new Reward ());
            return;
        }
#endif

        if (rewardedAd == null)
            return;

        new LogBuilder ("Rewarded ad display is called").SetColor (LogColor.Green).Build ();
        if (handleUserEarnedReward != null)
            rewardedAd.OnUserEarnedReward += handleUserEarnedReward;
        if (handleRewardedAdClosed != null)
            rewardedAd.OnAdClosed += handleRewardedAdClosed;

        if (rewardedAd.IsLoaded ()) {
            rewardedAd.Show ();
            new LogBuilder ("Rewarded ad display is shown").SetColor (LogColor.Green).Build ();
        }
    }

    public bool Rewarded_IsLoaded () {
#if UNITY_EDITOR
        return true;
#endif
        if (IsTesting)
            return true;

        if (Application.internetReachability == NetworkReachability.NotReachable)
            return false;

        return rewardedAd != null && rewardedAd.IsLoaded ();
    }

    #endregion

    string GetUnitId (AdType adType) {

#if UNITY_EDITOR
        switch (adType) {
            case AdType.Banner:
                return TestIosBannerUnitId;
            case AdType.Interstitial:
                return TestIosInterstitialUnitId;
            case AdType.Rewarded:
                return TestIosRewardedUnitId;
            default:
                return null;
        }
#endif

#if UNITY_IOS
        if (UserPrefs.GetAdTesting ()) {
            switch (adType) {
                case AdType.Banner:
                    return TestIosBannerUnitId;
                case AdType.Interstitial:
                    return TestIosInterstitialUnitId;
                case AdType.Rewarded:
                    return TestIosRewardedUnitId;
                default:
                    return null;
            }
        }
        switch (adType) {
            case AdType.Banner:
                return IosBannerUnitId;
            case AdType.Interstitial:
                return IosInterstitialUnitId;
            case AdType.Rewarded:
                return IosRewardedUnitId;
            default:
                return null;
        }

#elif UNITY_ANDROID
        if (UserPrefs.GetAdTesting ()) {
            switch (adType) {
                case AdType.Banner:
                    return TestAndroidBannerUnitId;
                case AdType.Interstitial:
                    return TestAndroidInterstitialUnitId;
                case AdType.Rewarded:
                    return TestAndroidRewardedUnitId;
                default:
                    return null;
            }
        }
        switch (adType) {
            case AdType.Banner:
                return AndroidBannerUnitId;
            case AdType.Interstitial:
                return AndroidInterstitialUnitId;
            case AdType.Rewarded:
                return AndroidRewardedUnitId;
            default:
                return null;
        }
#endif
    }
}
#endif