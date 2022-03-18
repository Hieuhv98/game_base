using UnityEngine;
using Worldreaver.UniUI;
using System;
using RescueFish.Controller;
using TMPro;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.Control;

namespace Gamee_Hiukka.UI 
{
    public class PopupSetting : PopupBase
    {
        [SerializeField] private ButtonBase btnRestore;
        [SerializeField] private ButtonBase btnRateUs;
        [SerializeField] private ButtonBase btnFacebook;
        [SerializeField] private SettingItem audioSetting;
        [SerializeField] private SettingItem musicSetting;
        [SerializeField] private SettingItem vibrateSetting;
        [SerializeField] private TextMeshProUGUI txtVersion;
        private Action _actionBack;
        public void Initialize(Action actionBack)
        {
            UpdateStatus();
            UpdateDisplay();
            _actionBack = actionBack;

            btnRestore.onClick.RemoveListener(RestorePurchase);
            btnRestore.onClick.AddListener(RestorePurchase);

            btnRateUs.onClick.RemoveListener(RateUs);
            btnRateUs.onClick.AddListener(RateUs);

            btnFacebook.onClick.RemoveListener(OpenFacebook);
            btnFacebook.onClick.AddListener(OpenFacebook);
        }

        public void SelectAudio()
        {
            GameData.AudioStatus = !GameData.AudioStatus;
            audioSetting.GetComponent<SettingItem>().UpdateStatus(GameData.AudioStatus);
            DataController.SaveAudioStatus();
            SettingManager.Instance.SetupAudio();
        }
        public void SelectMuzic()
        {
            GameData.MusicStatus = !GameData.MusicStatus;
            musicSetting.GetComponent<SettingItem>().UpdateStatus(GameData.MusicStatus);
            DataController.SaveMusicStatus();
            SettingManager.Instance.SetupMuzic();
        }
        public void SelectVibrate()
        {
            GameData.VibrateStatus = !GameData.VibrateStatus;
            vibrateSetting.GetComponent<SettingItem>().UpdateStatus(GameData.VibrateStatus);
            DataController.SaveVibrateStatus();
            SettingManager.Instance.SetupVibrate();
        }

        public void UpdateStatus()
        {
            audioSetting.GetComponent<SettingItem>().UpdateStatus(GameData.AudioStatus);
            musicSetting.GetComponent<SettingItem>().UpdateStatus(GameData.MusicStatus);
            vibrateSetting.GetComponent<SettingItem>().UpdateStatus(GameData.VibrateStatus);
        }

        private void RestorePurchase()
        {
            IAPManager.Instance.RestorePurchases();
        }
        private void RateUs()
        {
            AppRatingManager.Instance.OpenApp();
            //AppRatingManager.Instance.RateUs();
        }

        private void UpdateDisplay()
        {
            txtVersion.text = Application.version.ToString();

            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                btnRestore.gameObject.SetActive(true);
            }
            else
            {
                btnRestore.gameObject.SetActive(false);
            }
        }

        private void OpenFacebook()
        {
            Application.OpenURL("https://www.facebook.com/groups/840311413538901");
        }

        public void Back()
        {
            _actionBack?.Invoke();
        }
    }
}


