using Game_Base.Data;
using Game_Base.Pattern;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game_Base.Control
{
    public class AdsManager : Singleton<AdsManager>
    {
        // show
        public void ShowAdsBanner()
        {
            if (Config.IsAdmob)
            {
                AdmobManager.Instance.ShowBannerAD();
            }
            else { };
        }
        public void ShowAdsInterstitial()
        {
            if (Config.IsAdmob)
            {
                AdmobManager.Instance.ShowInterstitialAD();
            }
            else { }
        }
        public void ShowAdsRewared(Action<bool> actionCloseRewardedAd)
        {
            if (Config.IsAdmob)
            {
                AdmobManager.Instance.ShowRewardedAD(actionCloseRewardedAd);
            }
            else { };
        }

        // hide
        public void HideAdsBanner()
        {
            if (Config.IsAdmob)
            {
                AdmobManager.Instance.HideBannerViewAd();
            }
            else { };
        }
    }
}

