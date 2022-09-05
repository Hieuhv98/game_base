using Gamee_Hiukka.Data;
using System.Collections;
using System.Collections.Generic;
using Gamee_Hiukka.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Worldreaver.UniUI;
using System;

public class PopupFacebook : PopupBase
{
    string path = "https://www.facebook.com/groups/grabpacktroll.gameestudio/";
    Action actionClose;
    public void Initialize(Action actionClose)
    {
        this.actionClose = actionClose;
    }

    public void OpenFacebook()
    {
        GameData.IsLoginFB = true;
        Application.OpenURL(path);
        actionClose?.Invoke();
        Close();
    }

}
