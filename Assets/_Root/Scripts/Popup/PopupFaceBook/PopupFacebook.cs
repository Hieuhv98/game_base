using Gamee_Hiukka.Data;
using System.Collections;
using System.Collections.Generic;
using Gamee_Hiukka.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Worldreaver.UniUI;

public class PopupFacebook : PopupBase
{
    string path = "https://www.facebook.com/groups/huggeepin/";
    public void Initialize() 
    {

    }

    public void OpenFacebook() 
    {
        Application.OpenURL(path);
        Close();
    }

}
