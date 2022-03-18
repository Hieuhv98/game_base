using UnityEngine;
using Worldreaver.UniUI;
using System;
using RescueFish.Controller;
using TMPro;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.Control;

namespace Gamee_Hiukka.UI 
{
    public class PopupRate : PopupBase
    {
        public void Initialize() { }
        public void Rate() 
        {
            AppRatingManager.Instance.OpenApp();
            Close();
        }
    }
}


