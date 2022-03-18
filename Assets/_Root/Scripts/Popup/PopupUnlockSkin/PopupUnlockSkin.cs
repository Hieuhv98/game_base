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

namespace Gamee_Hiukka.UI 
{
    public class PopupUnlockSkin : PopupBase
    {
        [SerializeField] private SkeletonGraphic skePlayer;
//        [SerializeField] private GameObject flare;
        [SerializeField] private UniButton btnNoThank;
        [SerializeField] private UniButton btnGetIt;
        [SerializeField] private UniButton btnContinue;

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

            UpdateDisplay(skinGift);

            AudioManager.Instance.PlayAudioUnlockSkin();
        }

        private void Back() 
        {
            CloseGift();
        }

        private void GetItPressed()
        {
#if UNITY_EDITOR
            ShowGift(skinGift);

#elif UNITY_ANDROID || UNITY_IOS
            AdsManager.Instance.ShowAdsRewared((isWatched) =>
            {
                if(isWatched) ShowGift((skinGift));
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

