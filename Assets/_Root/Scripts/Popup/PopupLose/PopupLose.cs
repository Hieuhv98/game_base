using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Worldreaver.UniUI;

namespace Game_Base.UI 
{
    public class PopupLose : UniPopupBase
    {
        [SerializeField] private UniButton btnReplay;
        private Action _actionReplay;
        private Action _actionClose;
        public void Initialize(Action actionClose, Action actionReplay) 
        {
            _actionReplay = actionReplay;
            _actionClose = actionClose;

            btnReplay.onClick.RemoveListener(ReplayLevel);
            btnReplay.onClick.AddListener(ReplayLevel);
        }

        private void ReplayLevel() 
        {
            _actionReplay?.Invoke();
            Hide();
        }

        private void Hide() 
        {
            _actionClose?.Invoke();
        }
    }
}

