using Cysharp.Threading.Tasks;
using Gamee_Hiukka.Common;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.Pattern;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Gamee_Hiukka.Control
{
    public class Gamemanager : Singleton<Gamemanager>
    {
        public GamePlayController gamePlayController;
        [SerializeField] CamFollow cameraFollow;
        [SerializeField] Camera cameraUI;
        [SerializeField] GameObject objFollow;
        public Transform levelTransform;

        [SerializeField] EGameState state;
        public EGameLoadData eGameLoadData;

        public EGameState State
        {
            set => state = value;
            get => state; 
        }

        public PlayerController Player { private set; get; }
        public LevelMap LevelMapCurrent => _levelMapCurrent;
        public CamFollow CameraFollow { set => cameraFollow = value; get => cameraFollow; }
        public Camera CameraMain => cameraMain;
        public Camera CameraUI => cameraUI;
        public GamePlayController GamePlayController => gamePlayController;

        [SerializeField] private GameObject effect;
        [SerializeField, Range(0, 5)] private float timeWinDelay = 2f;
        [SerializeField, Range(0, 5)] private float timeLoseDelay = 1f;

        private GameObject _levelLoad;
        private GameObject _levelCurrent;
        private LevelMap _levelMapCurrent;
        private Camera cameraMain;
        float offsetCameraHeight = .75f;
        Sequence seqWin;
        Sequence seqLose;

        private bool _isPreLevelWin = false;

        private void OnEnable()
        {
            if (levelTransform == null)
            {
                GameObject level = new GameObject("_Level");
                levelTransform = level.transform;
            }
            if(objFollow == null) objFollow = new GameObject("Obj Follow");
            cameraMain = cameraFollow.GetComponentInChildren<Camera>();

            if (!GameData.IsStart)
            {
                GameData.IsStart = true;
                Config.TimeAtInterstitialAdShow = DateTime.Now;
                Config.TimeAtInterstitialAdLoseShow = DateTime.Now;
            }
            _isPreLevelWin = true;
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

            BridgeData.CheckLoopLevel();

            if (_levelLoad != null || GameData.LevelCurrentObj == null)
            {
                if (GameData.LevelList == null) DataController.LoadLevelList();
                if (GameData.LevelCollectionList == null) DataController.LoadLevelCollectionList();

                if (Config.IsLevelCollection) _levelLoad = await BridgeData.GetLevelCollection(GameData.LevelCollectionList[GameData.LevelCollectionIndexCurrent]);
                else _levelLoad = await BridgeData.GetLevel(GameData.LevelList[GameData.LevelIndexCurrent]);

                GameData.LevelCurrentObj = _levelLoad;

            }
            else _levelLoad = GameData.LevelCurrentObj;

            //_levelCurrent.SetActive(false);
            eGameLoadData = EGameLoadData.GAME_DATA_READY;
        }

        private void ShowLevelMap()
        {
            if (Config.IsLevelCollection) 
            {
                MyAnalytic.LogEvent(MyAnalytic.LEVEL_START, MyAnalytic.LEVEL_START, "levelCollection_" + GameData.LevelCollectionList[GameData.LevelCollectionIndexCurrent] + "-" + "level_" + GameData.LevelCurrent.ToString());
                var level = "levelCollection_" + GameData.LevelCollectionList[GameData.LevelCollectionIndexCurrent];
                if (!DataController.LoadDataByID(level))
                {
                    MyAnalytic.LogEvent(MyAnalytic.LEVEL_FIRST_START, MyAnalytic.LEVEL_FIRST_START, level);
                    DataController.SaveDataByID(level, true);
                }
            }
            else 
            {
                MyAnalytic.LogEvent(MyAnalytic.LEVEL_START, MyAnalytic.LEVEL_START, "level_" + GameData.LevelList[GameData.LevelIndexCurrent] + "-" + "level_" + GameData.LevelCurrent.ToString());
                var level = "level_" + GameData.LevelList[GameData.LevelIndexCurrent];
                if (!DataController.LoadDataByID(level))
                {
                    MyAnalytic.LogEvent(MyAnalytic.LEVEL_FIRST_START, MyAnalytic.LEVEL_FIRST_START, level);
                    DataController.SaveDataByID(level, true);
                }
            }

            //LogEventLevel();

            gamePlayController.DefautUI();
            gamePlayController.UpdateBG();
            gamePlayController.UIDefaut();
            CameraDefaut();
            PoolManager.ReleaseObjectAll();
            //TutorialManager.Instance.Defaut();


            //StartCoroutine(WaitForShowLevelMap());
            gamePlayController.ShowAdsBanner();

            effect.SetActive(false);

            if (IsShowInter && !Config.IsShowInterAdsBeforeWin)
            {
                state = EGameState.GAME_PAUSE;
                gamePlayController.ShowAdsInterstitial(() =>
                {
                    DOTween.Sequence().SetDelay(.05f).OnComplete(() =>
                    {
                        Config.TimeAtInterstitialAdShow = DateTime.Now;
                        Config.TimeAtInterstitialAdLoseShow = DateTime.Now;
                        state = EGameState.GAME_PLAYING;
                        StartCoroutine(WaitForShowLevelMap());
                    });
                });
            }
            else StartCoroutine(WaitForShowLevelMap());
        }

        private IEnumerator WaitForShowLevelMap()
        {
            yield return new WaitUntil(() => eGameLoadData == EGameLoadData.GAME_DATA_READY);
            state = EGameState.GAME_START;

            if (_levelCurrent != null) DestroyLevelMap();
            _levelCurrent = Instantiate(_levelLoad, levelTransform.transform);
            _levelCurrent.gameObject.SetActive(false);

            yield return new WaitUntil(() => _levelCurrent != null);
            _levelCurrent.gameObject.SetActive(true);
    
            _levelMapCurrent = _levelCurrent.GetComponent<LevelMap>();
            _levelMapCurrent.ActionLevelWin += GameWin;
            _levelMapCurrent.ActionLevelLose += GameLose;
            Player = _levelMapCurrent.Player;
            GamePlay();

            cameraFollow.SetTarget(objFollow);

            //state = EGameState.GAME_PLAYING;
            //_levelCurrent.SetActive(true);
        }
        private void DestroyLevelMap() { Destroy(_levelCurrent); levelTransform.Clear(); _levelCurrent = null; }
        #endregion
        public void GamePlay() 
        {
            state = EGameState.GAME_PLAYING;
        }
        public bool IsShowInter 
        {
            get
            {
                if(GameData.IsShowAds) 
                {
                    GameData.IsShowAds = false;
                    return false;
                }
                if (!AdsManager.Instance.IsInterLoaded) return false;

                bool isShowInter = false;
                if (Config.InterstitialAdFirstShowCount <= GameData.LevelCurrent)
                {
                    if (Config.InterstitialAdCountCurrent > Config.InterstitialAdShowCount)
                    {
                        if (Config.IsShowInterAdsLose)
                        {
                            if (_isPreLevelWin)
                            {
                                if ((DateTime.Now - Config.TimeAtInterstitialAdShow).TotalSeconds > Config.TimeInterAdShow)
                                {
                                    Config.InterstitialAdCountCurrent = 1;
                                    //Config.TimeAtInterstitialAdShow = DateTime.Now;
                                    isShowInter = true;
                                    //gamePlayController.ShowAdsInterstitial();
                                }
                            }
                            else
                            {
                                if ((DateTime.Now - Config.TimeAtInterstitialAdLoseShow).TotalSeconds > Config.TimeInterAdShowLose)
                                {
                                    Config.InterstitialAdCountCurrent = 1;
                                    //Config.TimeAtInterstitialAdLoseShow = DateTime.Now;
                                    isShowInter = true;
                                    //gamePlayController.ShowAdsInterstitial();
                                }
                            }
                        }
                        else
                        {
                            if ((DateTime.Now - Config.TimeAtInterstitialAdShow).TotalSeconds > Config.TimeInterAdShow)
                            {
                                Config.InterstitialAdCountCurrent = 1;
                                Config.TimeAtInterstitialAdShow = DateTime.Now;
                                isShowInter = true;
                                //gamePlayController.ShowAdsInterstitial();
                            }
                        }
                    }
                }
                return isShowInter;
            }
        }
        #region game
        public void GameWin(EWinType type = EWinType.WIN_NORMAL)
        {
            if (state == EGameState.GAME_ENDING || state == EGameState.GAME_LOSE) return;
            _isPreLevelWin = true;

            gamePlayController.UIMove();

            if(Config.IsLevelCollection) MyAnalytic.LogEvent(MyAnalytic.LEVEL_COMPLETE, MyAnalytic.LEVEL_COMPLETE, "levelCollection_" + GameData.LevelCollectionList[GameData.LevelCollectionIndexCurrent].ToString() + "-" + "level_" + GameData.LevelCurrent.ToString());
            else MyAnalytic.LogEvent(MyAnalytic.LEVEL_COMPLETE, MyAnalytic.LEVEL_COMPLETE, "level_" + GameData.LevelList[GameData.LevelIndexCurrent].ToString() + "-" + "level_" + GameData.LevelCurrent.ToString());
            state = EGameState.GAME_ENDING;
            gamePlayController.HideAdsBanner();
            effect.transform.position = Player.transform.position + Vector3.up * 2.5f;
            effect.SetActive(true);

            if (!Config.IsTest)
            {
                if (Config.IsLevelCollection) 
                {
                    GameData.LevelCollectionIndexCurrent++;
                    DataController.SaveLevelCollectionIndexCurrent();
                }
                else 
                {
                    GameData.LevelIndexCurrent++;
                    DataController.SaveLevelIndexCurrent();
                }
                GameData.LevelCurrent++;
                Config.InterstitialAdCountCurrent++;
                DataController.SaveLevelCurrent();
            }

            LoadLevelMap();
            var timeDelay = timeWinDelay;
            seqWin = DOTween.Sequence().SetDelay(timeDelay).OnComplete(() =>
            {
                if (!this.gameObject.activeInHierarchy) return;
                if (!Config.IsShowInterAdsBeforeWin)
                {
                    if (state == EGameState.GAME_LOSE) return;
                    GameData.ProcessCount++;

                    state = EGameState.GAME_WIN;
                    gamePlayController.ShowPopupWin(type);
                }
                else
                {
                    if (IsShowInter)
                    {
                        var stateCache = state;
                        state = EGameState.GAME_PAUSE;
                        gamePlayController.ShowAdsInterstitial(() =>
                        {
                            DOTween.Sequence().SetDelay(.05f).OnComplete(() =>
                            {
                                GameData.ProcessCount++;
                                Config.TimeAtInterstitialAdShow = DateTime.Now;
                                Config.TimeAtInterstitialAdLoseShow = DateTime.Now;
                                state = EGameState.GAME_WIN;
                                gamePlayController.ShowPopupWin(type);
                            });
                        });
                    }
                    else
                    {
                        if (state == EGameState.GAME_LOSE) return;
                        GameData.ProcessCount++;

                        state = EGameState.GAME_WIN;
                        gamePlayController.ShowPopupWin(type);
                    }
                }
            });
        }
        public void GameLose()
        {
            if (state == EGameState.GAME_ENDING) return;
            state = EGameState.GAME_ENDING;
            state = EGameState.GAME_LOSE;

            _isPreLevelWin = false;
            if(Config.IsShowInterAdsLose) Config.InterstitialAdCountCurrent++;

            gamePlayController.UIMove();

            if(Config.IsLevelCollection) MyAnalytic.LogEvent(MyAnalytic.LEVEL_FAILED, MyAnalytic.LEVEL_FAILED, "levelCollection_" + GameData.LevelCollectionList[GameData.LevelCollectionIndexCurrent] + "-" + "level_" + GameData.LevelCurrent.ToString());
            else MyAnalytic.LogEvent(MyAnalytic.LEVEL_FAILED, MyAnalytic.LEVEL_FAILED, "level_" + GameData.LevelList[GameData.LevelIndexCurrent] + "-" + "level_" + GameData.LevelCurrent.ToString());
            gamePlayController.HideAdsBanner();
            LoadLevelMap();

            seqLose = DOTween.Sequence().SetDelay(timeLoseDelay).OnComplete(() =>
            {
                if (state == EGameState.GAME_WIN) return;

                //state = EGameState.GAME_LOSE;
                gamePlayController.ShowPopupLose();
            });
        }

        public void GameNextLevel()
        {
            LoadLevelMap();

            ShowLevelMap();
#if (!UNITY_EDITOR)
            GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
#endif
        }
        public void GameReplayLevel()
        {
            if (state == EGameState.GAME_ENDING) return;
            if (state == EGameState.GAME_PLAYING || state == EGameState.GAME_LOSE)
            {
                if(Config.IsLevelCollection) MyAnalytic.LogEvent(MyAnalytic.LEVEL_REPLAY, MyAnalytic.LEVEL_REPLAY, "levelCollection_" + GameData.LevelCollectionList[GameData.LevelCollectionIndexCurrent] + "-" + "level_" + GameData.LevelCurrent.ToString());
                else MyAnalytic.LogEvent(MyAnalytic.LEVEL_REPLAY, MyAnalytic.LEVEL_REPLAY, "level_" + GameData.LevelList[GameData.LevelIndexCurrent] + "-" + "level_" + GameData.LevelCurrent.ToString());

                gamePlayController.HideAdsBanner();
                ShowLevelMap();
            }
            else if (state == EGameState.GAME_WIN)
            {
                if(Config.IsLevelCollection) MyAnalytic.LogEvent(MyAnalytic.LEVEL_REPLAY, MyAnalytic.LEVEL_REPLAY, "levelCollection_" + GameData.LevelCollectionList[GameData.LevelCollectionIndexCurrent - 1] + "-" + "level_" + (GameData.LevelCurrent - 1).ToString());
                else MyAnalytic.LogEvent(MyAnalytic.LEVEL_REPLAY, MyAnalytic.LEVEL_REPLAY, "level_" + GameData.LevelList[GameData.LevelIndexCurrent - 1] + "-" + "level_" + (GameData.LevelCurrent - 1).ToString());
                if (!Config.IsTest)
                {
                    if (Config.IsLevelCollection) 
                    {
                        GameData.LevelCollectionIndexCurrent--;
                        DataController.SaveLevelCollectionIndexCurrent();
                    }
                    else 
                    {
                        GameData.LevelIndexCurrent--;
                        DataController.SaveLevelIndexCurrent();
                    }

                    GameData.LevelCurrent--;
                    DataController.SaveLevelCurrent();
                }

                LoadLevelMap();
                ShowLevelMap();
            }
        }

        public void GameSkipLevel()
        {
            if (state == EGameState.GAME_ENDING || state == EGameState.GAME_TUTORIAL) return;
            if(Config.IsLevelCollection) MyAnalytic.LogEvent(MyAnalytic.LEVEL_SKIP, MyAnalytic.LEVEL_SKIP, "levelCollection_" + GameData.LevelCollectionList[GameData.LevelCollectionIndexCurrent] + "-" + "level_" + GameData.LevelCurrent.ToString());
            else MyAnalytic.LogEvent(MyAnalytic.LEVEL_SKIP, MyAnalytic.LEVEL_SKIP, "level_" + GameData.LevelList[GameData.LevelIndexCurrent] + "-" + "level_" + GameData.LevelCurrent.ToString());

            if (!Config.IsTest)
            {
                if (Config.IsLevelCollection)
                {
                    GameData.LevelCollectionIndexCurrent++;
                    DataController.SaveLevelCollectionIndexCurrent();
                }
                else
                {
                    GameData.LevelIndexCurrent++;
                    DataController.SaveLevelIndexCurrent();
                }

                GameData.LevelCurrent++;
                Config.InterstitialAdCountCurrent++;
                DataController.SaveLevelCurrent();
            }

            LoadLevelMap();
            ShowLevelMap();
        }

        public void GameSubmit()
        {
            if (state == EGameState.GAME_PLAYING)
            {
                state = EGameState.GAME_ENDING;
            }
        }

        public void GameUndo()
        {
            if (state == EGameState.GAME_PLAYING)
            {
            }
        }
        public void GameKill() 
        {
            if (seqWin != null) seqWin.Kill();
            if (seqLose != null) seqLose.Kill();
        }

        public void CameraRoom(GameObject popup) 
        {
            cameraFollow.StartRoom(objFollow, popup);
        }

        public void CameraDefaut() 
        {
            CameraFollow.Defaut();
        }

        public void UpdateTartgetText(ELevelTargetType type) 
        {
            gamePlayController.UpdateLevelTargetText(type);
        }

        private void OnDisable()
        {
            GameKill();
        }
        #endregion
        #region event
        #endregion
    }
}