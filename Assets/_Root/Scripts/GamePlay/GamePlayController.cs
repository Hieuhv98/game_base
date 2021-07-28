using Game_Base.GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game_Base.Control 
{
    public class GamePlayController : MonoBehaviour
    {
        public void ShowPopupWin() 
        {
            GamePopup.Instance.ShowPopupWin(NextLevel);
        }
        public void ShowPopupLose() 
        {
            GamePopup.Instance.ShowPopupLose(Replay);
        }

        public void NextLevel() { Gamemanager.Instance.GameNextLevel(); }
        public void Replay() { Gamemanager.Instance.GameReplayLevel(); }


        #region ads
        public void ShowAdsBanner() { AdsManager.Instance.ShowAdsBanner(); }
        public void ShowAdsInterstitial() { AdsManager.Instance.ShowAdsInterstitial(); }
        public void HideAdsBanner() { AdsManager.Instance.HideAdsBanner(); }
        #endregion
    }
}

