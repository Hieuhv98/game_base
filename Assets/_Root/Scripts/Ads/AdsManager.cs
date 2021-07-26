using Game_Base.Pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : Singleton<AdsManager>
{
    // show
    public void ShowAdsBanner() { }
    public void ShowAdsInterstitial() { }
    public void ShowAdsRewared() { }

    // hide
    public void HideAdsBanner() { }
}
