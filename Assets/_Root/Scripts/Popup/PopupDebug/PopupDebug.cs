using Gamee_Hiukka.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Worldreaver.UniUI;

public class PopupDebug : UniPopupBase
{
    [SerializeField] private UniButton btnBack;
    [SerializeField] private UniButton btnSave;
    [SerializeField] private TMP_InputField ifEnterScore;
    [SerializeField] private TMP_InputField ifEnterLevel;
    [SerializeField] private TMP_InputField ifEnterPass;
    [SerializeField] private TextMeshProUGUI txtNotiPass;

    private int score = 0;
    private int level = 1;
    private int eggID = 1;
    private int eggValue = 0;

    string pass = "Hiudz";
    private void Start()
    {
        ifEnterScore.contentType = TMP_InputField.ContentType.IntegerNumber;
        ifEnterLevel.contentType = TMP_InputField.ContentType.IntegerNumber;

        ifEnterPass.contentType = TMP_InputField.ContentType.Password;
    }
    public void Initialize() 
    {
        btnBack.onClick.RemoveAllListeners();
        btnBack.onClick.AddListener(Back);

        btnSave.onClick.RemoveListener(SaveData);
        btnSave.onClick.AddListener(SaveData);

        txtNotiPass.gameObject.SetActive(false);
    }

    private void SaveData() 
    {
/*        if (!ifEnterPass.text.Equals(pass))
        {
            txtNotiPass.gameObject.SetActive(true);
            return;
        }*/

        txtNotiPass.gameObject.SetActive(false);

        if (int.TryParse(ifEnterScore.text, out score))
        {
            GameData.CoinCurrent += score;
        }
        if (int.TryParse(ifEnterLevel.text, out level))
        {
            if(level > 0 && level < Config.LevelMax) 
            {
                GameData.LevelCurrent = level;
                GameData.LevelIndexCurrent = level - 1;
                GameData.LevelCurrentObj = null;
            }
        }

        Back();
    }
    private void Back() 
    {
        Close();
    }
}
