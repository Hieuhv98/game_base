using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Worldreaver.UniUI;
using RescueFish.UI;
using UnityEngine.UI;
using TMPro;
using Gamee_Hiukka.Control;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.GameUI;
using Gamee_Hiukka.UI;

public class PopupGiftCode : PopupBase
{
    [SerializeField] private InputField ifName;
    [SerializeField] private Text txtEnterName;
    [SerializeField] private GameObject txtError;
    [SerializeField] private GameObject txtEnterCode;

    SkinData skinGiftCode = null;
    Action actionClose = null;
    int maxChar = 12;
    string path = "https://www.facebook.com/groups/huggeepin/";

    public void Start()
    {
        ifName.contentType = InputField.ContentType.Alphanumeric;
        ifName.characterLimit = 13;
        ifName.onValueChange.AddListener(delegate { Manage(); });
        skinGiftCode = SkinResources.Instance.GetSkinGiftCode();
    }

    public void Initialize(Action actionClose) 
    {
        this.actionClose = actionClose;
        txtError.SetActive(false);
        txtEnterCode.gameObject.SetActive(false);
    }

    public void Enter() 
    {
        if (skinGiftCode == null) return;
        txtError.gameObject.SetActive(false);
        txtEnterCode.gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(txtEnterName.text)) 
        {
            if(skinGiftCode.Code == txtEnterName.text) 
            {
                actionClose?.Invoke();
                Close();
            }
            else 
            {
                txtError.SetActive(true);
            }
        }
        else 
        {
            txtEnterCode.SetActive(true);
        }
    }

    void Manage() 
    {
        string text = ifName.text;
        if (text != ifName.text.ToUpper())
        {
            ifName.text = ifName.text.ToUpper();
        }
    }
    public void OpenFB() 
    {
        Application.OpenURL(path);
    }
}
