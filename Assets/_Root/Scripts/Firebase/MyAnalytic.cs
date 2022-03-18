using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamee_Hiukka.Control
{
    public static class MyAnalytic
    {
        // name
        public const string LEVEL_COMPLETE = "level_complete";
        public const string LEVEL_FAILED = "level_failed";
        public const string LEVEL_SKIP = "level_skip";
        public const string LEVEL_REPLAY = "level_replay";
        public const string LEVEL_START = "level_start";

        public const string SKIN_UNLOCKED = "skin_unlocked";
        public const string DAILY_REWARD_CLAIM = "daily_reward_claim";
        public const string DAILY_REWARD_CLAIM_BY_ADS = "daily_reward_claim_by_ads";
        public const string PROCESS_CLAIM_SKIN = "process_claim_skin";

        public const string AD_CLICK = "ad_click";
        public const string AD_BANNER_IMPRESSION = "ad_banner_impression";
        public const string AD_BANNER_REQUEST = "ad_banner_request";
        public const string AD_INTERSTITIAL_IMPRESSION = "ad_interstitial_impression";
        public const string AD_INTERSTITIAL_REQUEST = "ad_interstitial_request";
        public const string AD_REWARD_IMPRESSION = "ad_reward_impression";
        public const string AD_REWARD_REQUEST = "ad_reward_request";

        public const string PLAYER_SLIDE_STICK = "player_slide_stick";
        public const string PLAYER_CLICK_STICK = "player_click_stick";

        public const string CLAIM_FIRST_PET = "claim_first_pet";
        public const string CLAIM_DAILY_DAY_2 = "claim_daily_day_2";
        public const string CLAIM_DAILY_DAY_7 = "claim_daily_day_7";
        // paramater
        public const string LEVEL = "level";
        public const string SKIN = "skin";

        public static void LogEvent(string name, string paramater = null, string value = null)
        {
#if !UNITY_EDITOR
            if(paramater != null && value != null) 
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent(name, new Firebase.Analytics.Parameter(paramater, value));
            }else Firebase.Analytics.FirebaseAnalytics.LogEvent(name);
#endif
        }
    }
}

