using Gamee_Hiukka.Pattern;
using Gamee_Hiukka.UI;
using System;
using Gamee_Hiukka.Daily;
using RescueFish.UI;
using UnityEngine;
using Worldreaver.UniUI;
using static SkinResources;
using Gamee_Hiukka.Common;
using Gamee_Hiukka.Control;

namespace Gamee_Hiukka.GameUI 
{
    public class GamePopup : Singleton<GamePopup>
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private CoinGeneration coinGeneration;
        private UniPopup _uniPopup;
        public UniPopup Popup => _uniPopup ?? (_uniPopup = new UniPopup());
        public CoinGeneration CoinGeneration => coinGeneration;

        [SerializeField] private PopupWin popupWinPrefab;
        [SerializeField] private PopupLose popupLosePrefab;
        [SerializeField] private PopupSetting popupSettingPrefab;
        [SerializeField] private PopupSkin popupSkinPrefab;
        [SerializeField] private PopupDailyReward popupDailyRewardPrefab;
        [SerializeField] private PopupShop popupShopPrefab;
        [SerializeField] private PopupRate popupRatePrefab;
        [SerializeField] private PopupTutorial popupTutorialPrefab;
        [SerializeField] private PopupNewSkin popupNewSkinPrefab;
        [SerializeField] private PopupUnlockSkin popupUnlockSkinPrefab;
        [SerializeField] private PopupUpdate popupUpdatePrefab;
        [SerializeField] private PopupFacebook popupFaceBookPrefab;
        [SerializeField] private PopupLogin popupLoginPrefab;
        [SerializeField] private PopupRank popupRankPrefab;
        [SerializeField] private PopupGiftCode popupGiftCodePrefab;

        [SerializeField] private PopupDebug popupDebugPrefab;


        public IUniPopupHandler popupWinHandler;
        public IUniPopupHandler popupLoseHandler;
        public IUniPopupHandler popupSettingHandler;
        public IUniPopupHandler popupSkinHandler;
        public IUniPopupHandler popupDailyRewardHandler;
        public IUniPopupHandler popupShopHandler;
        public IUniPopupHandler popupRateHandler;
        public IUniPopupHandler popupTutorialHandler;
        public IUniPopupHandler popupNewSkinHandler;
        public IUniPopupHandler popupUnlockSkinHandler;
        public IUniPopupHandler popupUpdateHandler;
        public IUniPopupHandler popupDebugHandler;
        public IUniPopupHandler popupFaceBookHandler;
        public IUniPopupHandler popupLoginHandler;
        public IUniPopupHandler popupRankHandler;
        public IUniPopupHandler popupGiftCodeHandler;


        public void ShowPopupWin(Action actionClose, Action actionNextLevel, Action actionReplayLevel, Action actionBackToHome, EWinType type = EWinType.WIN_NORMAL)
        {
            if (popupWinHandler != null)
            {
                if (popupWinHandler.ThisGameObject.activeSelf) return;

                Display();
                return;
            }

            popupWinHandler = Instantiate(popupWinPrefab, canvas.transform, false);
            Display();

            void Display()
            {
                // initialize
                var popup = (PopupWin)popupWinHandler;
                Popup.Show(popupWinHandler);
                popup.Initialize(actionClose, actionNextLevel, actionReplayLevel, actionBackToHome, type);
            }
        }

        public void ShowPopupLose(Action actionClose, Action actionReplay, Action actionSkip, Action actinNextLevel, Action actionBackToHome)
        {
            if (popupLoseHandler != null)
            {
                if (popupLoseHandler.ThisGameObject.activeSelf) return;

                Display();
                return;
            }

            popupLoseHandler = Instantiate(popupLosePrefab, canvas.transform, false);
            Display();

            void Display()
            {
                var popup = (PopupLose)popupLoseHandler;
                popup.Initialize(actionClose, actionReplay, actionSkip, actinNextLevel, actionBackToHome);
                Popup.Show(popupLoseHandler);
            }
        }

        public void ShowPopupSetting(Action actionClose)
        {
            if (popupSettingHandler != null)
            {
                if (popupSettingHandler.ThisGameObject.activeSelf) return;

                Display();
                return;
            }

            popupSettingHandler = Instantiate(popupSettingPrefab, canvas.transform, false);
            Display();

            void Display()
            {
                var popup = (PopupSetting)popupSettingHandler;
                popup.Initialize(actionClose);
                Popup.Show(popupSettingHandler);
            }
        }

        public void ShowPopupGiftCode(Action actionClose)
        {
            if (popupGiftCodeHandler != null)
            {
                if (popupGiftCodeHandler.ThisGameObject.activeSelf) return;

                Display();
                return;
            }

            popupGiftCodeHandler = Instantiate(popupGiftCodePrefab, canvas.transform, false);
            Display();

            void Display()
            {
                var popup = (PopupGiftCode)popupGiftCodeHandler;
                popup.Initialize(actionClose);
                Popup.Show(popupGiftCodeHandler);
            }
        }

        public void ShowPopupLogin()
        {
            if (popupLoginHandler != null)
            {
                if (popupLoginHandler.ThisGameObject.activeSelf) return;

                Display();
                return;
            }

            popupLoginHandler = Instantiate(popupLoginPrefab, canvas.transform, false);
            Display();

            void Display()
            {
                var popup = (PopupLogin)popupLoginHandler;
                popup.Initialize();
                Popup.Show(popupLoginHandler);
            }
        }

        public void ShowPopupRank()
        {
            if (popupRankHandler != null)
            {
                if (popupRankHandler.ThisGameObject.activeSelf) return;

                Display();
                return;
            }

            popupRankHandler = Instantiate(popupRankPrefab, canvas.transform, false);
            Display();

            void Display()
            {
                var popup = (PopupRank)popupRankHandler;
                popup.Initialize();
                Popup.Show(popupRankHandler);
            }
        }
        public void ShowPopupDebug()
        {
            if (popupDebugHandler != null)
            {
                if (popupDebugHandler.ThisGameObject.activeSelf) return;

                Display();
                return;
            }

            popupDebugHandler = Instantiate(popupDebugPrefab, canvas.transform, false);
            Display();

            void Display()
            {
                var popup = (PopupDebug)popupDebugHandler;
                popup.Initialize();
                Popup.Show(popupDebugHandler);
            }
        }
        public void ShowPopupRate()
        {
            if (popupRateHandler != null)
            {
                if (popupRateHandler.ThisGameObject.activeSelf) return;

                Display();
                return;
            }

            popupRateHandler = Instantiate(popupRatePrefab, canvas.transform, false);
            Display();

            void Display()
            {
                var popup = (PopupRate)popupRateHandler;
                popup.Initialize();
                Popup.Show(popupRateHandler);
            }
        }

        public void ShowPopupFaceBook()
        {
            if (popupFaceBookHandler != null)
            {
                if (popupFaceBookHandler.ThisGameObject.activeSelf) return;

                Display();
                return;
            }

            popupFaceBookHandler = Instantiate(popupFaceBookPrefab, canvas.transform, false);
            Display();

            void Display()
            {
                var popup = (PopupFacebook)popupFaceBookHandler;
                popup.Initialize();
                Popup.Show(popupFaceBookHandler);
            }
        }

        public void ShowPopupSkin(Action actionClose)
        {
            if (popupSkinHandler != null)
            {
                if (popupSkinHandler.ThisGameObject.activeSelf) return;
                Display();
                return;
            }

            popupSkinHandler = Instantiate(popupSkinPrefab, canvas.transform, false);
            Display();

            void Display()
            {
                var popup = (PopupSkin)popupSkinHandler;
                popup.Initialize(actionClose);
                Popup.Show(popupSkinHandler);
            }
        }
        
        public void ShowPopupDailyReward(Action actionClose)
        {
            if (popupDailyRewardHandler != null)
            {
                if (popupDailyRewardHandler.ThisGameObject.activeSelf) return;
                Display();
                return;
            }

            popupDailyRewardHandler = Instantiate(popupDailyRewardPrefab, canvas.transform, false);
            Display();

            void Display()
            {
                var popup = (PopupDailyReward)popupDailyRewardHandler;
                popup.Initialize(actionClose);
                Popup.Show(popupDailyRewardHandler);
            }
        }
        
        public void ShowPopupShop(Action actionBack)
        {
            if (popupShopHandler != null)
            {
                if (popupShopHandler.ThisGameObject.activeSelf) return;
                Display();
                return;
            }

            popupShopHandler = Instantiate(popupShopPrefab, canvas.transform, false);
            Display();

            void Display()
            {
                var popup = (PopupShop)popupShopHandler;
                popup.Initialize(actionBack);
                Popup.Show(popupShopHandler);
            }
        }

        public void ShowPopupNewUpdate()
        {
            if (popupUpdateHandler != null)
            {
                if (popupUpdateHandler.ThisGameObject.activeSelf) return;
                Display();
                return;
            }

            popupUpdateHandler = Instantiate(popupUpdatePrefab, canvas.transform, false);
            Display();

            void Display()
            {
                var popup = (PopupUpdate)popupUpdateHandler;
                popup.Initialize();
                Popup.Show(popupUpdateHandler);
            }
        }
        public void ShowPopupTutorial(Action actionClose)
        {
            popupTutorialHandler = Instantiate(popupTutorialPrefab, canvas.transform, false);
            Display();

            void Display()
            {
                var popup = (PopupTutorial)popupTutorialHandler;
                popup.Initialize(actionClose);
                Popup.Show(popupTutorialHandler);
            }
        }

        public void ShowPopupNewSkin(SkinData skin)
        {
            popupNewSkinHandler = Instantiate(popupNewSkinPrefab, canvas.transform, false);
            Display();

            void Display()
            {
                var popup = (PopupNewSkin)popupNewSkinHandler;
                popup.Initialize(skin);
                Popup.Show(popupNewSkinHandler);
            }
        }

        public void ShowPopupUnLockSkin(SkinData skinData, Action actionClose)
        {
            popupUnlockSkinHandler = Instantiate(popupUnlockSkinPrefab, canvas.transform, false);
            Display();

            void Display()
            {
                var popup = (PopupUnlockSkin)popupUnlockSkinHandler;
                popup.Initialize(skinData, actionClose);
                Popup.Show(popupUnlockSkinHandler);
            }
        }

        public PopupBase Get<T>() 
        {
            var popups = this.GetComponentsInChildren<PopupBase>();
            foreach(var popup in popups) 
            {
                if (popup is T) return popup;
            }
            return null; 
        }
        public void Hide() { Popup.Hide(); }
        public void HideAll() { Popup.HideAll(); }
    }
}

