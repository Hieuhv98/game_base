using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Worldreaver.UniUI;

namespace Game_Base.UI 
{
    public class PopupWin : UniPopupBase
    {
        private Action _actionNextLevel;
        public void Initialize(Action actionNextLevel) 
        {
            _actionNextLevel = actionNextLevel;
        }
    }
}

