using Gamee_Hiukka.Common;
using Gamee_Hiukka.Control;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamee_Hiukka.Data 
{
    public class Config : ScriptableObject
    {
        private static Config _instance;
        public static Config Instance => _instance ?? (_instance = Resources.Load<Config>(Constant.CONFIG_PATH));

        [Header("Game")]
        [SerializeField] private int levelMax = 100;
        [SerializeField] private int levelCollectionMax = 10;
        [SerializeField] private int interstitialAdShowCount = 2;
        [SerializeField] private int interstitialAdFirstShowCount = 3;
        [SerializeField] private int timeInterAdShow = 25;
        [SerializeField] private int timeInterAdShowLose = 25;
        [SerializeField] private bool isShowInterAdsLose = false;
        [SerializeField] private int watchVideoValue = 5;
        [SerializeField] private int levelStartLoop = 4;
        [SerializeField] private bool autoStartGame;
        [SerializeField] private int processCount = 7;
        [SerializeField] private int levelShowRate = 10;
        [SerializeField] private int levelShowRateLate = 30;
        [SerializeField] private int levelShowRateLateCount = 20;
        [SerializeField] private bool isAutoShowDailyReward;
        [SerializeField] private bool isShowInterAdsBeforeWin;
        [SerializeField] private bool isShowLevelCollection = false;
        [SerializeField] private int levelCountCollectionShow = 5;
        [SerializeField] private int levelShowUpdate = 5;

        [Header("Pin")]
        [SerializeField, Range(0.1f, 5f)] private float stickSpeedBackwardCoefficient = 1f;

        [Space,Header("Game Test")]
        [SerializeField] private bool isTest;
        [SerializeField] private LevelMap levelTest;

        [Space, Header("Ads")]
        [SerializeField] private bool isAdmob;
        [Header("android")] [SerializeField] private string admobAndroidBannerId = "ca-app-pub-3940256099942544/6300978111";
        [SerializeField] private string admobAndroidIntertitialId = "ca-app-pub-3940256099942544/1033173712";
        [SerializeField] private string admobAndroidRewardedId = "ca-app-pub-3940256099942544/5224354917";
        [SerializeField] private string admobAndroidRewardedIntertitialId = "ca-app-pub-2954224017334927/8087504625";

        [Header("ios")] [SerializeField] private string admobIosBannerId = "ca-app-pub-3940256099942544/2934735716";
        [SerializeField] private string admobIosIntertitialId = "ca-app-pub-3940256099942544/4411468910";
        [SerializeField] private string admobIosRewardedId = "ca-app-pub-3940256099942544/3986624511";
        [SerializeField] private string admobIosRewardedIntertitialId = "ca-app-pub-2954224017334927/8087504625";
        [SerializeField] private List<string> idDeviceTests;

        public static bool IsTest { set => Instance.isTest = value; get => Instance.isTest; }
        public static LevelMap LevelTest { set => Instance.levelTest = value; get => Instance.levelTest; }

        public static bool IsAdmob { set => Instance.isAdmob = value; get => Instance.isAdmob; }
        public static bool AutoStartGame { set => Instance.autoStartGame = value; get => Instance.autoStartGame; }
        public static List<string> IdDeviceTests { set => Instance.idDeviceTests = value; get => Instance.idDeviceTests; }

        public static float StickSpeedBackwardCoefficient => Instance.stickSpeedBackwardCoefficient;

        public static int LevelMax { set => Instance.levelMax = value; get => Instance.levelMax; }
        public static int LevelCollectionMax { set => Instance.levelCollectionMax = value; get => Instance.levelCollectionMax; }
        public static string AdmobAndroidBannerId { set => Instance.admobAndroidBannerId = value; get => Instance.admobAndroidBannerId; }
        public static string AdmobAndroidIntertitialId { set => Instance.admobAndroidIntertitialId = value; get => Instance.admobAndroidIntertitialId; }
        public static string AdmobAndroidRewardedId { set => Instance.admobAndroidRewardedId = value; get => Instance.admobAndroidRewardedId; }
        public static string AdmobIosBannerId { set => Instance.admobIosBannerId = value; get => Instance.admobIosBannerId; }
        public static string AdmobIosIntertitialId { set => Instance.admobIosIntertitialId = value; get => Instance.admobIosIntertitialId; }
        public static string AdmobIosRewardedId { set => Instance.admobIosRewardedId = value; get => Instance.admobIosRewardedId; }
        public static int TimeInterAdShow { set => Instance.timeInterAdShow = value; get => Instance.timeInterAdShow; }
        public static int TimeInterAdShowLose { set => Instance.timeInterAdShowLose = value; get => Instance.timeInterAdShowLose; }
        public static bool IsShowInterAdsLose { set => Instance.isShowInterAdsLose = value; get => Instance.isShowInterAdsLose; }
        public static int WatchVideoValue { set => Instance.watchVideoValue = value; get => Instance.watchVideoValue; }
        public static int InterstitialAdShowCount { set => Instance.interstitialAdShowCount = value; get => Instance.interstitialAdShowCount; }
        public static int InterstitialAdFirstShowCount { set => Instance.interstitialAdFirstShowCount = value; get => Instance.interstitialAdFirstShowCount; }
        public static int InterstitialAdCountCurrent { set => Instance._interstitialAdCountCurrent = value; get => Instance._interstitialAdCountCurrent; }
        public static DateTime TimeAtInterstitialAdShow { set => Instance._timeAtInterstitialAdShow = value; get => Instance._timeAtInterstitialAdShow; }
        public static DateTime TimeAtInterstitialAdLoseShow { set => Instance._timeAtInterstitialAdLoseShow = value; get => Instance._timeAtInterstitialAdLoseShow; }
        public static int ProcessCount { set => Instance.processCount = value; get => Instance.processCount; }
        public static int LevelShowRate => Instance.levelShowRate;
        public static int LevelShowRateLate => Instance.levelShowRateLate;
        public static int LevelShowRateLateCount => Instance.levelShowRateLateCount;
        public static bool IsAutoShowDailyReward { set => Instance.isAutoShowDailyReward = value; get => Instance.isAutoShowDailyReward; }
        public static bool IsShowInterAdsBeforeWin { set => Instance.isShowInterAdsBeforeWin = value; get => Instance.isShowInterAdsBeforeWin && AdsManager.Instance.IsInterLoaded; }
        public static int LevelStartLoop => Instance.levelStartLoop;
        public static bool IsLevelCollection => GameData.LevelCurrent % Instance.levelCountCollectionShow == 0 && Instance.isShowLevelCollection;
        public static int LevelShowUpdate => Instance.levelShowUpdate;
        public static bool IsRotationByCamera { set ; get ; }

        private int _interstitialAdCountCurrent = 1;
        private DateTime _timeAtInterstitialAdShow = new DateTime();
        private DateTime _timeAtInterstitialAdLoseShow = new DateTime();

        public static string AdmobBannerId
        {
            get
            {
#if UNITY_ANDROID
                return Instance.admobAndroidBannerId;
#elif UNITY_IOS
                return Instance.admobIosBannerId;
#else
                return "";
#endif
            }
        }

        public static string AdmobInterstitialId
        {
            get
            {
#if UNITY_ANDROID
                return Instance.admobAndroidIntertitialId;
#elif UNITY_IOS
                return Instance.admobIosIntertitialId;
#else
                return "";
#endif
            }
        }

        public static string AdmobRewardedId
        {
            get
            {
#if UNITY_ANDROID
                return Instance.admobAndroidRewardedId;
#elif UNITY_IOS
                return Instance.admobIosRewardedId;
#else
                return "";
#endif
            }
        }

        public static string AdmobRewardedIntertitialId
        {
            get
            {
#if UNITY_ANDROID
                return Instance.admobAndroidRewardedIntertitialId;
#elif UNITY_IOS
                return Instance.admobIosRewardedIntertitialId;
#else
                return "";
#endif
            }
        }
    }
}

