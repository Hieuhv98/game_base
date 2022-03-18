using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Worldreaver.UniUI;
using UniRx;
using DG.Tweening;
using Gamee_Hiukka.Control;
using Gamee_Hiukka.Data;
using TMPro;

namespace  Gamee_Hiukka.UI
{
       public class PopupShop : PopupBase
    {
        [SerializeField] private UniButton btnClose;
        [SerializeField] private UniButton btnFreeCoin;
        [SerializeField] private UniButton btnBuyCoinPack1;
        [SerializeField] private UniButton btnBuyCoinPack2;
        [SerializeField] private UniButton btnRemoveAds;
        [SerializeField] private UniButton btnUnlockAllSkin;
        [SerializeField] private UniButton btnBuyCoinX2;
        [SerializeField] private UniButton btnBuyCombo;
        [SerializeField] private TextMeshProUGUI textFreCoin;

        private Action _actionBack;
        public void Initialize(Action actionBack) 
        {
            this._actionBack = actionBack;
            btnClose.onClick.RemoveListener(Back);
            btnClose.onClick.AddListener(Back);

            btnFreeCoin.onClick.RemoveListener(BuyFreeCoin);
            btnFreeCoin.onClick.AddListener(BuyFreeCoin);

            btnBuyCoinPack1.onClick.RemoveListener(BuyCoinPack1);
            btnBuyCoinPack1.onClick.AddListener(BuyCoinPack1);

            btnBuyCoinPack2.onClick.RemoveListener(BuyCoinPack2);
            btnBuyCoinPack2.onClick.AddListener(BuyCoinPack2);

            if (DataParam.removeAds)
            {
                btnRemoveAds.enabled = false;
                btnRemoveAds.gameObject.SetActive(false);
            }else
            {
                btnRemoveAds.gameObject.SetActive(true);
                btnRemoveAds.onClick.RemoveListener(BuyRemoveAds);
                btnRemoveAds.onClick.AddListener(BuyRemoveAds);
            }

            if (DataParam.unlockAllSkin)
            {
                btnUnlockAllSkin.enabled = false;
                btnUnlockAllSkin.gameObject.SetActive(false);
            }
            else
            {
                btnUnlockAllSkin.gameObject.SetActive(true);
                btnUnlockAllSkin.onClick.RemoveListener(BuyUnlockAllSkin);
                btnUnlockAllSkin.onClick.AddListener(BuyUnlockAllSkin);
            }

            if (DataParam.coinX2)
            {
                btnBuyCoinX2.enabled = false;
                btnBuyCoinX2.gameObject.SetActive(false);
            }
            else
            {
                btnBuyCoinX2.gameObject.SetActive(true);
                btnBuyCoinX2.onClick.RemoveListener(BuyCoinX2);
                btnBuyCoinX2.onClick.AddListener(BuyCoinX2);
            }

            if (DataParam.coinX2 && DataParam.removeAds && DataParam.unlockAllSkin)
            {
                btnBuyCombo.enabled = false;
                btnBuyCombo.gameObject.SetActive(false);
            }
            else
            {
                btnBuyCombo.gameObject.SetActive(true);
                btnBuyCombo.onClick.RemoveListener(BuyCombo);
                btnBuyCombo.onClick.AddListener(BuyCombo);
            }

            textFreCoin.text = string.Format("+{0}", DataParam.COIN_FREE);
        }

        private void Back() 
        {
            _actionBack?.Invoke();
        }

        private void BuyFreeCoin() 
        {
#if UNITY_EDITOR
            AddScore(DataParam.COIN_FREE, btnFreeCoin.transform);

#elif UNITY_ANDROID || UNITY_IOS

            AdsManager.Instance.ShowAdsRewared((isWatched) =>
            {
                if(isWatched) AddScore(DataParam.COIN_FREE, btnFreeCoin.transform);
            });
#endif
        }
        private void BuyCoinPack1() 
        {
            IAPManager.Instance.BuyAddCoinPack1(() => 
            {
                GameData.CoinCurrent += DataParam.COIN_PACK_1 * DataParam.coinX2Value;
                DataController.SaveCoinCurrent();
                //AudioManager.Instance.PlayAudioCoinMove();
            });
        }
        private void BuyCoinPack2() 
        {
            IAPManager.Instance.BuyAddCoinPack2(() =>
            {
                GameData.CoinCurrent += DataParam.COIN_PACK_2 * DataParam.coinX2Value;
                DataController.SaveCoinCurrent();
                //AudioManager.Instance.PlayAudioCoinMove();
            });
        }
        private void BuyRemoveAds() 
        {
            IAPManager.Instance.BuyRemoveAds(() =>
            {
                btnRemoveAds.enabled = false;
                btnRemoveAds.gameObject.SetActive(false);
                DataParam.removeAds = true;
                DataController.SaveRemoveAds();
            });

        }
        private void BuyUnlockAllSkin() 
        {
            IAPManager.Instance.BuyUnlockAllSkin(() =>
            {
                btnUnlockAllSkin.enabled = false;
                btnUnlockAllSkin.gameObject.SetActive(false);
                DataParam.unlockAllSkin = true;
                DataController.SaveUnlockAllSkin();
                SkinResources.Instance.UnlockAllSkin();
            });
        }
        private void BuyCoinX2() 
        {
            IAPManager.Instance.BuyCoinX2(() =>
            {
                btnBuyCoinX2.enabled = false;
                btnBuyCoinX2.gameObject.SetActive(false);
                DataParam.coinX2 = true;
                DataParam.coinX2Value = 2;
                DataController.SaveCoinX2();
            });
        }
        private void BuyCombo() 
        {
            IAPManager.Instance.BuyCombo(() =>
            {
                btnBuyCombo.gameObject.SetActive(false);
                btnRemoveAds.gameObject.SetActive(false);
                btnUnlockAllSkin.gameObject.SetActive(false);
                btnBuyCoinX2.gameObject.SetActive(false);

                DataParam.removeAds = true;
                DataParam.unlockAllSkin = true;
                DataParam.coinX2 = true;
                DataParam.coinX2Value = 2;

                DataController.SaveRemoveAds();
                DataController.SaveUnlockAllSkin();
                SkinResources.Instance.UnlockAllSkin();
                DataController.SaveCoinX2();
            });
        }

        private void AddScore(int score, Transform tran)
        {
            GameData.CoinCurrent += score * DataParam.coinX2Value;
            DataController.SaveCoinCurrent();
            //AudioManager.Instance.PlayAudioCoinMove();
        }
    }
}
