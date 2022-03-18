using RescueFish.Controller;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SkinData
{
    [CustomSkinString]
    [SerializeField] string skinName;
    [SerializeField] string description;
    [SerializeField] int coin = 0;
    [SerializeField] bool isBuyCoin;
    [SerializeField] bool isWatchVideo;
    [SerializeField] bool isDailyReward;
    [SerializeField] bool isGiftCode;
    [SerializeField] string code;
    [HideInInspector, SerializeField] string id;
    [HideInInspector, SerializeField] bool isHas;

    public string SkinName => skinName;
    public string Description => description;
    public int Coin => coin;
    public bool IsBuyCoin => isBuyCoin;
    public bool IsWatchVideo => isWatchVideo;
    public bool IsDailyReward => isDailyReward;
    public bool IsGiftCode => isGiftCode;
    public string Code => code;
    public string ID => id;
    public bool IsHas
    {
        get => isHas;
        set => isHas = value;
    }
}
