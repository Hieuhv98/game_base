using Gamee_Hiukka.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static SkinResources;

namespace Gamee_Hiukka.Data 
{
    public static partial class GameData
    {
        #region ingame
        public static List<int> LevelList { set; get; } = null;
        public static int LevelCurrent { set; get; } = 1;
        public static int LevelIndexCurrent { set; get; } = 0;
        public static string IDSkinCurrent { set; get; }
        public static string IDSkinSelect { set; get; }
        public static int SkinUnlockCout { set; get; } = 0;
        public static List<Color> Colors = new List<Color>();
        public static List<Texture2D> Textures = new List<Texture2D>();
        public static SkinData SkinCache { set; get; } = new SkinData();

        public static bool IsProcessFull { set; get; } = false;
        public static int CoinCurrent { set; get; } = 0;
        public static string MaterialBonus { set; get; }

        public static int LevelStartLoop
        {
            get => PlayerPrefs.GetInt(DataKey.LEVEL_START_LOOP, 1);

            set => PlayerPrefs.SetInt(DataKey.LEVEL_START_LOOP, value);
        }

        #endregion

        #region config
        public static GameObject LevelCurrentObj { set; get; }
        public static bool IsNewGame
        {
            get => PlayerPrefs.GetInt(DataKey.KEY_IS_NEW_GAME, 1) == 0 ? false : true;

            set => PlayerPrefs.SetInt(DataKey.KEY_IS_NEW_GAME, value == false ? 0 : 1);
        }
        public static string VersionApp { set; get; }
        public static string DescritptionApp { set; get; }
        public static bool IsDontShowUpdate = false;
        public static float VersionDontUpdate = 100;

        public static int IndexCountryCurrent { set; get; } = 0;
        public static string CountryCode { set; get; }
        public static string UserName { set; get; }
        public static string PlayerID { set; get; } = null;

        public static string customID { set; get; } = null;
        public static string CustomID
        {
            get
            {
                if (string.IsNullOrEmpty(customID))
                {
                    customID = System.Guid.NewGuid().ToString() + " time: " + System.DateTime.Now;
                    DataController.SaveCustomID();
                }
                return customID;
            }
        }

        public static bool IsShowRated 
        {
            get => PlayerPrefs.GetInt(DataKey.KEY_SHOW_RATED, 0) == 0 ? false : true;

            set => PlayerPrefs.SetInt(DataKey.KEY_SHOW_RATED, value == false ? 0 : 1);
        }
        #endregion

        #region setting
        public static bool AudioStatus { set; get; } = true;
        public static bool MusicStatus { set; get; } = true;
        public static bool VibrateStatus { set; get; } = true;
        #endregion

        #region daily
        public static bool NewDay = false;

        public static bool LoadValue(string key)
        {
            var value = PlayerPrefs.GetInt(DataKey.DAILY_REWARDED + key, 0);
            return value == 0 ? false : true;
        }

        public static void SaveValue(string key, bool value)
        {
            var i = value == false ? 0 : 1;
            PlayerPrefs.SetInt(DataKey.DAILY_REWARDED + key, i);
        }
        public static int MonthNow
        {
            get => PlayerPrefs.GetInt(DataKey.DAILY_MONTH_NOW, 1);

            set => PlayerPrefs.SetInt(DataKey.DAILY_MONTH_NOW, value);
        }
        public static int DayMissionIndex
        {
            get => PlayerPrefs.GetInt(DataKey.DAILY_MISSION_DAY_INDEX, 0);

            set => PlayerPrefs.SetInt(DataKey.DAILY_MISSION_DAY_INDEX, value);
        }

        private static DateTime _dayMissionTime;

        public static DateTime DayMissionTime
        {
            get
            {
                _dayMissionTime = DateTime.Parse(PlayerPrefs.GetString(DataKey.DAILY_MISSION_DAY_TIME, "01/01/0001 00:00:00"));
                return _dayMissionTime;
            }

            set
            {
                _dayMissionTime = value;
                PlayerPrefs.SetString(DataKey.DAILY_MISSION_DAY_TIME, _dayMissionTime.ToString());
            }
        }

        public static int DayRewardIndex
        {
            get => PlayerPrefs.GetInt(DataKey.DAILY_REWARD_DAY_INDEX, 0);

            set => PlayerPrefs.SetInt(DataKey.DAILY_REWARD_DAY_INDEX, value);
        }
        public static bool HasReward
        {
            get => PlayerPrefs.GetInt(DataKey.DAILY_HAS_REWARD, 0) == 0 ? false : true;

            set => PlayerPrefs.SetInt(DataKey.DAILY_HAS_REWARD, value == false ? 0 : 1);
        }

        public static int WeekRewardIndex => (DayRewardIndex - 1) / 7 + 1;
        public static void CheckNewDay() 
        {
            var cache = DayMissionTime;
            if ((DateTime.Now - cache.AddDays(1)).TotalSeconds > 0) 
            {
                NewDay = true;
                HasReward = true;
                DayMissionTime = DateTime.Now;
                Debug.Log(DateTime.Now.ToString());
                DayMissionIndex++;
                DayRewardIndex++;

                if (DayRewardIndex > 28)
                {
                    MonthNow++;
                    DayRewardIndex = 1;
                }
            }
        }

        #endregion

        #region pet
        public static int CurrentPet { set => PlayerPrefs.SetInt(DataKey.PET_CURRENT, value); get => PlayerPrefs.GetInt(DataKey.PET_CURRENT, -1); }
        public static int PetLevel { set => PlayerPrefs.SetInt(DataKey.PET_LEVEL, value); get => PlayerPrefs.GetInt(DataKey.PET_LEVEL, 1); }
        #endregion
        #region room
        public static int CurrentRoom { set => PlayerPrefs.SetInt(DataKey.ROOM_CURRENT, value); get => PlayerPrefs.GetInt(DataKey.ROOM_CURRENT, 1); }
        #endregion
    }
}

