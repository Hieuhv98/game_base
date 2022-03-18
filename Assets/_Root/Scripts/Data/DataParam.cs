namespace Gamee_Hiukka.Data
{
    using UnityEngine;
    public static class DataParam
    {
        public static bool removeAds = false;
        public static bool unlockAllSkin = false;
        public static bool coinX2 = false;
        public static int coinX2Value = 1;

        public const string key = "com.huggypin."; 
        public const string REMOVE_ADS = key + "removeads";
        public const string ADD_COIN_PACK_1 = key + "50kgold";
        public const string ADD_COIN_PACK_2 = key + "500kgold";
        public const string COIN_X2 = key + "x2gold";
        public const string UNLOCK_ALL_SKIN = key + "unlockallskin";
        public const string COMBO = key + "vip";

        public const int COIN_PACK_1 = 50000;
        public const int COIN_PACK_2 = 500000;
        public const int COIN_FREE = 500;
        public const int GITCOIN = 100;
    }

}

