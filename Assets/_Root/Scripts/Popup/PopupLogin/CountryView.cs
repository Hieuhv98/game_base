using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CountryView : MonoBehaviour
{
    public CountryResources contryResources;
    public CountryItem countryItem;
    [SerializeField] private Transform content;

    private int indexCountry = 0;
    public void Initialize()
    {
        InstanceItemContry();
    }
    private void InstanceItemContry() 
    {
        if(content.GetComponentInChildren<CountryItem>() == null) 
        {
            foreach (var country in contryResources.countries)
            {
                var item = Instantiate(countryItem, content);
                item.Initialize(country.countryName, country.flag, indexCountry, Hide);
                indexCountry++;
            }
        }
    }

    public void Show() 
    {
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
