using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Worldreaver.UniUI;
using System;

namespace Gamee_Hiukka.UI 
{
    public class PopupTutorial : PopupBase
    {
        [SerializeField] ScrollSnapRect scrollSnap;
        [SerializeField] private ButtonBase btnNextStep;
        [SerializeField] private ButtonBase btnBackStep;
        [SerializeField] private ButtonBase btnDone;

        public void Initialize(Action actionClose) { }
    }
}

