using Game_Base.Common;
using Game_Base.Control;
using System.Collections.Generic;
using UnityEngine;

namespace Game_Base.Data
{
    public class Config : ScriptableObject
    {
        private static Config _instance;
        public static Config Instance => _instance ? _instance : Resources.Load<Config>(Constant.CONFIG_PATH);
        [Header("Game Test")]
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
        public static List<string> IdDeviceTests { set => Instance.idDeviceTests = value; get => Instance.idDeviceTests; }

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

