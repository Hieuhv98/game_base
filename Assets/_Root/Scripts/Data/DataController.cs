using Gamee_Hiukka.Data;
using Gamee_Hiukka.Helper;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gamee_Hiukka.Data;
using UnityEngine;

namespace Gamee_Hiukka.Control
{
    public static class DataController
    {
        #region game

        public static void LoadLevelCurrent()
        {
            GameData.LevelCurrent = PlayerPrefs.GetInt(DataKey.KEY_LEVEL_CURRENT, 1);
        }

        public static void SaveLevelCurrent() 
        {
            PlayerPrefs.SetInt(DataKey.KEY_LEVEL_CURRENT, GameData.LevelCurrent);
        }

        public static void LoadCoinCurrent()
        {
            GameData.CoinCurrent = PlayerPrefs.GetInt(DataKey.KEY_COIN_CURRENT, 0);
        }

        public static void SaveCoinCurrent()
        {
            PlayerPrefs.SetInt(DataKey.KEY_COIN_CURRENT, GameData.CoinCurrent);
        }

        // levels normal
        public static void LoadLevelIndexCurrent()
        {
            GameData.LevelIndexCurrent = PlayerPrefs.GetInt(DataKey.KEY_LEVEL_INDEX_CURRENT, 0);
        }

        public static void SaveLevelIndexCurrent()
        {
            PlayerPrefs.SetInt(DataKey.KEY_LEVEL_INDEX_CURRENT, GameData.LevelIndexCurrent);
        }

        public static void LoadLevelList()
        {
            string levelListData = LoadLevelListData();
            if(levelListData != "") 
            {
                GameData.LevelList = JsonHelper.FromJson<int>(levelListData).ToList();
            }
            else 
            {
                List<int> temp = new List<int>();
                for(int i = 1; i <= Config.LevelMax; i++) 
                {
                    temp.Add(i);
                }
                GameData.LevelList = temp;
                SaveLevelList();
            }
        }

        private static string LoadLevelListData() 
        {
            return PlayerPrefs.GetString(DataKey.KEY_LEVEL_LIST_DATA);
        }

        public static void SaveLevelList()
        {
            string levelListData = JsonHelper.ToJson(GameData.LevelList.ToArray());
            PlayerPrefs.SetString(DataKey.KEY_LEVEL_LIST_DATA, levelListData);
        }

        // levels bonus
        public static void LoadIsProcessFull() 
        {
            var isProcessFull = true;
            bool.TryParse(PlayerPrefs.GetString(DataKey.KEY_IS_PROCESS_FULL, "false"), out isProcessFull);
            GameData.IsProcessFull = isProcessFull;
        }

        public static void SaveIsProcessFull() 
        {
            PlayerPrefs.SetString(DataKey.KEY_IS_PROCESS_FULL, GameData.IsProcessFull.ToString());
        }

        private static string LoadLevelBonusListData()
        {
            return PlayerPrefs.GetString(DataKey.KEY_LEVEL_BONUS_LIST_DATA);
        }

        public static void LoadIDSKinCurrent() 
        {
            if (PlayerPrefs.GetString(DataKey.KEY_ID_SKIN_CURRENT) != "")
            {
                GameData.IDSkinCurrent = PlayerPrefs.GetString(DataKey.KEY_ID_SKIN_CURRENT);
            }
            else GameData.IDSkinCurrent = SkinResources.Instance.GetSkinDefaut().ID;
        }
        public static void SaveIDSkinCurrent() 
        {
            PlayerPrefs.SetString(DataKey.KEY_ID_SKIN_CURRENT, GameData.IDSkinCurrent);
        }

        public static void LoadSKinUnlockCount ()
        {
            GameData.SkinUnlockCout = PlayerPrefs.GetInt(DataKey.KEY_SKIN_UNLOCK_COUNT, 0);
        }
        public static void SaveSkinUnlockCount()
        {
            PlayerPrefs.SetInt(DataKey.KEY_SKIN_UNLOCK_COUNT, GameData.SkinUnlockCout);
        }
        #endregion
        
        #region iap
        public static void LoadRemoveAds() 
        {
            bool.TryParse(PlayerPrefs.GetString(DataKey.REMOVE_ADS, "false"), out DataParam.removeAds);
        }

        public static void SaveRemoveAds() 
        {
            PlayerPrefs.SetString(DataKey.REMOVE_ADS, DataParam.removeAds.ToString());
        }

        public static void LoadUnlockAllSkin()
        {
            bool.TryParse(PlayerPrefs.GetString(DataKey.UNLOCK_ALL_SKIN, "false"), out DataParam.unlockAllSkin);
        }

        public static void SaveUnlockAllSkin()
        {
            PlayerPrefs.SetString(DataKey.UNLOCK_ALL_SKIN, DataParam.unlockAllSkin.ToString());
        }

        public static void LoadCoinX2()
        {
            bool.TryParse(PlayerPrefs.GetString(DataKey.COIN_X2, "false"), out DataParam.coinX2);
        }

        public static void SaveCoinX2()
        {
            PlayerPrefs.SetString(DataKey.COIN_X2, DataParam.coinX2.ToString());
        }

        #endregion

        #region setting
        public static void LoadAudioStatus()
        {
            var audioStatus = true;
            bool.TryParse(PlayerPrefs.GetString(DataKey.KEY_AUDIO_STATUS, "true"), out audioStatus);
            GameData.AudioStatus = audioStatus;
        }
        public static void SaveAudioStatus()
        {
            PlayerPrefs.SetString(DataKey.KEY_AUDIO_STATUS, GameData.AudioStatus.ToString());
        }
        public static void LoadMusicStatus()
        {
            var musicStatus = true;
            bool.TryParse(PlayerPrefs.GetString(DataKey.KEY_MUSIC_STATUS, "true"), out musicStatus);
            GameData.MusicStatus = musicStatus;
        }
        public static void SaveMusicStatus()
        {
            PlayerPrefs.SetString(DataKey.KEY_MUSIC_STATUS, GameData.MusicStatus.ToString());
        }
        public static void LoadVibrateStatus()
        {
            var vibrateStatus = true;
            bool.TryParse(PlayerPrefs.GetString(DataKey.KEY_VIBRATE_STATUS, "true"), out vibrateStatus);
            GameData.VibrateStatus = vibrateStatus;
        }
        public static void SaveVibrateStatus()
        {
            PlayerPrefs.SetString(DataKey.KEY_VIBRATE_STATUS, GameData.VibrateStatus.ToString());
        }
        #endregion
        #region rank
        public static void SaveUserName()
        {
            PlayerPrefs.SetString(DataKey.KEY_USER_NAME, GameData.UserName);
        }
        public static void LoadUserName()
        {
            GameData.UserName = PlayerPrefs.GetString(DataKey.KEY_USER_NAME, null);
        }

        public static void SaveCountryCode()
        {
            PlayerPrefs.SetString(DataKey.KEY_COUNTRY_CODE, GameData.CountryCode);
        }
        public static void LoadCountryCode()
        {
            GameData.CountryCode = PlayerPrefs.GetString(DataKey.KEY_COUNTRY_CODE, null);
        }

        public static void SavePlayerID()
        {
            PlayerPrefs.SetString(DataKey.KEY_PLAYER_ID, GameData.PlayerID);
        }
        public static void LoadPlayerID()
        {
            GameData.PlayerID = PlayerPrefs.GetString(DataKey.KEY_PLAYER_ID, null);
        }

        public static void SaveCustomID()
        {
            PlayerPrefs.SetString(DataKey.KEY_CUSTOM_ID, GameData.customID);
        }
        public static void LoadCustomID()
        {
            GameData.customID = PlayerPrefs.GetString(DataKey.KEY_CUSTOM_ID, null);
        }
        #endregion

        #region save data by id
        public static void SaveDataByID(string id, bool isHas)
        {
            PlayerPrefs.SetString(id, isHas.ToString());
        }

        public static bool LoadDataByID(string id)
        {
            bool isHas = false;
            bool.TryParse(PlayerPrefs.GetString(id, null), out isHas);
            return isHas;
        }
        #endregion

        #region update
        public static void SaveStatusUpdate()
        {
            PlayerPrefs.SetString(DataKey.KEY_VERSION_DONT_UPDATE, GameData.IsDontShowUpdate.ToString());
        }
        public static void LoadStatusUpdate()
        {
            bool.TryParse(PlayerPrefs.GetString(DataKey.KEY_VERSION_DONT_UPDATE, "False"), out GameData.IsDontShowUpdate);
        }
        public static void SaveVersionDontUpdate()
        {
            PlayerPrefs.SetFloat(DataKey.KEY_STATUS_UPDATE, GameData.VersionDontUpdate);
        }
        public static void LoadVersionDontUpdate()
        {
            GameData.VersionDontUpdate = PlayerPrefs.GetFloat(DataKey.KEY_STATUS_UPDATE, 0);
        }
        #endregion

    }
}

