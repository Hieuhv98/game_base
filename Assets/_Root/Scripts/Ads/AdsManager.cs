using Game_Base.Control;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.Pattern;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamee_Hiukka.Control
{
    public class AdsManager : Singleton<AdsManager>
    {
        [SerializeField] AdmobManager admobManager;
        [SerializeField] ApplovinManager applovinManager;

        public bool IsLoaded => Config.IsAdmob ? admobManager.IsLoadedReward : applovinManager.IsRewardedReady();
        public bool IsInterLoaded => Config.IsAdmob ? admobManager.IsLoadedInterstitial : applovinManager.IsInterReady();
        public void InitAds() 
        {
            if (Config.IsAdmob) admobManager.Init();
            else applovinManager.InitAds();
        }
        // show
        public void ShowAdsBanner()
        {
            if (DataParam.removeAds) return;
            MyAnalytic.LogEvent(MyAnalytic.AD_CLICK);
            MyAnalytic.LogEvent(MyAnalytic.AD_BANNER_REQUEST);

            if (Config.IsAdmob) 
            {
                if (admobManager == null) return;
                if (admobManager.IsLoadedBanner) 
                {
                    admobManager.ShowBannerAD();
                }
            }
            else 
            {
                if (applovinManager == null) return;
                applovinManager.ShowBanner();
            };
        }
        public void ShowAdsInterstitial(Action actionCompleted = null)
        {
            if (DataParam.removeAds) return;
            MyAnalytic.LogEvent(MyAnalytic.AD_CLICK);
            if (Config.IsAdmob) 
            {
                if (admobManager == null) return;
                if (admobManager.IsLoadedInterstitial) 
                {
                    admobManager.ShowInterstitialAD(actionCompleted);
                }
            }
            else 
            {
                if (applovinManager == null) return;
                applovinManager.ShowIntertitial(actionCompleted);
            }
        }
        public void ShowAdsRewared(Action<bool> actionCloseRewardedAd) 
        {
            MyAnalytic.LogEvent(MyAnalytic.AD_CLICK);
            MyAnalytic.LogEvent(MyAnalytic.AD_REWARD_REQUEST);
            if (Config.IsAdmob)
            {
                if (admobManager == null) return;
                if (admobManager.IsLoadedReward) 
                {
                    admobManager.ShowRewardedAD(actionCloseRewardedAd);
                }
            }
            else
            {
                if (applovinManager == null) return;
                applovinManager.ShowReward(actionCloseRewardedAd);
            };
        }

        // hide
        public void HideAdsBanner() 
        {
            if (Config.IsAdmob)
            {
                if (admobManager == null) return;
                admobManager.HideBannerViewAd();
            }
            else 
            {
                if (applovinManager == null) return;
                applovinManager.HideBanner();
            };
        }
    }
}

