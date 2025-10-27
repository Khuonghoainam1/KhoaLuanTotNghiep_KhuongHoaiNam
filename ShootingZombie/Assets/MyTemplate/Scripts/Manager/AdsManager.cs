///https://dash.applovin.com/documentation/mediation/unity/getting-started/rewarded-ads
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Thanh.Core;
//using GoogleMobileAds.Api;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

namespace MyGame
{
    public class AdsManager : Singleton<AdsManager>
    {

        //        public string appKey = "";
        //        public string appKeyIOS = "";

        //        private float interCooldown = 30f;
        //        private int interFromLevel = 3;
        //        [HideInInspector]
        //        public bool isInterShowing = false;

        //        [HideInInspector]
        //        public bool isRewardShowing = false;
        //        [HideInInspector]
        //        public float lastTimeShowAds;

        //        private bool resumeMusic;
        //        private bool resumeSound;

        //        public static bool CheatAds = false;

        //        string adLocation = "";

        //#if UNITY_IOS
        //        private ATTrackingStatusBinding.AuthorizationTrackingStatus m_PreviousStatus;
        //        private bool m_Once;
        //#endif

        //        public void Init()
        //        {

        //#if UNITY_IOS
        //            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
        //            Debug.LogFormat("Tracking status at start: {0}", status);
        //            m_PreviousStatus = status;

        //            SkAdNetworkBinding.SkAdNetworkUpdateConversionValue(0);
        //            SkAdNetworkBinding.SkAdNetworkRegisterAppForNetworkAttribution();
        //            // Set the flag as true 
        //            AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(true);
        //#endif

        //            AppOpenAdManager.Instance.Init();

        //            IronSource.Agent.setConsent(User.Instance[ItemID.IronSource_Consent] == 1);

        //            IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
        //            IronSourceEvents.onImpressionDataReadyEvent += ImpressionSuccessEvent;

        //            // SDK init
        //            Debug.Log("[IronSource] Init");
        //#if UNITY_IOS
        //            IronSource.Agent.init(appKeyIOS, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.BANNER);
        //#else
        //            IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.BANNER);
        //#endif            
        //            IronSource.Agent.shouldTrackNetworkState(true);
        //            IronSourceConfig.Instance.setClientSideCallbacks(true);
        //            IronSource.Agent.setAdaptersDebug(false);

        //            SceneManager.activeSceneChanged += OnActiveSceneChange;
        //        }

        //        void SdkInitializationCompletedEvent()
        //        {
        //            Debug.Log("[IronSource] SdkInitializationCompletedEvent");

        //            IronSource.Agent.validateIntegration();

        //#if ShowBanner
        //            InitializeBannerAds();
        //#endif
        //            InitializeInterstitialAds();
        //            InitializeRewardedAds();
        //        }

        //        private IEnumerator Start()
        //        {
        //            while (FirebaseManager.Instance.remote.FetchDone == false)
        //            {
        //                yield return null;
        //            }

        //            interCooldown = FirebaseManager.Instance.remote.GetTimeShowInter();
        //            interFromLevel = FirebaseManager.Instance.remote.GetInterFromLevel();
        //        }

        //        private void OnActiveSceneChange(Scene arg0, Scene arg1)
        //        {
        //            OnRewardSuccess = null;
        //            OnRewardFail = null;
        //            rewardedLoc = AdLocation.None;
        //        }

        //        #region interstitial
        //        Action callbackInterSuccess;
        //        Action callbackInterFail;
        //        public bool TryToShowInterstitial(string _adLocation, Action callbackSuccess, Action callbackFail)
        //        {
        //            adLocation = "inter_" + _adLocation;

        //#if UNITY_EDITOR || !ShowBanner
        //            callbackFail?.Invoke();
        //            return false;
        //#endif

        //            if (!IsSpamAds()
        //                || User.Instance[ItemID.PlayingLevel] < interFromLevel
        //                || CheatAds)
        //            {
        //                callbackFail?.Invoke();
        //                return false;
        //            }

        //            isInterShowing = false;

        //            callbackInterSuccess = null;
        //            callbackInterFail = null;

        //            if (Time.time - lastTimeShowAds >= interCooldown)
        //            {
        //                if (IronSource.Agent.isInterstitialReady())
        //                {
        //                    callbackInterSuccess = callbackSuccess;
        //                    callbackInterFail = callbackFail;
        //                    isInterShowing = true;
        //                    IronSource.Agent.showInterstitial(adLocation);
        //                    return true;
        //                }
        //            }

        //            callbackFail?.Invoke();
        //            return false;
        //        }

        //        public void InitializeInterstitialAds()
        //        {
        //            if (!IsSpamAds())
        //            {
        //                return;
        //            }

        //            //Add AdInfo Interstitial Events
        //            IronSourceInterstitialEvents.onAdLoadFailedEvent += InterOnAdLoadFailedEvent;
        //            IronSourceInterstitialEvents.onAdShowFailedEvent += InterOnAdShowFailedEvent;
        //            IronSourceInterstitialEvents.onAdOpenedEvent += InterOnAdOpenedEvent;
        //            IronSourceInterstitialEvents.onAdClosedEvent += InterOnAdClosedEvent;

        //            LoadInterstitial(0);
        //        }

        //        private void LoadInterstitial(float delay)
        //        {
        //            StartCoroutine(IE_LoadInter(delay));
        //        }

        //        IEnumerator IE_LoadInter(float delay)
        //        {
        //            if (delay > 0)
        //            {
        //                yield return WaitForSecondsCache.Get(delay);
        //            }

        //            IronSource.Agent.loadInterstitial();

        //            yield break;
        //        }

        //        void InterOnAdLoadFailedEvent(IronSourceError ironSourceError)
        //        {
        //            Debug.LogError("[Inter] OnAdLoadFailedEvent " + ironSourceError.ToString());

        //            LoadInterstitial(3f);
        //            callbackInterFail?.Invoke();
        //            callbackInterFail = null;
        //            callbackInterSuccess = null;
        //        }

        //        void InterOnAdOpenedEvent(IronSourceAdInfo adInfo)
        //        {
        //            Debug.Log("[Inter] OnAdOpenedEvent " + adInfo.ToString());

        //            PauseGame();

        //            isInterShowing = true;
        //            lastTimeShowAds = Time.time;

        //            FirebaseManager.Instance.Interstitial_show(adLocation);
        //        }

        //        void InterOnAdShowFailedEvent(IronSourceError ironSourceError, IronSourceAdInfo adInfo)
        //        {
        //            Debug.LogError("[Inter] OnAdShowFailedEvent " + ironSourceError.ToString() + " And AdInfo " + adInfo.ToString());

        //            LoadInterstitial(0);
        //            callbackInterFail?.Invoke();
        //            callbackInterFail = null;
        //            callbackInterSuccess = null;
        //            isInterShowing = false;
        //            lastTimeShowAds = Time.time;
        //        }

        //        void InterOnAdClosedEvent(IronSourceAdInfo adInfo)
        //        {
        //            Debug.Log("[Inter] OnAdClosedEvent " + adInfo.ToString());

        //            LoadInterstitial(0);

        //            callbackInterSuccess?.Invoke();
        //            callbackInterSuccess = null;
        //            callbackInterFail = null;

        //            lastTimeShowAds = Time.time;
        //            isInterShowing = false;

        //            ResumeGame();
        //        }
        //        #endregion

        //        #region video
        //        private Action OnRewardSuccess;
        //        private Action OnRewardFail;
        //        private AdLocation rewardedLoc;

        //        public void InitializeRewardedAds()
        //        {
        //            //Add AdInfo Rewarded Video Events
        //            IronSourceRewardedVideoEvents.onAdLoadFailedEvent += VideoOnAdLoadFailedEvent;
        //            IronSourceRewardedVideoEvents.onAdShowFailedEvent += VideoOnAdShowFailedEvent;
        //            IronSourceRewardedVideoEvents.onAdOpenedEvent += VideoOnAdOpenedEvent;
        //            IronSourceRewardedVideoEvents.onAdClosedEvent += VideoOnAdClosedEvent;
        //            IronSourceRewardedVideoEvents.onAdRewardedEvent += VideoOnAdRewardedEvent;
        //            IronSourceRewardedVideoEvents.onAdClickedEvent += VideoOnAdClickedEvent;

        //            // Load the first rewarded ad
        //            LoadRewardedAd(0);
        //        }

        //        bool isRewardSuccess = false;
        //        public void ShowVideoReward(AdLocation loc, Action successAction, Action failAction)
        //        {
        //#if UNITY_EDITOR || !ShowBanner
        //            successAction?.Invoke();
        //            successAction = null;
        //            return;
        //#endif

        //            if (CheatAds)
        //            {
        //                successAction?.Invoke();
        //                successAction = null;
        //                return;
        //            }

        //            isRewardSuccess = false;
        //            isRewardShowing = false;

        //            OnRewardSuccess = successAction;
        //            OnRewardFail = failAction;
        //            rewardedLoc = loc;

        //            if (IronSource.Agent.isRewardedVideoAvailable())
        //            {
        //                isRewardShowing = true;
        //                IronSource.Agent.showRewardedVideo("video_" + rewardedLoc.ToString());
        //            }
        //            else
        //            {
        //                failAction?.Invoke();
        //            }
        //        }

        //        private void LoadRewardedAd(float delay)
        //        {
        //            //StartCoroutine(IE_LoadRewardedAd(delay));
        //        }

        //        IEnumerator IE_LoadRewardedAd(float delay)
        //        {
        //            if (delay > 0)
        //            {
        //                yield return WaitForSecondsCache.Get(delay);
        //            }

        //            IronSource.Agent.loadRewardedVideo();

        //            yield break;
        //        }

        //        void VideoOnAdOpenedEvent(IronSourceAdInfo adInfo)
        //        {
        //            Debug.Log("[Video] OnAdOpenedEvent " + adInfo.ToString());

        //            lastTimeShowAds = Time.time;
        //            isRewardShowing = true;

        //            PauseGame();
        //        }

        //        void VideoOnAdClosedEvent(IronSourceAdInfo adInfo)
        //        {
        //            Debug.Log("[Video] OnAdClosedEvent " + adInfo.ToString());

        //            LoadRewardedAd(0);
        //            lastTimeShowAds = Time.time;
        //            isRewardShowing = false;
        //            if (isRewardSuccess == false)
        //            {
        //                OnRewardFail?.Invoke();
        //            }
        //            ResumeGame();
        //        }

        //        void VideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo info)
        //        {
        //            Debug.LogError("[Video] OnAdShowFailedEvent " + error.ToString());

        //            LoadRewardedAd(0);
        //            OnRewardFail?.Invoke();
        //        }

        //        void VideoOnAdLoadFailedEvent(IronSourceError ironSourceError)
        //        {
        //            Debug.LogError("[Video] OnAdLoadFailedEvent " + ironSourceError.ToString());

        //            LoadRewardedAd(3);
        //        }

        //        void VideoOnAdRewardedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
        //        {
        //            Debug.Log("[Video] OnAdRewardedEvent With Placement" + ironSourcePlacement.ToString() + "And AdInfo " + adInfo.ToString());

        //            FirebaseManager.Instance.rewarded_video_show(rewardedLoc.ToString());
        //            isRewardSuccess = true;
        //            AudioAssistant.PlaySound("Rewarded");
        //            OnRewardSuccess?.Invoke();
        //            OnRewardSuccess = null;
        //            rewardedLoc = AdLocation.None;
        //            lastTimeShowAds = Time.time;
        //            isRewardShowing = false;
        //        }

        //        void VideoOnAdClickedEvent(IronSourcePlacement ironSourcePlacement, IronSourceAdInfo adInfo)
        //        {
        //            Debug.Log("[Video] ReardedVideoOnAdClickedEvent With Placement" + ironSourcePlacement.ToString() + "And AdInfo " + adInfo.ToString());
        //            lastTimeShowAds = Time.time;
        //        }
        //        #endregion

        //        #region banner
        //#if ShowBanner
        //        private void InitializeBannerAds()
        //        {
        //            if (!IsSpamAds())
        //            {
        //                return;
        //            }

        //            //Add AdInfo Banner Events
        //            IronSourceBannerEvents.onAdLoadedEvent += BannerOnAdLoadedEvent;
        //            IronSourceBannerEvents.onAdLoadFailedEvent += BannerOnAdLoadFailedEvent;

        //            LoadBanner(0);
        //        }

        //        private void LoadBanner(float delay)
        //        {
        //            StartCoroutine(IE_LoadBanner(delay));
        //        }

        //        IEnumerator IE_LoadBanner(float delay)
        //        {
        //            if (delay > 0)
        //            {
        //                yield return WaitForSecondsCache.Get(delay);
        //            }

        //            var size = IronSourceBannerSize.BANNER;
        //            size.SetAdaptive(true);
        //            IronSource.Agent.loadBanner(size, IronSourceBannerPosition.BOTTOM);
        //        }


        //        public void ShowBanner()
        //        {
        //            if (!IsSpamAds())
        //            {
        //                return;
        //            }

        //            IronSource.Agent.displayBanner();
        //        }

        //        public void HideBanner()
        //        {
        //            IronSource.Agent.hideBanner();
        //        }

        //        void BannerOnAdLoadedEvent(IronSourceAdInfo adInfo)
        //        {
        //            Debug.Log("[Banner] BannerOnAdLoadedEvent " + adInfo.ToString());
        //            ShowBanner();
        //        }

        //        void BannerOnAdLoadFailedEvent(IronSourceError ironSourceError)
        //        {
        //            Debug.Log("[Banner] BannerOnAdLoadFailedEvent " + ironSourceError.ToString());

        //            LoadBanner(3f);
        //        }
        //#endif
        //        #endregion

        //        #region AOA
        //        public void OnApplicationPause(bool paused)
        //        {
        //            Debug.Log("unity-script: OnApplicationPause = " + paused);
        //            IronSource.Agent.onApplicationPause(paused);

        //            // Display the app open ad when the app is foregrounded
        //            double secondFromLastAds = Time.time - lastTimeShowAds;
        //            double secondFromLastIap = Time.time - InAppManager.Instance.lastTimeShowIAP;

        //            if (!paused && !InAppManager.Instance.showingIAP && !isInterShowing && !isRewardShowing && secondFromLastAds > 3 && secondFromLastIap > 3)
        //            {
        //                Debug.Log("[AOA] secondFromLastAds " + secondFromLastAds + " secondFromLastIap " + secondFromLastIap);
        //                Debug.Log("isInterShowing " + isInterShowing + " isRewardShowing " + isRewardShowing);

        //                AppOpenAdManager.Instance.ShowAdIfAvailable();
        //            }
        //        }
        //        #endregion

        //        void ImpressionSuccessEvent(IronSourceImpressionData impressionData)
        //        {
        //            Debug.Log("unity - script: I got ImpressionSuccessEvent ToString(): " + impressionData.ToString());
        //            Debug.Log("unity - script: I got ImpressionSuccessEvent allData: " + impressionData.allData);

        //            if (impressionData != null && !string.IsNullOrEmpty(impressionData.adNetwork))
        //            {
        //                SendEventRealtime(impressionData);
        //            }
        //        }

        //        private static void SendEventRealtime(IronSourceImpressionData data)
        //        {
        //            Firebase.Analytics.Parameter[] AdParameters = {
        //                new Firebase.Analytics.Parameter("ad_platform", "iron_source"),
        //                new Firebase.Analytics.Parameter("ad_source", data.adNetwork),
        //                new Firebase.Analytics.Parameter("ad_unit_name",data.adUnit),
        //                new Firebase.Analytics.Parameter("currency","USD"),
        //                new Firebase.Analytics.Parameter("value",data.revenue.Value),
        //                new Firebase.Analytics.Parameter("placement",data.placement),
        //                new Firebase.Analytics.Parameter("country_code",data.country),
        //                new Firebase.Analytics.Parameter("ad_format",data.instanceName),
        //            };

        //            Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_Impression_ironsource", AdParameters);
        //        }

        //        public static bool IsSpamAds()
        //        {
        //            return User.Instance[ItemID.RemoveAds] <= 0
        //                && User.Instance[ItemID.IAP_Count] <= 0;
        //        }

        //        bool isPause = false;
        //        public void PauseGame()
        //        {
        //            if (isPause)
        //            {
        //                return;
        //            }

        //            isPause = true;

        //            resumeMusic = User.Instance.EnableMusic;
        //            resumeSound = User.Instance.EnableSound;

        //            Time.timeScale = 0f;
        //            User.Instance.EnableMusic = false;
        //            User.Instance.EnableSound = false;
        //        }

        //        public void ResumeGame()
        //        {
        //            if (isPause == false)
        //            {
        //                return;
        //            }

        //            isPause = false;

        //            Time.timeScale = PopupManager.Instance.GetOpeningPopups().Where(x => x.isPause).Count() > 0 ? 0f : 1;
        //            User.Instance.EnableMusic = resumeMusic;
        //            User.Instance.EnableSound = resumeSound;
        //        }

        //        public void RequestAuthorizationTracking()
        //        {
        //#if UNITY_IOS
        //            if (!m_Once)
        //            {
        //                m_Once = true;
        //                ATTrackingStatusBinding.RequestAuthorizationTracking();
        //            }

        //            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
        //            if (m_PreviousStatus != status)
        //            {
        //                m_PreviousStatus = status;
        //                Debug.LogFormat("Tracking status updated: {0}", status);
        //            }
        //#endif
        //        }

          }
    }

    public enum AdLocation
    {
        None,
        PopupNotice,
        DailyReward,
        EndGame,
        Spin,
        Upgrade,
        SelectMode,
        Revice,
        GetSkin,
    }