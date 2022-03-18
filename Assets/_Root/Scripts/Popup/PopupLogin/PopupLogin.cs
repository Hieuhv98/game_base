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

public class PopupLogin : PopupBase
{
    [SerializeField] private InputField ifName;
    [SerializeField] private Text txtEnterName;
    [SerializeField] private GameObject txtError;
    [SerializeField] private CountrySelect contrySelect;

    public void Start()
    {
        ifName.contentType = InputField.ContentType.Alphanumeric;
    }

    public void Initialize() 
    {
        contrySelect.Initialize();

        txtError.SetActive(false);
    }

    public void Login() 
    {
        GameData.CountryCode = contrySelect.countryCodeSelect;
        DataController.SaveCountryCode();
        if (!string.IsNullOrEmpty(txtEnterName.text)) 
        {
            GameData.UserName = txtEnterName.text;
            DataController.SaveUserName();
            Close();
            GamePopup.Instance.ShowPopupRank();
        }
        else 
        {
            txtError.SetActive(true);
        }
    }
}
