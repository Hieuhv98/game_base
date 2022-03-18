using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Worldreaver.UniUI;
using DG.Tweening;
using UnityEngine.UI;

namespace Gamee_Hiukka.UI 
{
    [RequireComponent(typeof(CanvasGroup))]
    public class PopupBase : UniPopupBase
    {
        private CanvasGroup _canvasGroup;
        [SerializeField] Transform content;
        Vector3 scaleContentDefaut = Vector3.one;
        public void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (content == null) return;
            scaleContentDefaut = content.localScale;
        }

        public override void Show() 
        {
            base.Show();

            _canvasGroup.alpha = 0;
            _canvasGroup.DOFade(1f, 0.15f);

            if (content == null) return;
            content.localScale = scaleContentDefaut * 0.95f;
            content.DOScale(scaleContentDefaut * 1.05f , 0.25f).SetEase(Ease.InBack).OnComplete(() => 
            {
                content.DOScale(scaleContentDefaut, 0.1f);
            });

        }
        public override void Close() 
        {
            /*_canvasGroup.DOFade(0, 0.1f).OnComplete(() =>
            {
                content.DOScale(Vector3.one * 0.8f, 0.05f).OnComplete(() =>
                {
                    base.Close();
                });
            });*/

            base.Close();

        }
    }
}

