using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Worldreaver.UniUI;
using Gamee_Hiukka.Control;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.GameUI;
using Spine.Unity;
using UniRx;
using static SkinResources;
using TMPro;

namespace Gamee_Hiukka.UI 
{
    public class PopupUnlockSkin : PopupBase
    {
        [SerializeField] private GameObject model;
        [SerializeField] private GameObject title;
        [SerializeField] private SkeletonGraphic skePlayer;
//        [SerializeField] private GameObject flare;
        [SerializeField] private UniButton btnNoThank;
        [SerializeField] private UniButton btnGetIt;
        [SerializeField] private UniButton btnContinue;
        [SerializeField] private GameObject coinGroup;
        [SerializeField] private TextMeshProUGUI txtCoin;
        [SerializeField] private int coinGift;
        [SerializeField] private GameObject iconCoinDisplay;
        [SerializeField] private GameObject coinDisplay;

        private SkinData skinGift;
        private Action _actionClose;

        private void Start() 
        {
/*            var flareTransfrom = flare.GetComponent<RectTransform>();
            flareTransfrom
                .DORotate(new Vector3(0, 0, 180), 1f, RotateMode.Fast)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear);*/
        }
        public void Initialize(SkinData skinGift, Action actionClose)
        {
            this.skinGift = skinGift;
            this._actionClose = actionClose;

            btnNoThank.gameObject.SetActive(true);
            btnGetIt.gameObject.SetActive(true);
            btnContinue.gameObject.SetActive(true);

            btnNoThank.onClick.RemoveListener(Back);
            btnNoThank.onClick.AddListener(Back);

            btnGetIt.onClick.RemoveListener(GetItPressed);
            btnGetIt.onClick.AddListener(GetItPressed);

            btnContinue.onClick.RemoveListener(Back);
            btnContinue.onClick.AddListener(Back);

            if (skinGift != null)
            {
                coinDisplay.SetActive(false);
                UpdateDisplay(skinGift);
            }
            else coinDisplay.SetActive(true);

            AudioManager.Instance.PlayAudioUnlockSkin();
            OnBoxUnlock();
        }

        public void OnBoxUnlock()
        {
            if (skinGift != null)
            {
                model.gameObject.SetActive(true);
            }
            else
            {
                coinGroup.gameObject.SetActive(true);
                txtCoin.text = "+" + coinGift.ToString();
            }

            btnNoThank.gameObject.SetActive(true);
            btnGetIt.gameObject.SetActive(true);
            title.gameObject.SetActive(true);
        }
        private void Back() 
        {
            CloseGift();
        }

        private void GetItPressed()
        {
#if UNITY_EDITOR
            if (skinGift != null) ShowGift(skinGift);
            else AddScore(coinGift);
#elif UNITY_ANDROID || UNITY_IOS
            AdsManager.Instance.ShowAdsRewared((isWatched) =>
            {
                if (skinGift != null) ShowGift(skinGift);
                else AddScore(coinGift);
            });
#endif
        }

        private void ShowGift(SkinData skin)
        {
            MyAnalytic.LogEvent(MyAnalytic.PROCESS_CLAIM_SKIN);
            SkinResources.Instance.SetIsHasSkin(skin.ID);
            GameData.IDSkinCurrent = skin.ID;
            DataController.SaveIDSkinCurrent();

            CloseGift();
            GamePopup.Instance.ShowPopupNewSkin(skin);
        }
        private void AddScore(int score)
        {
            GamePopup.Instance.CoinGeneration.GenerateCoin(null, () =>
            {
                GameData.CoinCurrent += score * DataParam.coinX2Value;
                DataController.SaveCoinCurrent();
                CloseGift();
            }, txtCoin.gameObject, iconCoinDisplay.gameObject);
        }

        private void CloseGift()
        {
            _actionClose?.Invoke();
            Close();
        }

        private void UpdateDisplay(SkinData skinGift) 
        {
            Util.UpdateSkin(skePlayer, skinGift.SkinName);
        }
    }
}

