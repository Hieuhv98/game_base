using Game_Base.GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Worldreaver.UniUI;

namespace Game_Base.Control 
{
    public class GamePlayController : MonoBehaviour
    {
        [SerializeField] private UniButton btnReplay;

        private void Start()
        {
            Init();
        }

        private void Init() 
        {
            btnReplay.onClick.RemoveListener(ReplayLevel);
            btnReplay.onClick.AddListener(ReplayLevel);
        }

        public void ShowPopupWin() 
        {
            GamePopup.Instance.ShowPopupWin(NextLevel);
        }
        public void ShowPopupLose() 
        {
            GamePopup.Instance.ShowPopupLose(ReplayLevel);
        }

        public void NextLevel() { Gamemanager.Instance.GameNextLevel(); }
        public void ReplayLevel() { Gamemanager.Instance.GameReplayLevel(); }


        #region ads
        public void ShowAdsBanner() { AdsManager.Instance.ShowAdsBanner(); }
        public void ShowAdsInterstitial() { AdsManager.Instance.ShowAdsInterstitial(); }
        public void HideAdsBanner() { AdsManager.Instance.HideAdsBanner(); }
        #endregion
    }
}

