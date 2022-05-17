using Gamee_Hiukka.Common;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.GameUI;
using Gamee_Hiukka.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Worldreaver.UniUI;
using Spine;
using Spine.Unity;
using DG.Tweening;
using Game_Base.Control;
using Gamee_Hiukka.Other;
using static Gamee_Hiukka.Control.Gamemanager;

namespace Gamee_Hiukka.Control 
{
    public class GameMenuController : MonoBehaviour
    {
        [SerializeField] private SkeletonGraphic skePlayer;
        [SerializeField] private ButtonBase btnStartGame;
        [SerializeField] private ButtonBase btnSetting;
        [SerializeField] private ButtonBase btnShop;
        [SerializeField] private ButtonBase btnDaily;
        [SerializeField] private ButtonBase btnRemoveAds;
        [SerializeField] private ButtonBase btnDebug;
        [SerializeField] private TextMeshProUGUI txtLevel;
        [SerializeField] private GameObject dailyNoti;
        [SerializeField] private CoinDisplay coinDisplay;
        [SerializeField] private List<ObjMoveUI> objMoves;
        [SerializeField] private ButtonBase btnNextRoom;
        [SerializeField] private GameObject fxUnlockNewRoom;

        bool isLoadLevelCompleted = false;
        ObjMoveUI coinObjMove = null;

        public void Start() 
        {
            Init();
        }

        public void Init()
        {
            AudioManager.Instance.PlayAudioBackGroundGameMenu();
            Util.UpdateSkinCurrent(skePlayer);
            isLoadLevelCompleted = false;

            //LoadLevelMap();

            txtLevel.text = string.Format(Constant.LEVEL_TEXT, GameData.LevelCurrent);
            btnStartGame.onClick.RemoveListener(StartGame);
            btnStartGame.onClick.AddListener(StartGame);

            btnSetting.onClick.RemoveListener(ShowPopupSetting);
            btnSetting.onClick.AddListener(ShowPopupSetting);

            btnShop.onClick.RemoveListener(ShowPopupShop);
            btnShop.onClick.AddListener(ShowPopupShop);
            
            btnDaily.onClick.RemoveListener(ShowPopupDailyReward);
            btnDaily.onClick.AddListener(ShowPopupDailyReward);

            UpdateDisplay();
/*            if (!GameData.IsShowRated && GameData.LevelCurrent > 11)
            {
                ShowPopupRate();
                GameData.IsShowRated = true;
            }*/

            dailyNoti.SetActive(GameData.HasReward);

            if(GameData.IsNewGame && Config.AutoStartGame) 
            {
                StartGame();
            }
            else
            {
                //UIDefaut();
            }

            CheckNewUpdate();

            AdsManager.Instance.HideAdsBanner();
            
            coinDisplay.ActionClose -= UpdateDisplay;
            coinDisplay.ActionClose += UpdateDisplay;

            coinDisplay.ActionUpdate -= OnCoinChange;
            coinDisplay.ActionUpdate += OnCoinChange;

            coinObjMove = coinDisplay.GetComponent<ObjMoveUI>();
        }

        void UpdateDisplay()
        {
            btnRemoveAds.gameObject.SetActive(!DataParam.removeAds);
        }

        void OnCoinChange() 
        {
        }

        void CheckNewUpdate() 
        {
            if (Util.ConvertVersion(Application.version) < Util.ConvertVersion(GameData.VersionApp))
            {
                DataController.LoadStatusUpdate();
                DataController.LoadVersionDontUpdate();

                if (Util.ConvertVersion(GameData.VersionApp) > GameData.VersionDontUpdate)
                {
                    GameData.IsDontShowUpdate = false;
                    DataController.SaveStatusUpdate();
                }

                if (!GameData.IsDontShowUpdate && GameData.LevelCurrent != 1)
                {
                    GamePopup.Instance.ShowPopupNewUpdate();
                }
            }
        }

        public void ShowPopupRate() 
        {
            GamePopup.Instance.ShowPopupRate();
        }
        private void ShowPopupSetting() 
        {
            GamePopup.Instance.ShowPopupSetting(HidePopup);
        }
        public void ShowPopupRank()
        {
            if (GameData.UserName.Equals(""))
            {
                GamePopup.Instance.ShowPopupLogin();
            }
            else
            {
                GamePopup.Instance.ShowPopupRank();
            }
        }

        public void BuyRemoveAds()
        {
            IAPManager.Instance.BuyRemoveAds(() =>
            {
                btnRemoveAds.enabled = false;
                btnRemoveAds.gameObject.SetActive(false);
                DataParam.removeAds = true;
                DataController.SaveRemoveAds();
            });

        }

        private void ShowPopupShop()
        {
            GamePopup.Instance.ShowPopupSkin(UpdateSkin);
        }
        public void ShowPopupGiftCode()
        {
            GamePopup.Instance.ShowPopupGiftCode(UpdateSkin);
        }
        private void ShowPopupDailyReward()
        {
            GamePopup.Instance.ShowPopupDailyReward(UpdateSkin);
        }

        public void ShowPopupFB() 
        {
            GamePopup.Instance.ShowPopupFaceBook();
        }

        public void ShowPopupDebug() 
        {
            GamePopup.Instance.ShowPopupDebug();
        }

        private void StartGame()
        {
            coinDisplay.ActionClose -= UpdateDisplay;
            coinDisplay.ActionUpdate -= OnCoinChange;
            SceneManager.LoadScene(2);
        }

        private void HidePopup() 
        {
            GamePopup.Instance.Hide();
        }

        private void UpdateSkin() 
        {
            Util.UpdateSkinCurrent(skePlayer);
            dailyNoti.SetActive(GameData.HasReward);
            UpdateDisplay();
        }
         
        public void UIMove()
        {
            btnStartGame.interactable = false;
            foreach (var obj in objMoves)
            {
                obj.Move();
            }
        }

        public void UIMoveBack()
        {
            btnStartGame.interactable = true;

            foreach (var obj in objMoves)
            {
                obj.MoveBack();
            }
        }

        public void UIDefaut()
        {
            foreach (var obj in objMoves)
            {
                obj.Defaut();
            }
        }
    }
}

