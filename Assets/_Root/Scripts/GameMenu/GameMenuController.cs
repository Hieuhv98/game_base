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
using UnityEngine.UI;

namespace Gamee_Hiukka.Control
{
    public class GameMenuController : MonoBehaviour
    {
        [SerializeField] SkeletonGraphic skeleton;
        [SerializeField] private ButtonBase btnStartGame;
        [SerializeField] private ButtonBase btnSetting;
        [SerializeField] private ButtonBase btnShop;
        [SerializeField] private ButtonBase btnDaily;
        [SerializeField] private ButtonBase btnRemoveAds;
        [SerializeField] private ButtonBase btnDebug;
        [SerializeField] private TextMeshProUGUI txtLevel;
        [SerializeField] private GameObject dailyNoti;
        [SerializeField] private GameObject skinNoti;
        [SerializeField] private GameObject fbNoti;
        [SerializeField] private GameObject rankNoti;
        [SerializeField] private CoinDisplay coinDisplay;
        [SerializeField] private List<ObjMoveUI> objMoves;

        ObjMoveUI coinObjMove = null;

        public void Start()
        {
            Init();
        }

        public void Init()
        {
            AudioManager.Instance.PlayAudioBackGroundGameMenu();

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

            dailyNoti.SetActive(GameData.HasReward);
            skinNoti.SetActive(SkinResources.Instance.CanBuySkin);
            fbNoti.SetActive(!GameData.IsLoginFB);
            rankNoti.SetActive(GameData.UserName == "");

            UIDefaut();
            UpdateSkin();

            if (!Config.IsAutoShowDailyReward) CheckNewUpdate();
            else
            {
                if (GameData.HasReward)
                {
                    DOTween.Sequence().SetDelay(.35f).OnComplete(() =>
                    {
                        GamePopup.Instance.ShowPopupDailyReward(() =>
                        {
                            UpdateSkin();
                            CheckNewUpdate();
                        });
                    });
                }
                else CheckNewUpdate();
            }

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
            skinNoti.SetActive(SkinResources.Instance.CanBuySkin);
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

                if (!GameData.IsDontShowUpdate && GameData.LevelCurrent > Config.LevelShowUpdate)
                {
                    GamePopup.Instance.ShowPopupNewUpdate();
                }
            }
        }

        public void ShowPopupRate()
        {
            //GamePopup.Instance.ShowPopupRate();
        }
        private void ShowPopupSetting()
        {
            /*            isShowSetting = !isShowSetting;
                        if (isShowSetting) settingAnimator.Play("ShowSetting");
                        else settingAnimator.Play("HideSetting");*/
            GamePopup.Instance.ShowPopupSetting(null);
        }
        public void ShowPopupRank()
        {
            if (GameData.UserName.Equals(""))
            {
                GamePopup.Instance.ShowPopupLogin(() => { rankNoti.SetActive(GameData.UserName == ""); });
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
            GamePopup.Instance.ShowPopupFaceBook(() => fbNoti.SetActive(!GameData.IsLoginFB));
        }

        public void ShowPopupDebug()
        {
            GamePopup.Instance.ShowPopupDebug();
        }

        private void StartGame()
        {
            GamePopup.Instance.HideAll();
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
            dailyNoti.SetActive(GameData.HasReward);
            skinNoti.SetActive(SkinResources.Instance.CanBuySkin);
            Util.UpdateSkinCurrent(skeleton);
            UpdateDisplay();
        }

        public void UIMove()
        {
            //btnStartGame.interactable = false;
            foreach (var obj in objMoves)
            {
                obj.Move();
            }
        }

        public void UIMoveBack()
        {
            //btnStartGame.interactable = true;
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

