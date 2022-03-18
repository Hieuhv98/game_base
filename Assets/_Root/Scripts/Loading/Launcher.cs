using Cysharp.Threading.Tasks;
using Gamee_Hiukka.Data;
using UnityEngine;
using DG.Tweening;
using UniRx;
using System;
using static Gamee_Hiukka.Control.Gamemanager;

namespace Gamee_Hiukka.Control 
{
    public class Launcher : MonoBehaviour
    {
        [SerializeField] private FirebaseApp firebaseApp;
        public bool IsLoadDataComplete { set; get; } = false;

        public async void LoadData() 
        {
            IsLoadDataComplete = false;

            firebaseApp.Init();
            Vibration.Init();
            GameData.CheckNewDay();

            SkinResources.Instance.LoadDataAsset();
            DataController.LoadLevelCurrent();
            DataController.LoadCoinCurrent();
            DataController.LoadLevelIndexCurrent();
            DataController.LoadLevelList();
            DataController.LoadIsProcessFull();
            DataController.LoadIDSKinCurrent();
            DataController.LoadSKinUnlockCount();

            DataController.LoadUserName();
            DataController.LoadCountryCode();
            DataController.LoadPlayerID();
            DataController.LoadCustomID();

            DataController.LoadAudioStatus();
            DataController.LoadMusicStatus();
            DataController.LoadVibrateStatus();

            DataController.LoadRemoveAds();
            DataController.LoadUnlockAllSkin();
            DataController.LoadCoinX2();

            if (DataParam.coinX2) DataParam.coinX2Value = 2;
            else DataParam.coinX2Value = 1;

            if (GameData.LevelIndexCurrent >= Config.LevelMax)
            {
                Util.Shuffle(GameData.LevelList);
                GameData.LevelIndexCurrent = 0;
                DataController.SaveLevelList();
                DataController.SaveLevelIndexCurrent();
            }
            GameData.LevelCurrentObj = await BridgeData.GetLevel(GameData.LevelList[GameData.LevelIndexCurrent]);
            await LoadFileData();

            IsLoadDataComplete = true;
        }

        public async UniTask LoadFileData() 
        {
            await UniTask.Run(() =>
            {
                //LoadLevelMap();
            });
        }

        public void Setting()
        {
            SettingManager.Instance.UpdateSetting();
        }
    }
}

