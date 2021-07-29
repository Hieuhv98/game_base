using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Worldreaver.UniUI;

namespace Game_Base.UI 
{
    public class PopupWin : UniPopupBase
    {
        [SerializeField] private UniButton btnNextLevel;
        [SerializeField] private UniButton btnReplay;

        private Action _actionClose;
        private Action _actionNextLevel;
        private Action _actionReplayLevel;
        public void Initialize(Action actionClose, Action actionNextLevel) 
        {
            _actionClose = actionClose;
            _actionNextLevel = actionNextLevel;
            btnNextLevel.onClick.RemoveListener(NextLevel);
            btnNextLevel.onClick.AddListener(NextLevel);
        }

        private void NextLevel() 
        {
            _actionNextLevel?.Invoke();
            Hide();
        }

        private void Hide()
        {
            _actionClose?.Invoke();
        }
    }
}

