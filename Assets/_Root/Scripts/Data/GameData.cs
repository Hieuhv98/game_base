using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game_Base.Data 
{
    public static class GameData
    {
        public static int LevelCurrent { set; get; } = 1;

        #region setting
        public static bool AudioStatus { set; get; } = true;
        public static bool MusicStatus { set; get; } = true;
        public static bool VibrateStatus { set; get; } = true;
        #endregion
    }
}

