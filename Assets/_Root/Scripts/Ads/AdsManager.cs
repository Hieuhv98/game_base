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
                if (admobManager.IsLoadedBanner) 
                {
                    admobManager.ShowBannerAD();
                }
            }
            else { applovinManager.ShowBanner(); };
        }
        public void ShowAdsInterstitial()
        {
            if (DataParam.removeAds) return;
            MyAnalytic.LogEvent(MyAnalytic.AD_CLICK);
            if (Config.IsAdmob) 
            {
                if (admobManager.IsLoadedInterstitial) 
                {
                    admobManager.ShowInterstitialAD(null);
                }
            }
            else { applovinManager.ShowIntertitial(null); }
        }
        public void ShowAdsRewared(Action<bool> actionCloseRewardedAd) 
        {
            MyAnalytic.LogEvent(MyAnalytic.AD_CLICK);
            if (Config.IsAdmob)
            {
                if (admobManager.IsLoadedReward) 
                {
                    admobManager.ShowRewardedAD(actionCloseRewardedAd);
                }
            }
            else
            {
                applovinManager.ShowReward(actionCloseRewardedAd);
            };
        }

        // hide
        public void HideAdsBanner() 
        {
            if (Config.IsAdmob)
            {
                admobManager.HideBannerViewAd();
            }
            else { applovinManager.HideBanner(); };
        }
    }
}

