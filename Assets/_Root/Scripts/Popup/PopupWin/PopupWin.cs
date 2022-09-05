using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Worldreaver.UniUI;
using Gamee_Hiukka.Control;
using UniRx;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.Common;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Gamee_Hiukka.GameUI;
using Spine.Unity;

namespace Gamee_Hiukka.UI 
{
    public class PopupWin : PopupBase
    {
        [SerializeField] Image image;
        [SerializeField] private ButtonBase btnHome;
        [SerializeField] private ButtonBase btnReplay;

        [SerializeField] private ButtonBase btnWatchVideo;
        [SerializeField] private ButtonBase btnNextLevel;
        [SerializeField] private ButtonBase btnCollection;

        [SerializeField] private TextMeshProUGUI txtLevel;
        [SerializeField] private Image imgProcess;
        [SerializeField] private TextMeshProUGUI txtProcess;
        [SerializeField] private TextMeshProUGUI txtCoin;
        [SerializeField] private TextMeshProUGUI txtCoinWatch;
        [SerializeField] private Transform iconCoinDisplay;
        [SerializeField] private int coinWin = 100;
        [SerializeField] private int processTimes = 15;
        [SerializeField] private GameObject normalWinType;
        [SerializeField] private GameObject collectionWinType;
        [SerializeField] private Image imgCollection;
        [SerializeField] private SkeletonGraphic skeBox;
        [SerializeField] private GameObject iconWatch;
        [SerializeField] private GameObject fadeTut;

        private Action _actionClose;
        private Action _actionNextLevel;
        private Action _actionReplayLevel;
        private Action _actionBackToHome;
        private EWinType _type;
        bool isSellect = false;
        int coinWinValue = 0;

        private float _processValueCurent;
        private float _processValueUp => 100 / Config.ProcessCount;
        float _speedLoadProcess = 15;
        private float _timeLoadProcess => _processValueUp / _speedLoadProcess;
        public void Initialize(Action actionClose, Action actionNextLevel, Action actionReplayLevel, Action actionBackToHome, EWinType type = EWinType.WIN_NORMAL)
        {
            _actionClose = actionClose;
            _actionNextLevel = actionNextLevel;
            _actionReplayLevel = actionReplayLevel;
            _actionBackToHome = actionBackToHome;
            _type = type;
            isSellect = false;

            btnHome.gameObject.SetActive(true);
            btnReplay.interactable = true;
            btnNextLevel.interactable = true;
            btnWatchVideo.interactable = true;

            AudioManager.Instance.PlayAudioGameWin();

            txtLevel.text = string.Format(Constant.LEVEL_TEXT, GameData.LevelCurrent - 1);

            UpdateImage();
            ProcessRun();

            btnNextLevel.gameObject.SetActive(false);
            btnWatchVideo.gameObject.SetActive(true);
            
            Util.ShowDelay(btnNextLevel.gameObject, 1.5f, true);

            btnReplay.onClick.RemoveListener(ReplayLevel);
            btnReplay.onClick.AddListener(ReplayLevel);

            btnWatchVideo.onClick.RemoveListener(WatchVideo);
            btnWatchVideo.onClick.AddListener(WatchVideo);

            btnNextLevel.onClick.RemoveListener(NextLevel);
            btnNextLevel.onClick.AddListener(NextLevel);

            btnHome.onClick.RemoveListener(BackToHome);
            btnHome.onClick.AddListener(BackToHome);

            txtCoin.text = string.Format("+{0}", coinWinValue);
            txtCoinWatch.text = string.Format("+{0}", coinWinValue * Config.WatchVideoValue);
            //imgCollection.gameObject.SetActive(false);


            if (_type == EWinType.WIN_NORMAL)
            {
                normalWinType.gameObject.SetActive(true);
                collectionWinType.gameObject.SetActive(false);
            }

            //Gamemanager.Instance.CameraRoom(this.gameObject);

            if (!GameData.IsShowRated) 
            {
                if (GameData.LevelCurrent > Config.LevelShowRate)
                {
                    DOTween.Sequence().SetDelay(0.5f).OnComplete(() => ShowPopupRate());
                }
            }
            else 
            {
                if (!GameData.IsClickRated) 
                {
                    if(GameData.LevelCurrent > Config.LevelShowRateLate && GameData.LevelCurrent % Config.LevelShowRateLateCount == 0) 
                    {
                        DOTween.Sequence().SetDelay(0.5f).OnComplete(() => ShowPopupRate());
                    }
                }
            }
        }

        public void UpdateImage() 
        {
        }
        private void ProcessRun()
        {
            skeBox.AnimationState.SetAnimation(1, "Idle0", true);
            _processValueCurent = GameData.ProcessCount % Config.ProcessCount * _processValueUp;
            if (_processValueCurent == 0)
            {
                skeBox.AnimationState.SetAnimation(1, "Idle", true);
                //btnNextLevel.gameObject.SetActive(false);
                //btnWatchVideo.gameObject.SetActive(false);
                imgProcess.DOFillAmount(1, _timeLoadProcess).OnUpdate(() =>
                {
                    txtProcess.text = string.Format("{0}%", (int)(imgProcess.fillAmount * 100));
                });
                imgProcess.DOFillAmount(1, _timeLoadProcess).OnComplete(() => OnProcessFull());
                txtProcess.text = string.Format("{0}%", 100);
                GameData.IsProcessFull = true;
                DataController.SaveIsProcessFull();
            }
            else
            {
                imgProcess.DOFillAmount(_processValueCurent / 100, _timeLoadProcess).OnUpdate(() =>
                {
                    txtProcess.text = string.Format("{0}%", (int)(imgProcess.fillAmount * 100));
                });
                imgProcess.DOFillAmount(_processValueCurent / 100, _timeLoadProcess).OnComplete(() =>
                {
                    txtProcess.text = string.Format("{0}%", _processValueCurent);
                    GameData.IsProcessFull = false;
                    DataController.SaveIsProcessFull();
                });
            }
        }

        public void BackToHome()
        {
            //Hide();
            _actionBackToHome?.Invoke();
        }

        private void WatchVideo()
        {
#if UNITY_EDITOR
            GameData.IsShowAds = true;
            AddCoin(coinWinValue * Config.WatchVideoValue);
#elif UNITY_ANDROID || UNITY_IOS
            AdsManager.Instance.ShowAdsRewared((isWatched) =>
            {
                if (isWatched)
                {
                    MyAnalytic.LogEvent(MyAnalytic.LEVEL_REWARD_BONUS);
                    GameData.IsShowAds = true;
                    AddCoin(coinWinValue * Config.WatchVideoValue);
                }
            });
#endif
        }

        private void NextLevel()
        {
            if (isSellect) return;
            isSellect = true;
            AddCoin(coinWinValue);
        }

        void AddCoin(int coin) 
        {
            btnHome.gameObject.SetActive(false);
            btnReplay.interactable = false;

            btnNextLevel.gameObject.SetActive(false);
            btnWatchVideo.gameObject.SetActive(false);

            GamePopup.Instance.CoinGeneration.GenerateCoin(null, () => 
            {
                GameData.CoinCurrent += coin * DataParam.coinX2Value;
                DataController.SaveCoinCurrent();

                btnHome.interactable = true;
                btnReplay.interactable = true;

            }, txtCoin.gameObject, iconCoinDisplay.gameObject);
        }

        private void ReplayLevel()
        {
            if (GameData.IsProcessFull) GameData.IsProcessFull = false;
            _actionReplayLevel?.Invoke();
            Hide();
        }

        private void OnProcessFull()
        {
            skeBox.AnimationState.SetAnimation(1, "Open", false);
            var skins = SkinResources.Instance.GetAllSkin();
            List<SkinData> skinsNotHas = new List<SkinData>();
            SkinData skinGift;

            foreach (var skin in skins)
            {
                if (!skin.IsHas && !skin.IsDailyReward && !skin.IsGiftCode) skinsNotHas.Add(skin);
            }

            if (skinsNotHas.Count > 0)
            {
                var skinIndex = UnityEngine.Random.Range(0, skinsNotHas.Count - 1);
                skinGift = skinsNotHas[skinIndex];
                GamePopup.Instance.ShowPopupUnLockSkin(skinGift, UpdateView);
            }
            else GamePopup.Instance.ShowPopupUnLockSkin(null, UpdateView);
        }

        void UpdateView() 
        {
            btnNextLevel.gameObject.SetActive(true);
            btnWatchVideo.gameObject.SetActive(true);
            Gamemanager.Instance.Player.UpdateSkinCurrent();
        }
        private void Hide()
        {
            if (GameData.IsProcessFull)
            {
                txtProcess.text = string.Format("{0}%", 0);
                imgProcess.fillAmount = 0;
            }
            Close();
            _actionClose?.Invoke();
        }

        void ShowPopupDaily()
        {
            GamePopup.Instance.ShowPopupDailyReward(() => Gamemanager.Instance.Player.UpdateSkinCurrent());
        }
        void ShowPopupRate() 
        {
            GamePopup.Instance.ShowPopupRate();
            GameData.IsShowRated = true;
        }
    }
}
