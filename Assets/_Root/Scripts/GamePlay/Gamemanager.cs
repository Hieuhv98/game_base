using Cysharp.Threading.Tasks;
using Gamee_Hiukka.Common;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.Pattern;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;

namespace Gamee_Hiukka.Control
{
    public class Gamemanager : Singleton<Gamemanager>
    {
        public InputManager inputManager;
        public GamePlayController gamePlayController;
        [SerializeField] CamFollow cameraFollow;
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

        [SerializeField] private GameObject effect;
        [SerializeField, Range(0, 5)] private float timeWinDelay = 2f;
        [SerializeField, Range(0, 5)] private float timeLoseDelay = 1f;

        private GameObject _levelLoad;
        private GameObject _levelCurrent;
        private LevelMap _levelMapCurrent;

        private bool _isPreLevelWin = false;

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

            if (GameData.LevelIndexCurrent >= Config.LevelMax - GameData.LevelStartLoop + 1)
            {
                if (GameData.LevelStartLoop != Config.LevelStartLoop)
                {
                    GameData.LevelStartLoop = Config.LevelStartLoop;
                    List<int> temp = new List<int>();
                    for (int i = GameData.LevelStartLoop; i <= Config.LevelMax; i++)
                    {
                        temp.Add(i);
                    }
                    GameData.LevelList = temp;
                }

                Util.Shuffle(GameData.LevelList);
                GameData.LevelIndexCurrent = 0;
                DataController.SaveLevelList();
                DataController.SaveLevelIndexCurrent();
            }

            if (_levelLoad != null || GameData.LevelCurrentObj == null)
            {
                _levelLoad = await BridgeData.GetLevel(GameData.LevelList[GameData.LevelIndexCurrent]);
                GameData.LevelCurrentObj = _levelLoad;

            }
            else _levelLoad = GameData.LevelCurrentObj;

            //_levelCurrent.SetActive(false);
            eGameLoadData = EGameLoadData.GAME_DATA_READY;
        }

        private void ShowLevelMap()
        {
            MyAnalytic.LogEvent(MyAnalytic.LEVEL_START, MyAnalytic.LEVEL_START, "level_" + GameData.LevelList[GameData.LevelIndexCurrent] + "-" + "level_" + GameData.LevelCurrent.ToString());
            LogEventLevel();

            gamePlayController.UpdateLevelText();
            gamePlayController.UpdateBG();
            gamePlayController.UIDefaut();
            CameraDefaut();

            if (_levelCurrent != null) DestroyLevelMap();

            StartCoroutine(WaitForShowLevelMap());
            gamePlayController.ShowAdsBanner();

            effect.SetActive(false);

            if (IsShowInter) gamePlayController.ShowAdsInterstitial();
        }

        private IEnumerator WaitForShowLevelMap()
        {
            yield return new WaitUntil(() => eGameLoadData == EGameLoadData.GAME_DATA_READY);
            state = EGameState.GAME_START;

            _levelCurrent = Instantiate(_levelLoad, levelTransform.transform);
            _levelMapCurrent = _levelCurrent.GetComponent<LevelMap>();
            _levelMapCurrent.ActionLevelWin += GameWin;
            _levelMapCurrent.ActionLevelLose += GameLose;
            Player = _levelMapCurrent.Player;
            gamePlayController.UpdateLevelTargetText(_levelMapCurrent.Type);
            
            state = EGameState.GAME_PLAYING;

            //_levelCurrent.SetActive(true);
        }
        private void DestroyLevelMap() { Destroy(_levelCurrent); }
        #endregion

        bool IsShowInter 
        {
            get
            {
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
                                    Config.TimeAtInterstitialAdShow = DateTime.Now;
                                    isShowInter = true;
                                    //gamePlayController.ShowAdsInterstitial();
                                }
                            }
                            else
                            {
                                if ((DateTime.Now - Config.TimeAtInterstitialAdLoseShow).TotalSeconds > Config.TimeInterAdShowLose)
                                {
                                    Config.InterstitialAdCountCurrent = 1;
                                    Config.TimeAtInterstitialAdLoseShow = DateTime.Now;
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
            if (state == EGameState.GAME_ENDING) return;
            _isPreLevelWin = false;
            gamePlayController.UIMove();

            MyAnalytic.LogEvent(MyAnalytic.LEVEL_COMPLETE, MyAnalytic.LEVEL_COMPLETE, "level_" + GameData.LevelList[GameData.LevelIndexCurrent].ToString() + "-" + "level_" + GameData.LevelCurrent.ToString());
            state = EGameState.GAME_ENDING;
            gamePlayController.HideAdsBanner();
            effect.transform.position = Player.transform.position + Vector3.up * 2.5f;
            effect.SetActive(true);
            AudioManager.Instance.PlayAudioPlayerDone();

            if (!Config.IsTest)
            {
                GameData.LevelCurrent++;
                GameData.LevelIndexCurrent++;
                Config.InterstitialAdCountCurrent++;
                DataController.SaveLevelCurrent();
                DataController.SaveLevelIndexCurrent();
            }

            if (GameData.LevelIndexCurrent < Config.LevelMax) LoadLevelMap();

            DOTween.Sequence().SetDelay(timeWinDelay).OnComplete(() =>
            {
                if (state == EGameState.GAME_LOSE) return;
                
                state = EGameState.GAME_WIN;
                gamePlayController.ShowPopupWin(type);
            });
        }
        public void GameLose()
        {
            if (state == EGameState.GAME_ENDING) return;
            _isPreLevelWin = false;
            if(Config.IsShowInterAdsLose) Config.InterstitialAdCountCurrent++;

            gamePlayController.UIMove();

            MyAnalytic.LogEvent(MyAnalytic.LEVEL_FAILED, MyAnalytic.LEVEL_FAILED, "level_" + GameData.LevelList[GameData.LevelIndexCurrent] + "-" + "level_" + GameData.LevelCurrent.ToString());
            state = EGameState.GAME_ENDING;
            gamePlayController.HideAdsBanner();
            LoadLevelMap();

            DOTween.Sequence().SetDelay(timeLoseDelay).OnComplete(() =>
            {
                if (state == EGameState.GAME_WIN) return;

                state = EGameState.GAME_LOSE;
                gamePlayController.ShowPopupLose();
            });
        }

        public void GameNextLevel()
        {
            if (GameData.LevelIndexCurrent >= Config.LevelMax) LoadLevelMap();

            ShowLevelMap();
        }
        public void GameReplayLevel()
        {
            if (state == EGameState.GAME_ENDING) return;
            if (state == EGameState.GAME_PLAYING || state == EGameState.GAME_LOSE)
            {
                MyAnalytic.LogEvent(MyAnalytic.LEVEL_REPLAY, MyAnalytic.LEVEL_REPLAY, "level_" + GameData.LevelList[GameData.LevelIndexCurrent] + "-" + "level_" + GameData.LevelCurrent.ToString());

                gamePlayController.HideAdsBanner();
                ShowLevelMap();
            }
            else if (state == EGameState.GAME_WIN)
            {
                MyAnalytic.LogEvent(MyAnalytic.LEVEL_REPLAY, MyAnalytic.LEVEL_REPLAY, "level_" + GameData.LevelList[GameData.LevelIndexCurrent - 1] + "-" + "level_" + (GameData.LevelCurrent - 1).ToString());
                if (!Config.IsTest)
                {
                    GameData.LevelCurrent--;
                    GameData.LevelIndexCurrent--;
                    DataController.SaveLevelCurrent();
                    DataController.SaveLevelIndexCurrent();
                }

                LoadLevelMap();
                ShowLevelMap();
            }
        }

        public void GameSkipLevel()
        {
            if (state == EGameState.GAME_ENDING) return;
            MyAnalytic.LogEvent(MyAnalytic.LEVEL_SKIP, MyAnalytic.LEVEL_SKIP, "level_" + GameData.LevelList[GameData.LevelIndexCurrent] + "-" + "level_" + GameData.LevelCurrent.ToString());

#if UNITY_EDITOR
            gamePlayController.HideAdsBanner();

            if (!Config.IsTest)
            {
                GameData.LevelCurrent++;
                GameData.LevelIndexCurrent++;
                DataController.SaveLevelCurrent();
                DataController.SaveLevelIndexCurrent();
            }

            LoadLevelMap();
            ShowLevelMap();

#elif UNITY_ANDROID || UNITY_IOS
            gamePlayController.ShowAdsRewared((isWatched) =>
            {
                if (isWatched)
                {
                    gamePlayController.HideAdsBanner();

                    if (!Config.IsTest)
                    {
                        GameData.LevelCurrent++;
                        GameData.LevelIndexCurrent++;
                        DataController.SaveLevelCurrent();
                        DataController.SaveLevelIndexCurrent();
                    }

                    LoadLevelMap();
                    ShowLevelMap();
                }
            });
#endif
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

        public void CameraRoom(GameObject popup) 
        {
            cameraFollow.StartRoom(Player.gameObject, popup);
        }

        public void CameraDefaut() 
        {
            CameraFollow.Defaut();
        }

        #endregion

        #region log adjust event
        void LogEventLevel() 
        {
            switch (GameData.LevelCurrent) 
            {
                case 1:
                    AdjustLog.AdjustLogEventPlayLevel1();
                    break;
                case 2:
                    AdjustLog.AdjustLogEventPlayLevel2();
                    break;
                case 3:
                    AdjustLog.AdjustLogEventPlayLevel3();
                    break;
                case 4:
                    AdjustLog.AdjustLogEventPlayLevel4();
                    break;
                case 5:
                    AdjustLog.AdjustLogEventPlayLevel5();
                    break;
                case 6:
                    AdjustLog.AdjustLogEventPlayLevel6();
                    break;
                case 7:
                    AdjustLog.AdjustLogEventPlayLevel7();
                    break;
                case 8:
                    AdjustLog.AdjustLogEventPlayLevel8();
                    break;
                case 9:
                    AdjustLog.AdjustLogEventPlayLevel9();
                    break;
                case 10:
                    AdjustLog.AdjustLogEventPlayLevel10();
                    break;
                case 15:
                    AdjustLog.AdjustLogEventPlayLevel15();
                    break;
                case 20:
                    AdjustLog.AdjustLogEventPlayLevel20();
                    break;
                case 25:
                    AdjustLog.AdjustLogEventPlayLevel25();
                    break;
                case 30:
                    AdjustLog.AdjustLogEventPlayLevel30();
                    break;
                case 35:
                    AdjustLog.AdjustLogEventPlayLevel35();
                    break;
                case 40:
                    AdjustLog.AdjustLogEventPlayLevel40();
                    break;
                case 45:
                    AdjustLog.AdjustLogEventPlayLevel45();
                    break;
                case 50:
                    AdjustLog.AdjustLogEventPlayLevel50();
                    break;
            }
        }
        #endregion
    }
}