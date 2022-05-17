using Firebase.Extensions;
using Game_Base.Control;
using Gamee_Hiukka.Control;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.Pattern;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace Gamee_Hiukka.Control
{
    public class RemoteConfig : MonoBehaviour
    {
        private const string IS_ADMOB = "IS_ADMOB";
#if UNITY_ANDROID
        private const string ADMOB_ANDROID_BANNER_ID = "ADMOB_ANDROID_BANNER_ID";
        private const string ADMOB_ANDROID_INTERTITIAL_ID = "ADMOB_ANDROID_INTERTITIAL_ID";
        private const string ADMOB_ANDROID_REWARDED_ID = "ADMOB_ANDROID_REWARDED_ID";
#elif UNITY_IOS
        private const string ADMOB_IOS_BANNER_ID = "ADMOB_IOS_BANNER_ID";
        private const string ADMOB_IOS_INTERTITIAL_ID = "ADMOB_IOS_INTERTITIAL_ID";
        private const string ADMOB_IOS_REWARDED_ID = "ADMOB_IOS_REWARDED_ID";
#endif
        private const string IS_REWARDED_INTERSTITIAL = "IS_REWARDED_INTERSTITIAL";
        private const string IS_SHOW_CROSS_ADS = "IS_SHOW_CROSS_ADS";
        private const string IS_SHOW_TUTORIAL = "IS_SHOW_TUTORIAL";
        private const string INTER_AD_SHOW_COUNT = "INTER_AD_SHOW_COUNT";
        private const string INTER_AD_SHOW_COUNT_IN_NEW_APP = "INTER_AD_SHOW_COUNT_IN_NEW_APP";
        private const string TIME_INTER_AD_SHOW_DELAY = "TIME_INTER_AD_SHOW_DELAY";
        private const string TIME_INTER_AD_SHOW_LOSE_DELAY = "TIME_INTER_AD_SHOW_LOSE_DELAY";
        private const string IS_INTER_ADS_LOSE = "IS_INTER_ADS_LOSE";
        private const string VERSION_APP = "VERSION_APP";
        private const string VERSION_APP_IOS = "VERSION_APP_IOS";
        private const string DESCRIPTION_APP = "DESCRIPTION_APP";

        private const string AUTO_START_GAME = "AUTO_START_GAME";
        private readonly Dictionary<string, object> defaults = new Dictionary<string, object>();

        public void Init()
        {
            defaults.Add(IS_ADMOB, "true");
#if UNITY_ANDROID
            defaults.Add(ADMOB_ANDROID_BANNER_ID, "ca-app-pub-8566745611252640/3512982444");
            defaults.Add(ADMOB_ANDROID_INTERTITIAL_ID, "ca-app-pub-8566745611252640/9886819107");
            defaults.Add(ADMOB_ANDROID_REWARDED_ID, "ca-app-pub-8566745611252640/5947574099");
#elif UNITY_IOS
                    defaults.Add(ADMOB_IOS_BANNER_ID, "ca-app-pub-8566745611252640/3693336145");
                    defaults.Add(ADMOB_IOS_INTERTITIAL_ID, "ca-app-pub-8566745611252640/1067172801");
                    defaults.Add(ADMOB_IOS_REWARDED_ID, "ca-app-pub-8566745611252640/3412659761");
#endif
            defaults.Add(IS_REWARDED_INTERSTITIAL, "false");
            defaults.Add(IS_SHOW_CROSS_ADS, "false");
            defaults.Add(IS_SHOW_TUTORIAL, "true");
            defaults.Add(INTER_AD_SHOW_COUNT, "3");
            defaults.Add(TIME_INTER_AD_SHOW_DELAY, "25");
            defaults.Add(TIME_INTER_AD_SHOW_LOSE_DELAY, "25");
            defaults.Add(IS_INTER_ADS_LOSE, "false");
            defaults.Add(INTER_AD_SHOW_COUNT_IN_NEW_APP, "3");
            defaults.Add(VERSION_APP, "1.0");
            defaults.Add(VERSION_APP_IOS, "1.0");
            defaults.Add(DESCRIPTION_APP, "New Update");
            defaults.Add(AUTO_START_GAME, "false");
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);

            FetchDataAsync();
        }

        private Task FetchDataAsync()
        {
            Task fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            return fetchTask.ContinueWith(FetchComplete);
        }

        private void FetchComplete(Task fetchTask)
        {
            if (fetchTask.IsCanceled)
            {
                Debug.Log("Fetch canceled.");
            }
            else if (fetchTask.IsFaulted)
            {
                Debug.Log("Fetch encountered an error.");
            }
            else if (fetchTask.IsCompleted)
            {
                Debug.Log("Fetch completed successfully!");
            }

            var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
            switch (info.LastFetchStatus)
            {
                case Firebase.RemoteConfig.LastFetchStatus.Success:
                    Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
                    Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                    info.FetchTime));
                    break;
                case Firebase.RemoteConfig.LastFetchStatus.Failure:
                    switch (info.LastFetchFailureReason)
                    {
                        case Firebase.RemoteConfig.FetchFailureReason.Error:
                            Debug.Log("Fetch failed for unknown reason");
                            break;
                        case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                            Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                            break;
                    }
                    break;
                case Firebase.RemoteConfig.LastFetchStatus.Pending:
                    Debug.Log("Latest Fetch call still pending.");
                    break;
            }

            Config.IsAdmob = bool.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(IS_ADMOB).StringValue);

#if UNITY_ANDROID
            Config.AdmobAndroidBannerId = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(ADMOB_ANDROID_BANNER_ID).StringValue;
            Config.AdmobAndroidIntertitialId = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(ADMOB_ANDROID_INTERTITIAL_ID).StringValue;
            Config.AdmobAndroidRewardedId = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(ADMOB_ANDROID_REWARDED_ID).StringValue;
#elif UNITY_IOS
            Config.AdmobIosBannerId = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(ADMOB_IOS_BANNER_ID).StringValue;
            Config.AdmobIosIntertitialId = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(ADMOB_IOS_INTERTITIAL_ID).StringValue;
            Config.AdmobIosRewardedId = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(ADMOB_IOS_REWARDED_ID).StringValue;
#endif
            Config.InterstitialAdShowCount = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(INTER_AD_SHOW_COUNT).StringValue);
            Config.InterstitialAdFirstShowCount = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(INTER_AD_SHOW_COUNT_IN_NEW_APP).StringValue);

            Config.TimeInterAdShow = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(TIME_INTER_AD_SHOW_DELAY).StringValue);
            Config.TimeInterAdShowLose = int.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(TIME_INTER_AD_SHOW_LOSE_DELAY).StringValue);
            Config.IsShowInterAdsLose = bool.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(IS_INTER_ADS_LOSE).StringValue);

#if UNITY_IOS
            GameData.VersionApp = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(VERSION_APP_IOS).StringValue;
#elif UNITY_ANDROID || UNITY_EDITOR
            GameData.VersionApp = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(VERSION_APP).StringValue;
#endif

            GameData.DescritptionApp = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(DESCRIPTION_APP).StringValue;
            Config.AutoStartGame = bool.Parse(Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(AUTO_START_GAME).StringValue);

            DOTween.Sequence().SetDelay(.1f).OnComplete(() => AdsManager.Instance.InitAds()) ;
        }
    }
}
