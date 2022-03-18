using Gamee_Hiukka.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Worldreaver.UniUI;

public class CountrySelect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtCountry;
    [SerializeField] private Image imgFlag;
    [SerializeField] private CountryView viewContry;
    private bool isDown = false;
    private List<Country> countries;
    private int indexCountry = -1;

    public string countryCodeSelect { get; set; }
    private void Awake()
    {
        countries = CountryResources.Instance.GetCountries();
        GameData.IndexCountryCurrent = CountryResources.Instance.GetCountryIndexDefaut("US");

    }
    public void Initialize()
    {
        viewContry.Initialize();
        viewContry.gameObject.SetActive(false);
    }

    public void Update()
    {
        if(indexCountry != GameData.IndexCountryCurrent) 
        {
            indexCountry = GameData.IndexCountryCurrent;
            UpdateDisplay(indexCountry);
        }
    }

    private void UpdateDisplay(int index) 
    {
        txtCountry.text = countries[index].countryName;
        countryCodeSelect = countries[index].countryCode.ToString();
        imgFlag.sprite = countries[index].flag;
    }

    public void View()
    {
        isDown = !isDown;
        if (isDown)
        {
            viewContry.Show();
        }
        else viewContry.Hide();
    }
}
