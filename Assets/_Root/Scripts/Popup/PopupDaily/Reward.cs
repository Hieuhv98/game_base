using System.Collections;
using System.Collections.Generic;
using Gamee_Hiukka.Data;
using RescueFish.Controller;
using UnityEngine;

namespace Gamee_Hiukka.Daily
{
    [System.Serializable]
    public class Reward
    {
        [SerializeField] int day;
        [SerializeField] int coin;
        [SerializeField] Sprite icon;
        [SerializeField] bool isRewardSkin;
        [CustomSkinString, SerializeField] private string skinName; 

        public int Day => day;
        public int DayNow => day + (GameData.MonthNow - 1) * 28;
        public int Coin => coin;
        public Sprite Icon => icon;
        public bool IsRewardSkin=> isRewardSkin;
        public bool Active => day == GameData.DayRewardIndex;
        public string SkinName => skinName;
    }
}

