using Cysharp.Threading.Tasks;
using Gamee_Hiukka.Common;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.GameUI;
using Gamee_Hiukka.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Worldreaver.UniUI;

namespace Gamee_Hiukka.Control
{
    public class GamePlayController : MonoBehaviour
    {
        [SerializeField] private GamePlayUI gamePlayUI;

        private void Start()
        {
            if (GameData.IsNewGame) GameData.IsNewGame = false;

            //if (GameData.IsNewGame && !Config.IsTest) ShowPopupTutorial();

            Initialize();
        }

        private void Initialize()
        {
            gamePlayUI.Initialize();
            AudioManager.Instance.PlayAudioBackGroundGamePlay();
        }

        public void ShowPopupWin(EWinType type = EWinType.WIN_NORMAL, bool isShowGatcha = false)
        {
            GamePopup.Instance.ShowPopupWin(HidePopup, NextLevel, ReplayLevel, BackToHome, type);
        }
        public void ShowPopupLose()
        {
            GamePopup.Instance.ShowPopupLose(HidePopup, ReplayLevel, SkipLevel, NextLevel, BackToHome);
        }

        public void ShowPopupTutorial()
        {
            GamePopup.Instance.ShowPopupTutorial(HidePopup);
        }

        public void ShowPopupSkin()
        {
            if (Gamemanager.Instance.State != EGameState.GAME_PLAYING) return;
            AdsManager.Instance.HideAdsBanner();
            GamePopup.Instance.ShowPopupSkin(() =>
            {
                AdsManager.Instance.ShowAdsBanner();
                Gamemanager.Instance.Player.UpdateSkinCurrent();
            });
        }

        public void ShowPopupNewSkin()
        {
            //GamePopup.Instance.ShowPopupNewSkin(HidePopup, NextLevel); 
        }

        public void BackToHome()
        {
            if (Gamemanager.Instance.State != EGameState.GAME_ENDING)
            {
                Gamemanager.Instance.GameKill();
                HideAdsBanner();
                var gameMenuScene = SceneManager.LoadSceneAsync(1);
                gameMenuScene.allowSceneActivation = false;

                var popupWin = GamePopup.Instance.Get<PopupWin>();
                var popupLose = GamePopup.Instance.Get<PopupLose>();
                if (popupWin != null) popupWin.Close();
                if (popupLose != null) popupLose.Close();
                Gamemanager.Instance.LevelMapCurrent.gameObject.SetActive(false);
                UniTask.WaitUntil(() => gameMenuScene.isDone);
                gameMenuScene.allowSceneActivation = true;
            }
        }
        public void DefautUI()
        {
            gamePlayUI.Defaut();
        }

        public void UpdateLevelTargetText(ELevelTargetType type)
        {
            gamePlayUI.UpdateLevelTargetText(type);
        }

        public void UpdateBG()
        {
            gamePlayUI.UpdateBG();
        }

        public void UIMove()
        {
            gamePlayUI.UIMove();
        }

        public void UIDefaut()
        {
            gamePlayUI.UIDefaut();
        }

        private void HidePopup()
        {
            //cameraUIView.gameObject.SetActive(false);
            AudioManager.Instance.PauseAuido();
            GamePopup.Instance.Hide();
        }

        public void NextLevel() { Gamemanager.Instance.GameNextLevel(); }
        public void ReplayLevel()
        {
            if (GameData.IsUIMoving) return;
            Gamemanager.Instance.GameReplayLevel();
        }
        public void SkipLevel(Action<bool> actionWatched = null)
        {
            HideAdsBanner();

#if UNITY_EDITOR
            GameData.IsShowAds = true;
            Gamemanager.Instance.GameSkipLevel();
            actionWatched?.Invoke(true);
#elif UNITY_ANDROID || UNITY_IOS
            ShowAdsRewared((isWatched) =>
            {
                actionWatched?.Invoke(isWatched);
                if (isWatched)
                {
                    GameData.IsShowAds = true;
                    Gamemanager.Instance.GameSkipLevel();
                }
            });

#endif
        }
        public void Skip() { SkipLevel(); }

        #region ads
        public void ShowAdsBanner() { AdsManager.Instance.ShowAdsBanner(); }
        public void ShowAdsInterstitial(Action actionCompleted = null) { AdsManager.Instance.ShowAdsInterstitial(actionCompleted); }
        public void ShowAdsRewared(Action<bool> actionCloseRewardedAd) { AdsManager.Instance.ShowAdsRewared(actionCloseRewardedAd); }
        public void HideAdsBanner() { AdsManager.Instance.HideAdsBanner(); }
        #endregion
    }
}

