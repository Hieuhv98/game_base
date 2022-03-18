using Gamee_Hiukka.Data;
using Gamee_Hiukka.Pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamee_Hiukka.Control 
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
