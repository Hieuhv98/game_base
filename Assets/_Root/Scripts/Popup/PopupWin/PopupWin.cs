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

namespace Gamee_Hiukka.UI 
{
    public class PopupWin : PopupBase
    {
        [SerializeField] private ButtonBase btnHome;
        [SerializeField] private ButtonBase btnReplay;

        [SerializeField] private ButtonBase btnWatchVideo;
        [SerializeField] private ButtonBase btnNextLevel;

        [SerializeField] private TextMeshProUGUI txtLevel;
        [SerializeField] private Image imgProcess;
        [SerializeField] private TextMeshProUGUI txtProcess;
        [SerializeField] private TextMeshProUGUI txtCoin;
        [SerializeField] private TextMeshProUGUI txtCoinWatch;
        [SerializeField] private Transform iconCoinDisplay;
        [SerializeField] private int coinWinValue = 100;
        [SerializeField] private int processTimes = 15;

        private Action _actionClose;
        private Action _actionNextLevel;
        private Action _actionReplayLevel;
        private Action _actionBackToHome;
        private EWinType _type;

        private float _processValueCurent;
        private float _processValueUp => 100 / processTimes;
        float _speedLoadProcess = 6;
        private float _timeLoadProcess => _processValueUp / _speedLoadProcess;

        bool isSellected = false;
        public void Initialize(Action actionClose, Action actionNextLevel, Action actionReplayLevel, Action actionBackToHome, EWinType type = EWinType.WIN_NORMAL)
        {
            _actionClose = actionClose;
            _actionNextLevel = actionNextLevel;
            _actionReplayLevel = actionReplayLevel;
            _actionBackToHome = actionBackToHome;
            _type = type;
            isSellected = false;

            AudioManager.Instance.PlayAudioGameWin();

            txtLevel.text = string.Format(Constant.LEVEL_TEXT, GameData.LevelCurrent - 1);

            ProcessRun();

            btnHome.interactable = true;
            btnReplay.interactable = true;

            btnNextLevel.gameObject.SetActive(true);
            btnWatchVideo.gameObject.SetActive(true);

            btnReplay.onClick.RemoveListener(ReplayLevel);
            btnReplay.onClick.AddListener(ReplayLevel);

            btnWatchVideo.onClick.RemoveListener(WatchVideo);
            btnWatchVideo.onClick.AddListener(WatchVideo);

            btnNextLevel.onClick.RemoveListener(NextLevel);
            btnNextLevel.onClick.AddListener(NextLevel);

            btnHome.onClick.RemoveListener(BackToHome);
            btnHome.onClick.AddListener(BackToHome);

            txtCoin.text = string.Format("+ {0}", coinWinValue);
            txtCoinWatch.text = string.Format("Get {0}", coinWinValue * Config.WatchVideoValue);

            Gamemanager.Instance.CameraRoom(this.gameObject);

            if (!GameData.IsShowRated && GameData.LevelCurrent > 11)
            {
                DOTween.Sequence().SetDelay(0.5f).OnComplete(() => ShowPopupRate());
            }
        }

        private void ProcessRun()
        {
            _processValueCurent = (GameData.LevelCurrent - 1) % processTimes * _processValueUp;
            if (_processValueCurent == 0)
            {
                btnNextLevel.interactable = false;
                btnWatchVideo.interactable = false;

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
            AddCoin(coinWinValue * Config.WatchVideoValue);
#elif UNITY_ANDROID || UNITY_IOS
            AdsManager.Instance.ShowAdsRewared((isWatched) =>
            {
                AddCoin(coinWinValue * Config.WatchVideoValue);
            });
#endif
        }

        private void NextLevel()
        {
            AddCoin(coinWinValue);
        }

        void AddCoin(int coin) 
        {
            if (isSellected) return;
            isSellected = true;

            btnHome.interactable = false;
            btnReplay.interactable = false;

            btnNextLevel.gameObject.SetActive(false);
            btnWatchVideo.gameObject.SetActive(false);

            GamePopup.Instance.CoinGeneration.GenerateCoin(null, () => 
            {
                GameData.CoinCurrent += coin * DataParam.coinX2Value;
                DataController.SaveCoinCurrent();

                btnHome.interactable = true;
                btnReplay.interactable = true;
                DOTween.Sequence().SetDelay(.5f).OnComplete(() =>
                {
                    _actionNextLevel?.Invoke();
                    Hide();
                });
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
                GamePopup.Instance.ShowPopupUnLockSkin(skinGift, UpdatePlayerSkin);
            }
            else GamePopup.Instance.ShowPopupUnLockSkin(null, UpdatePlayerSkin);
        }

        void UpdatePlayerSkin() 
        {
            Gamemanager.Instance.Player.UpdateSkinCurrent();
        }
        private void Hide()
        {
            Close();
            _actionClose?.Invoke();
        }

        void ShowPopupRate() 
        {
            GamePopup.Instance.ShowPopupRate();
            GameData.IsShowRated = true;
        }
    }
}
