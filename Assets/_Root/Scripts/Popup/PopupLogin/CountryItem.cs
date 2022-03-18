using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Worldreaver.UniUI;
using System;
using Gamee_Hiukka.Data;

public class CountryItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtCountry;
    [SerializeField] private Image imgFlag;

    private int index = 0;
    private UniButton btnClick;
    private Action _actionHide;
    private void Awake()
    {
        btnClick = GetComponent<UniButton>();
        btnClick.onClick.RemoveListener(UpdateDisplay);
        btnClick.onClick.AddListener(UpdateDisplay);
    }

    public void Initialize(string countryName, Sprite flag, int index, Action actionHide) 
    {
        _actionHide = actionHide;
        this.txtCountry.text = countryName;
        this.imgFlag.sprite = flag;
        this.index = index;
    }

    public void UpdateDisplay() 
    {
        GameData.IndexCountryCurrent = index;
        _actionHide?.Invoke();
    }
}
