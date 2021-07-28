using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Worldreaver.UniUI;

namespace Game_Base.UI 
{
    public class PopupLose : UniPopupBase
    {
        private Action _actionReplay;
        public void Initialize(Action actionReplay) 
        {
            _actionReplay = actionReplay;
        }
    }
}

