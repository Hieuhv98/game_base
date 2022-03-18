
using DG.Tweening;
using Gamee_Hiukka.Control;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.Pattern;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabManager : Singleton<PlayFabManager>
{
    #region login

    [SerializeField] private string titleId = "8873A";
    private const string STATICSTIC_NAME = "HighScore";

    public bool IsLogined { get; private set; }
    public bool IsLoadedWolrd { get; private set; }
    public bool IsLoadedCountry { get; private set; }
    public bool IsSubmitedWorld { get; private set; }
    public bool IsSubmitedCountry{ get; private set; }
    public bool IsUpdate { get; private set; }

    private List<PlayerLeaderboardEntry> listPlayerLeaderBoardWorld;
    private List<PlayerLeaderboardEntry> listPlayerLeaderBoardCountry;

    public void ResetRequest() 
    {
        IsSubmitedWorld = false;
        IsSubmitedCountry = false;
        IsLoadedWolrd = false;
        IsLoadedCountry = false;
    }
    public void ResetRun()
    {
        IsLoadedWolrd = false;
        IsLoadedCountry = false;
        IsLogined = false;
        IsSubmitedWorld = false;
        IsSubmitedCountry = false;
        IsUpdate = true;
    }

    public void RequestLogin()
    {
        ResetRun();
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId)) PlayFabSettings.staticSettings.TitleId = titleId;

        var request = new LoginWithCustomIDRequest { CustomId = GameData.CustomID , CreateAccount = true, };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        // todo
        Debug.Log("login sucess");
        IsLogined = true;

/*        if (string.IsNullOrEmpty(GameData.PlayerID))
        {
            GetPlayerProfile();
        }*/

        GetPlayerProfile();

        void GetPlayerProfile() { PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest(), Successs, _ => { }); }

        void Successs(GetPlayerProfileResult profileResult)
        {
            GameData.PlayerID = profileResult.PlayerProfile.PlayerId;
            DataController.SavePlayerID();
            var displayName = profileResult.PlayerProfile.DisplayName;
            if (string.IsNullOrEmpty(displayName) && !string.IsNullOrEmpty(GameData.UserName))
            {
                IsUpdate = false;
                UpdateUserDisplayName(GameData.UserName, GameData.CountryCode, _ => { DOTween.Sequence().SetDelay(1f).OnComplete(() => IsUpdate = true); }, error => { Debug.Log("update user display error"); }); // update user display name
            }
        }

        void UpdateUserDisplayName(string newName, string countryCode, Action<UpdateUserTitleDisplayNameResult> resultCallback, Action<PlayFabError> errorCallback)
        {
            Debug.Log("update display name success");
            PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = $"{newName}-{countryCode}" }, resultCallback, errorCallback);
        }

        //RequestLeaderboard();
    }

    private void OnLoginFailure(PlayFabError error)
    {
        // todo
        Debug.Log("login fail");
        IsLogined = false;
    }

    #endregion

    #region submit score
    public void SubmitScoreWorld(int playerScore)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = STATICSTIC_NAME,
                Value = playerScore
            }
        }
        }, result => OnStatisticsUpdatedWorld(result), FailureSubmitCallback);
    }

    public void SubmitScoreCountry(int playerScore)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = STATICSTIC_NAME + "-" + GameData.CountryCode,
                Value = playerScore
            }
        }
        }, result => OnStatisticsUpdatedCountry(result), FailureSubmitCallback);
    }

    private void OnStatisticsUpdatedWorld(UpdatePlayerStatisticsResult updateResult)
    {
        IsSubmitedWorld = true;
        Debug.Log("successfully submitted high score world");
    }

    private void OnStatisticsUpdatedCountry(UpdatePlayerStatisticsResult updateResult)
    {
        IsSubmitedCountry = true;
        Debug.Log("successfully submitted high score country");
    }
    private void FailureSubmitCallback(PlayFabError error)
    {
        IsSubmitedWorld = false;
        IsSubmitedCountry = false;
        Debug.LogWarning("something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
    #endregion

    #region get data
    //Get the players with the top 100 high scores in the game
    public void RequestLeaderboardWorld()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = STATICSTIC_NAME,
            StartPosition = 0,
            MaxResultsCount = 100,
        }, result => DisplayLeaderboardWorld(result), FailureRequestCallback);
    }

    public void RequestLeaderboardCountry()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = STATICSTIC_NAME + "-" + GameData.CountryCode,
            StartPosition = 0,
            MaxResultsCount = 100,
        }, result => DisplayLeaderboarCountry(result), FailureRequestCallback);
    }

    private void FailureRequestCallback(PlayFabError error)
    {
        IsLoadedWolrd = false;
        IsLoadedCountry = false;
        Debug.LogWarning("something went wrong with your API call. Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    private void DisplayLeaderboardWorld(GetLeaderboardResult result)
    {
        Debug.Log("got data world");
        listPlayerLeaderBoardWorld = result.Leaderboard;
        IsLoadedWolrd = true;
    }
    private void DisplayLeaderboarCountry(GetLeaderboardResult result)
    {
        Debug.Log("got data country");
        listPlayerLeaderBoardCountry= result.Leaderboard;
        IsLoadedCountry = true;
    }

    public List<PlayerLeaderboardEntry> GetDataPlayerLeaderBoardWorld() 
    {
        return listPlayerLeaderBoardWorld;
    }

    public List<PlayerLeaderboardEntry> GetDataPlayerLeaderBoardCountry()
    {
        return listPlayerLeaderBoardCountry;
    }
    #endregion
}
