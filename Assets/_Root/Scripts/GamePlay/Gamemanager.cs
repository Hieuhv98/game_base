using Game_Base.Pattern;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Gamemanager : Singleton<Gamemanager>
{
    public GamePlayController gamePlayController;
    public Transform levelTransform;

    public EGameStatus eGameStatus;

    private GameObject _levelCurrent;
    private GameObject _levelOld;

    private void OnEnable()
    {
        if(levelTransform == null) 
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
        eGameStatus = EGameStatus.GAME_START;
        eGameStatus = EGameStatus.GAME_LOADING;
        _levelCurrent = await BridgeData.GetLevel(GameData.LevelCurrent);
        //_levelCurrent.SetActive(false);
        eGameStatus = EGameStatus.GAME_READY;
    }

    private void ShowLevelMap() 
    {
        if (_levelOld != null) DestroyLevelMap();

        StartCoroutine(WaitForShowLevelMap());
        ShowAdsBanner();
    }

    private IEnumerator WaitForShowLevelMap() 
    {
        yield return new WaitUntil(() => eGameStatus == EGameStatus.GAME_READY);
        eGameStatus = EGameStatus.GAME_PLAYING;
        _levelOld = _levelCurrent;
        Instantiate(_levelCurrent, levelTransform.transform);
        //_levelCurrent.SetActive(true);
    }
    private void DestroyLevelMap() { Destroy(_levelOld); }
    #endregion

    #region game
    public void GameWin() 
    {
        HideAdsBanner();
        eGameStatus = EGameStatus.GAME_WIN;
        GameData.LevelCurrent++;
        GamePopup.Instance.ShowPopupWin();
        LoadLevelMap();
    }
    public void GameLose() 
    {
        HideAdsBanner();
        eGameStatus = EGameStatus.GAME_LOSE;
        GamePopup.Instance.ShowPopupLose();
        LoadLevelMap();
    }

    public void GameNextLevel() 
    {
        ShowLevelMap();
    }
    public void GameReplayLevel() 
    {
        if(eGameStatus == EGameStatus.GAME_PLAYING) 
        {
            ShowLevelMap();
        }else if(eGameStatus == EGameStatus.GAME_WIN) 
        {
            GameData.LevelCurrent--;
            ShowLevelMap();
        }
    }

    #endregion

    #region ads
    private void ShowAdsBanner() { AdsManager.Instance.ShowAdsBanner(); }
    private void ShowAdsInterstitial() { AdsManager.Instance.ShowAdsInterstitial(); }
    private void HideAdsBanner() { AdsManager.Instance.HideAdsBanner(); }
    #endregion

    public enum EGameStatus
    {
        GAME_START,
        GAME_LOADING,
        GAME_READY,
        GAME_PLAYING,
        GAME_WIN,
        GAME_LOSE
    }

    public static class BridgeData 
    {
        public static async Task<GameObject> GetLevel(int index) 
        {
            if (Config.IsTest) return Config.LevelTest;
            Debug.Log(string.Format(Constant.LEVEL, index));
            return await Addressables.LoadAssetAsync<GameObject>(string.Format(Constant.LEVEL, index)).Task;
        }
    }
}
