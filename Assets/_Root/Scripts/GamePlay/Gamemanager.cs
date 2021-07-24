using GameName.Pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Gamemanager : Singleton<Gamemanager>
{
    public GamePlayController gamePlayController;
    public Transform levelTransform;

    public EGameStatus eGameStatus;


    private GameObject _level;

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
    }

    // level map
    private void LoadLevelMap() 
    {
        Addressables.LoadAssetAsync<GameObject>(string.Format("Level_{0}", GameData.LevelCurrent));

        _level.SetActive(false);
    }
    private void ShowLevelMap() 
    {
        eGameStatus = EGameStatus.GAME_PLAYING;
        _level.SetActive(true);
    }
    private void DestroyLevelMap() { Destroy(_level); }

    // win - lose
    public void GameWin() 
    {
        GameData.LevelCurrent++;
        gamePlayController.ShowWin();
        LoadLevelMap();
    }
    public void GameLose() 
    {
        gamePlayController.ShowLose();
    }

    // ads 
    private void ShowAdsBanner() { AdsManager.Instance.ShowAdsBanner(); }
    private void ShowAdsInterstitial() { AdsManager.Instance.ShowAdsInterstitial(); }
    private void HideBanner() { AdsManager.Instance.HideAdsBanner(); }

    public enum EGameStatus
    {
        GAME_START,
        GAME_PRE_PLAYING,
        GAME_PLAYING,
        GAME_WIN,
        GAME_LOSE
    }
}
