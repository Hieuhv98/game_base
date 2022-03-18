using Gamee_Hiukka.Common;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.GameUI;
using Gamee_Hiukka.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static SkinResources;

namespace Gamee_Hiukka.Control 
{
    public class SkinItem : MonoBehaviour
    {
        [SerializeField] private SkeletonGraphic skePlayer;
        [SerializeField] ButtonBase btnWatchVideo;
        [SerializeField] ButtonBase btnCoin;
        [SerializeField] GameObject coinLock;
        [SerializeField] private TextMeshProUGUI txtCoin;
        [SerializeField] private TextMeshProUGUI txtCoinDisbale;
        [SerializeField] ButtonBase btnDailyReward;
        [SerializeField] ButtonBase btnGiftCode;
        [SerializeField] GameObject active;
 
        private SkinData _skinData;
        public System.Action<SkinItem> actionUpdated;
        public SkinData SkinData => _skinData;

        public void Init(SkinData skinData) 
        {
            _skinData = skinData;
            Util.UpdateSkin(skePlayer, _skinData.SkinName);
        }

        public void UpdateActive()
        {
            active.SetActive(_skinData.ID == GameData.IDSkinCurrent);
        }

        public void UpdateDisplay()
        {
            UpdateActive();
            
            coinLock.gameObject.SetActive(false);
            btnWatchVideo.gameObject.SetActive(false);
            btnCoin.gameObject.SetActive(false);
            btnDailyReward.gameObject.SetActive(false);
            btnGiftCode.gameObject.SetActive(false);
            
            if (!_skinData.IsHas)
            {
                if (_skinData.IsBuyCoin)
                {
                    btnCoin.gameObject.SetActive(true);
                    txtCoin.text = _skinData.Coin.ToString();
                    txtCoinDisbale.text = _skinData.Coin.ToString();
                    
                    if(_skinData.Coin <= GameData.CoinCurrent) coinLock.gameObject.SetActive(false);
                    else coinLock.gameObject.SetActive(true);
                }
                else if(_skinData.IsWatchVideo)
                {
                    btnWatchVideo.gameObject.SetActive(true);
                }else if (_skinData.IsDailyReward)
                {
                    btnDailyReward.gameObject.SetActive(true);
                }else if (_skinData.IsGiftCode)
                {
                    btnGiftCode.gameObject.SetActive(true);
                }

            }
        }

        public void BuyWatchVideo() 
        {
#if UNITY_EDITOR
            BuySkin();
#elif UNITY_ANDROID || UNITY_IOS
            AdsManager.Instance.ShowAdsRewared((isWatched) =>
            {
                if (isWatched)
                {
                    BuySkin();
                }
            });
#endif
        }

        public void ShowDailyReward()
        {
            GamePopup.Instance.ShowPopupDailyReward(()=>actionUpdated?.Invoke(this));      
        }

        public void ShowPopupGiftCode() 
        {
            GamePopup.Instance.ShowPopupGiftCode(() => BuySkin());
        }

        public void BuyCoin()
        {
            GameData.CoinCurrent -= _skinData.Coin;

            if (_skinData.Coin == 20000)
            {
                AdjustLog.AdjustLogEventBuySkin20000();
            }
            else if(_skinData.Coin == 50000)
            {
                AdjustLog.AdjustLogEventBuySkin50000();
            }

            DataController.SaveCoinCurrent();
            BuySkin();

        }
        public void Click() 
        {
            if (_skinData.IsHas && _skinData.ID != GameData.IDSkinCurrent) 
            {
                GameData.IDSkinCurrent = _skinData.ID;
                DataController.SaveIDSkinCurrent();
            }
            actionUpdated?.Invoke(this);
        }

        void BuySkin()
        {
            SkinResources.Instance.SetIsHasSkin(_skinData.ID);
            GameData.IDSkinCurrent = _skinData.ID;
            DataController.SaveIDSkinCurrent();
            UpdateDisplay();
            GamePopup.Instance.ShowPopupNewSkin(_skinData);
            actionUpdated?.Invoke(this);
        }
        private void Hide() { GamePopup.Instance.Hide(); }
    }
}

