using Cysharp.Threading.Tasks;
using Game_Base.Common;
using Game_Base.Data;
using Game_Base.GameUI;
using Game_Base.Pattern;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game_Base.Control 
{
    public class Gamemanager : Singleton<Gamemanager>
    {
        public GamePlayController gamePlayController;
        public Transform levelTransform;

        public EGameStatus eGameStatus;
        public EGameLoadData eGameLoadData;

        private GameObject _levelCurrent;
        private GameObject _levelView;

        private void OnEnable()
        {
            if (levelTransform == null)
            {
                GameObject level = new GameObject("_Level");
                levelTransform = level.transform;
            }
        }

        private void Start()
        {
            LoadLevelMap();
            ShowLevelMap();
        }

        #region level map
        private async void LoadLevelMap()
        {
            eGameLoadData = EGameLoadData.GAME_DATA_LOADING;
            _levelCurrent = await BridgeData.GetLevel(GameData.LevelCurrent);
            //_levelCurrent.SetActive(false);
            eGameLoadData = EGameLoadData.GAME_DATA_READY;
        }

        private void ShowLevelMap()
        {
            if(_levelView != null) DestroyLevelMap();

            StartCoroutine(WaitForShowLevelMap());
            gamePlayController.ShowAdsBanner();
        }

        private IEnumerator WaitForShowLevelMap()
        {
            yield return new WaitUntil(() => eGameLoadData == EGameLoadData.GAME_DATA_READY);
            eGameStatus = EGameStatus.GAME_START;

            _levelView = Instantiate(_levelCurrent, levelTransform.transform);

            eGameStatus = EGameStatus.GAME_PLAYING;

            //_levelCurrent.SetActive(true);
        }
        private void DestroyLevelMap() {Destroy(_levelView);}
        #endregion

        #region game
        public void GameWin()
        {
            gamePlayController.HideAdsBanner();
            eGameStatus = EGameStatus.GAME_WIN;
            GameData.LevelCurrent++;
            gamePlayController.ShowPopupWin();
            LoadLevelMap();
        }
        public void GameLose()
        {
            gamePlayController.HideAdsBanner();
            eGameStatus = EGameStatus.GAME_LOSE;
            gamePlayController.ShowPopupLose();
            LoadLevelMap();
        }

        public void GameNextLevel()
        {
            ShowLevelMap();
        }
        public void GameReplayLevel()
        {
            if (eGameStatus == EGameStatus.GAME_PLAYING || eGameStatus == EGameStatus.GAME_LOSE)
            {
                ShowLevelMap();
            }
            else if (eGameStatus == EGameStatus.GAME_WIN)
            {
                GameData.LevelCurrent--;
                LoadLevelMap();
                ShowLevelMap();
            }
        }

        #endregion

        public enum EGameStatus
        {
            GAME_START,
            GAME_PLAYING,
            GAME_WIN,
            GAME_LOSE
        }

        public enum EGameLoadData 
        {
            GAME_DATA_LOADING,
            GAME_DATA_READY,
        }

        public static class BridgeData
        {
            public static async UniTask<GameObject> GetLevel(int index)
            {
                if (Config.IsTest) return Config.LevelTest.gameObject;
                return await Addressables.LoadAssetAsync<GameObject>(string.Format(Constant.LEVEL, index));
            }
        }
    }
}

