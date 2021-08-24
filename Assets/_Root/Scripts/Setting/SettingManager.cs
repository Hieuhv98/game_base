using Game_Base.Data;
using Game_Base.Pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game_Base.Control 
{
    public class SettingManager : Singleton<SettingManager>
    {
        public void UpdateSetting()
        {
            SetupAudio();
            SetupMuzic();
            SetupVibrate();
        }

        public void SetupAudio() { }
        public void SetupMuzic()
        {
            if (GameData.MusicStatus)
            {
                AudioManager.Instance.PlayMusic();
            }
            else AudioManager.Instance.PauseMusic();
        }
        public void SetupVibrate() { }
    }
}
