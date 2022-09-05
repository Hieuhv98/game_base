using Cysharp.Threading.Tasks;
using Gamee_Hiukka.Data;
using UnityEngine;
using DG.Tweening;
using UniRx;
using System;
using static Gamee_Hiukka.Control.Gamemanager;
using UnityEngine.AddressableAssets;

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
            GameData.CheckNewDay();

            DataController.LoadLevelCurrent();
            DataController.LoadCoinCurrent();
            DataController.LoadLevelIndexCurrent();
            DataController.LoadLevelCollectionIndexCurrent();
            DataController.LoadLevelList();
            DataController.LoadLevelCollectionList();
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

            SkinResources.Instance.LoadDataAsset();

            if (DataParam.coinX2) DataParam.coinX2Value = 2;
            else DataParam.coinX2Value = 1;

            Addressables.InitializeAsync();
            BridgeData.CheckLoopLevel();
            if (Config.IsLevelCollection) GameData.LevelCurrentObj = await BridgeData.GetLevelCollection(GameData.LevelCollectionList[GameData.LevelCollectionIndexCurrent]);
            else GameData.LevelCurrentObj = await BridgeData.GetLevel(GameData.LevelList[GameData.LevelIndexCurrent]);
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

