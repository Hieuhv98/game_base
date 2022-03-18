using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Worldreaver.UniUI;
using Gamee_Hiukka.GameUI;
using Gamee_Hiukka.Control;
using Gamee_Hiukka.UI;
using Spine.Unity;

namespace RescueFish.UI
{
    public class PopupNewSkin : PopupBase
    {
        [SerializeField] private SkeletonGraphic skePlayer;
        //[SerializeField] private GameObject flare;
        [SerializeField] private UniButton btnContinue;

        private void Start() 
        {
            /*var flareTransfrom = flare.GetComponent<RectTransform>();
            flareTransfrom
                .DORotate(new Vector3(0, 0, 180), 1f, RotateMode.Fast)
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear);*/
        }
        public void Initialize(SkinData skin)
        {
            AudioManager.Instance.PlayAudioNewSkin();
            btnContinue.onClick.RemoveListener(Back);
            btnContinue.onClick.AddListener(Back);

            UpdateDisplay(skin);
            MyAnalytic.LogEvent(MyAnalytic.SKIN_UNLOCKED, MyAnalytic.SKIN, skin.SkinName);
        }

        private void Back() 
        {
            Close();
        }

        private void UpdateDisplay(SkinData skin) 
        {
            Util.UpdateSkin(skePlayer, skin.SkinName);
        }
    }
}