using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamee_Hiukka.Data 
{
    public static class DataKey
    {
        private const string KEY_GAME = "GAMEE.COM.HUGGY_TIME_";

        #region ingame
        public const string KEY_LEVEL_CURRENT = KEY_GAME + "KEY_LEVEL_CURRENT";
        public const string KEY_COIN_CURRENT = KEY_GAME + "KEY_COIN_CURRENT";
        public const string KEY_LEVEL_INDEX_CURRENT = KEY_GAME + "KEY_LEVEL_INDEX_CURRENT";
        public const string KEY_LEVEL_BONUS_INDEX_CURRENT = KEY_GAME + "KEY_LEVEL_BONUS_INDEX_CURRENT";
        public const string KEY_LEVEL_LIST_DATA = KEY_GAME + "KEY_LEVEL_LIST_DATA";
        public const string KEY_LEVEL_BONUS_LIST_DATA = KEY_GAME + "KEY_LEVEL_BONUS_LIST_DATA";
        public const string KEY_IS_PROCESS_FULL = KEY_GAME + "KEY_IS_PROCESS_FULL";
        public const string KEY_IS_NEW_GAME = KEY_GAME + "KEY_IS_NEW_GAME";

        public const string KEY_ID_SKIN_CURRENT = KEY_GAME + "KEY_ID_SKIN_CURRENT";
        public const string KEY_SKIN_UNLOCK_COUNT = KEY_GAME + "KEY_SKIN_UNLOCK_COUNT";

        public const string KEY_SHOW_RATED = KEY_GAME + "KEY_SHOW_RATED";

        public const string KEY_VERSION_DONT_UPDATE = KEY_GAME + "KEY_VERSION_DONT_UPDATE";
        public const string KEY_STATUS_UPDATE = KEY_GAME + "KEY_STATUS_UPDATE";
        public const string LEVEL_START_LOOP = KEY_GAME + "_LEVEL_START_LOOP";
        #endregion
        #region iap
        public static string REMOVE_ADS = KEY_GAME + "REMOVE_ADS";
        public static string UNLOCK_ALL_SKIN = KEY_GAME + "UNLOCK_ALL_SKIN";
        public static string COIN_X2 = KEY_GAME + "COIN_X2";
        public static string COMBO = KEY_GAME + "COMBO";
        #endregion
        #region rank
        public const string KEY_USER_NAME = KEY_GAME + "KEY_USER_NAME";
        public const string KEY_COUNTRY_CODE = KEY_GAME + "KEY_COUNTRY_CODE";
        public const string KEY_PLAYER_ID = KEY_GAME + "KEY_PLAYER_ID";
        public const string KEY_CUSTOM_ID = KEY_GAME + "KEY_CUSTOM_ID";
        
        #endregion
        #region setting
        public const string KEY_AUDIO_STATUS = KEY_GAME + "KEY_AUDIO_STATUS";
        public const string KEY_MUSIC_STATUS = KEY_GAME + "KEY_MUSIC_STATUS"; 
        public const string KEY_VIBRATE_STATUS = KEY_GAME + "KEY_VIBRATE_STATUS";
        #endregion

        #region MyRegion
        public const string DAILY_MISSION_DAY_INDEX = KEY_GAME + "_DAILY_MISSION_DAY_INDEX";
        public const string DAILY_MISSION_DAY_TIME = KEY_GAME + "_DAILY_MISSION_DAY_TIME";
        public const string DAILY_REWARD_DAY_INDEX = KEY_GAME + "_DAILY_REWARD_DAY_INDEX";
        public const string DAILY_HAS_REWARD = KEY_GAME + "_DAILY_HAS_REWARD";
        public const string DAILY_REWARDED = KEY_GAME + "_DAILY_REWARDED";
        public const string DAILY_MONTH_NOW = KEY_GAME + "_DAILY_MONTH_NOW";
        #endregion
        #region pet
        public const string PET_CURRENT = KEY_GAME + "_PET_CURRENT";
        public const string PET_LEVEL = KEY_GAME + "_PET_LEVEL";
        #endregion
        #region room
        public const string ROOM_CURRENT = KEY_GAME + "_ROOM_CURRENT";
        #endregion

    }
}

