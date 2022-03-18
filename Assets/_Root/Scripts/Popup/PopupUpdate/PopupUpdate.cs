using Gamee_Hiukka.Control;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Worldreaver.UniUI;

public class PopupUpdate : PopupBase
{
    [SerializeField] private ButtonBase btnBack;
    [SerializeField] private ButtonBase btnUpdate;
    [SerializeField] private TextMeshProUGUI txtDescription;
    [SerializeField] private TextMeshProUGUI txtVersion;
    [SerializeField] private Toggle tgCheck;

    private bool isStatus = false;
    public void Initialize()
    {
        btnBack.onClick.RemoveListener(Back);
        btnBack.onClick.AddListener(Back);

        btnUpdate.onClick.RemoveListener(UpdateVersion);
        btnUpdate.onClick.AddListener(UpdateVersion);

        //txtDescription.text = GameData.DescritptionApp.Replace("\\n", "\n");
        txtDescription.text = GameData.DescritptionApp;
        txtVersion.text = "Version " + GameData.VersionApp;
        tgCheck.isOn = false;
    }

    private void Update()
    {
        if(isStatus != tgCheck.isOn) 
        {
            isStatus = tgCheck.isOn;
        }
    }

    private void Back() 
    {
        if (isStatus) 
        {
            GameData.IsDontShowUpdate = true;
            GameData.VersionDontUpdate = Util.ConvertVersion(GameData.VersionApp);

            DataController.SaveStatusUpdate();
            DataController.SaveVersionDontUpdate();
        }

        Close();
    }

    public void UpdateVersion() 
    {
        AppRatingManager.Instance.OpenApp();
    }

}
