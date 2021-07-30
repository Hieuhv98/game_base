using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using Game_Base.Pattern;
using Game_Base.Data;

#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

namespace Game_Base.Control
{
    public class AdmobManager : Singleton<AdmobManager>
    {
        private BannerView bannerViewAd;
        private InterstitialAd interstitialAd;
        private RewardedAd rewardedAd;
        private RewardedInterstitialAd rewardedInterstitialAd;
        private Action<bool> _actionCloseRewardedAd;
        private Action<bool> _actionCloseRewardInterAD;
        public bool IsLoadedReward 
        {
            get
            {
                if (rewardedAd != null) return this.rewardedAd.IsLoaded();

                return false;
            }
        }
        public bool IsLoadedInterstitial
        {
            get
            {
                if (interstitialAd != null) return this.interstitialAd.IsLoaded();

                return false;
            }
        }
        public bool IsLoadedBanner 
        {
            get
            {
                return bannerViewAd != null;
            }
        }

        private bool isWatched = false;
        private bool isHasGift = false;

        public void Init() 
        {
#if UNITY_IOS
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
#endif
            MobileAds.Initialize(initStatus => {
                InitBannerViewAD();
                InitInterstitialAD();
                InitRewarderdAD();
                InitRewardedInterstitialAd();
                DeviceTest();
                //TestMediation(initStatus);

                HideBannerViewAd();
            });
        }

        #region mediation
        private void TestMediation(InitializationStatus initStatus)
        {
            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        // The adapter initialization did not complete.
                        MonoBehaviour.print("Adapter: " + className + " not ready.");
                        break;
                    case AdapterState.Ready:
                        // The adapter was successfully initialized.
                        MonoBehaviour.print("Adapter: " + className + " is initialized.");
                        break;
                }
            }
        }

        public void ShowMediationTestSuite()
        {
            Debug.Log("Mediation: " + "Showded!");
        }

        public void HandleMediationTestSuiteDismissed(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleMediationTestSuiteDismissed event received");
        }
        #endregion


        private void DeviceTest() 
        {
            RequestConfiguration requestConfiguration = new RequestConfiguration
            .Builder()
            .SetTestDeviceIds(Config.IdDeviceTests)
            .build();

            MobileAds.SetRequestConfiguration(requestConfiguration);
        }

        #region init
        private void InitBannerViewAD() 
        {
            this.bannerViewAd = new BannerView(Config.AdmobBannerId, AdSize.Banner, AdPosition.Bottom);
            this.bannerViewAd.OnAdLoaded += HandleOnAdLoaded;
            this.bannerViewAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            this.bannerViewAd.OnAdClosed += HandleBannerADClose;
            Debug.Log(this.bannerViewAd);
            this.bannerViewAd.OnPaidEvent += HandleAdPaidEventBanner;

            AdRequest request = new AdRequest.Builder().Build();
            this.bannerViewAd.LoadAd(request);
        }
        private void InitInterstitialAD() 
        {
            this.interstitialAd = new InterstitialAd(Config.AdmobInterstitialId);
            this.interstitialAd.OnAdLoaded += HandleOnAdLoaded;
            this.interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            this.interstitialAd.OnAdClosed += HandleInterstitialADClose;
            this.interstitialAd.OnPaidEvent += HandleAdPaidEventInter;

            AdRequest request = new AdRequest.Builder().Build();
            this.interstitialAd.LoadAd(request);
        }
        private void InitRewarderdAD() 
        {
            isWatched = false;

            this.rewardedAd = new RewardedAd(Config.AdmobRewardedId);
            
            this.rewardedAd.OnAdLoaded += HandleOnAdLoaded;
            this.rewardedAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            this.rewardedAd.OnAdClosed += HandleRewardedAdClose;
            this.rewardedAd.OnPaidEvent += HandleAdPaidEventReward;

            AdRequest request = new AdRequest.Builder().Build();
            this.rewardedAd.LoadAd(request);
        }

        private void InitRewardedInterstitialAd() 
        {
            isHasGift = false;
            AdRequest request = new AdRequest.Builder().Build();
            RewardedInterstitialAd.LoadAd(Config.AdmobRewardedIntertitialId, request, adLoadCallback);
        }

        private void adLoadCallback(RewardedInterstitialAd ad, AdFailedToLoadEventArgs error)
        {
            if (error == null)
            {
                rewardedInterstitialAd = ad;
                rewardedInterstitialAd.OnAdDidDismissFullScreenContent += HandleAdDidDismiss;
            }
        }

        #endregion

        #region show ad
        public void ShowBannerAD() 
        {
            this.bannerViewAd.Show();
        }

        public void ShowInterstitialAD() 
        {
            this.interstitialAd.Show();
        }

        public void ShowRewardedAD(Action<bool> actionCloseRewardedAd) 
        {
            this._actionCloseRewardedAd = actionCloseRewardedAd;
            this.rewardedAd.Show();
        }

        public void ShowRewardedIntertitialAD(Action<bool> actionCloseRewardInterAD) 
        {
            if (rewardedInterstitialAd != null)
            {
                this._actionCloseRewardInterAD = actionCloseRewardInterAD;
                rewardedInterstitialAd.Show(HandleEarnedRewardIntertitial);
            }
        }

        #endregion

        #region handle
        public void HandleOnAdLoaded(object sender, EventArgs args) 
        {
            Debug.Log("Ads is loaded!");
        }
        public void HandleOnAdFailedToLoad(object sender, EventArgs args) 
        {
            this.bannerViewAd.SetPosition(AdPosition.Bottom);
            Debug.Log("Ads load is failed!");
        }

        public void HandleBannerADClose(object sender, EventArgs args)
        {
            this.InitBannerViewAD();
        }

        public void HandleInterstitialADClose(object sender, EventArgs args) 
        {
            this.InitInterstitialAD();
        }

        public void HandleRewardedAdClose(object sender, EventArgs args)
        {
            this.InitRewarderdAD();
        }
        public void HandleUserEarnedReward(object sender, Reward args)
        {
            isWatched = true;
            _actionCloseRewardedAd?.Invoke(isWatched);

            this.InitRewarderdAD();
        }
        private void HandleEarnedRewardIntertitial(Reward reward)
        {
            isHasGift = true;
            _actionCloseRewardInterAD?.Invoke(isHasGift);
        }

        private void HandleAdDidDismiss(object sender, EventArgs args)
        {
            this.InitRewardedInterstitialAd();
        }

        //  paid_ad_impression hander
        private void HandleAdPaidEventReward(object sender, AdValueEventArgs e)
        {
            var adValue = e.AdValue;
            // Log an event with ad value parameters
            Firebase.Analytics.Parameter[] LTVParameters =
            {
            // Log ad value in micros.
            new Firebase.Analytics.Parameter("valuemicros", adValue.Value),
            // These values below won't be used in ROASrecipe.
            // But log for purposes of debugging and futurereference.
            new Firebase.Analytics.Parameter("currency", adValue.CurrencyCode), new Firebase.Analytics.Parameter("precision", (int) adValue.Precision),
            new Firebase.Analytics.Parameter("adunitid", Config.AdmobRewardedId), new Firebase.Analytics.Parameter("network", "admob")
        };
            Firebase.Analytics.FirebaseAnalytics.LogEvent("paid_ad_impression", LTVParameters);
        }

        private void HandleAdPaidEventInter(object sender, AdValueEventArgs e)
        {
            var adValue = e.AdValue;
            // Log an event with ad value parameters
            Firebase.Analytics.Parameter[] LTVParameters =
            {
            // Log ad value in micros.
            new Firebase.Analytics.Parameter("valuemicros", adValue.Value),
            // These values below won't be used in ROASrecipe.
            // But log for purposes of debugging and futurereference.
            new Firebase.Analytics.Parameter("currency", adValue.CurrencyCode), new Firebase.Analytics.Parameter("precision", (int) adValue.Precision),
            new Firebase.Analytics.Parameter("adunitid", Config.AdmobInterstitialId), new Firebase.Analytics.Parameter("network", "admob")
        };
            Firebase.Analytics.FirebaseAnalytics.LogEvent("paid_ad_impression", LTVParameters);
        }
        private void HandleAdPaidEventBanner(object sender, AdValueEventArgs e)
        {
            var adValue = e.AdValue;
            // Log an event with ad value parameters
            Firebase.Analytics.Parameter[] LTVParameters =
            {
            // Log ad value in micros.
            new Firebase.Analytics.Parameter("valuemicros", adValue.Value),
            // These values below won't be used in ROASrecipe.
            // But log for purposes of debugging and futurereference.
            new Firebase.Analytics.Parameter("currency", adValue.CurrencyCode), new Firebase.Analytics.Parameter("precision", (int) adValue.Precision),
            new Firebase.Analytics.Parameter("adunitid", Config.AdmobBannerId), new Firebase.Analytics.Parameter("network", "admob")
        };
            Firebase.Analytics.FirebaseAnalytics.LogEvent("paid_ad_impression", LTVParameters);
        }
        #endregion

        public void HideBannerViewAd() 
        {
            if(this.bannerViewAd != null) 
            {
                this.bannerViewAd.Hide();
            }
        }
    }
}
