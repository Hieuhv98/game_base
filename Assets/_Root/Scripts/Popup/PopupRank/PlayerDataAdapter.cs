using Gamee_Hiukka.Data;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataAdapter : MonoBehaviour
{
    private PlayerInfo myPlayer = new PlayerInfo();
    private List<PlayerLeaderboardEntry> listPlayerLeaderBoardWorld;
    private List<PlayerLeaderboardEntry> listPlayerLeaderBoardCountry;

    private int rankMyPlayerInWorld = 101;
    private int rankMyPlayerInCountry = 101;

    private string displayNameDefaut = "NoName-US";
    public void Init(List<PlayerLeaderboardEntry> listPlayerLeaderBoardWorld, List<PlayerLeaderboardEntry> listPlayerLeaderBoardCountry) 
    {
        this.listPlayerLeaderBoardWorld = listPlayerLeaderBoardWorld;
        this.listPlayerLeaderBoardCountry = listPlayerLeaderBoardCountry;
    }

    public List<PlayerInfo> GetPlayerWord() 
    {
        List<PlayerInfo> listPlayerWorld = new List<PlayerInfo>();
        listPlayerWorld.Clear();

        foreach(var playerData in listPlayerLeaderBoardWorld) 
        {
            var displayName = playerData.DisplayName;

            if (string.IsNullOrEmpty(displayName))
            {
                displayName = displayNameDefaut;
            }

            var stringCut = Util.CutName(displayName, '-');
            Sprite flag = CountryResources.Instance.GetFlag(stringCut[1]);
            listPlayerWorld.Add(new PlayerInfo(playerData.Position + 1, flag, stringCut[0], playerData.StatValue));

            if (GameData.UserName.Equals(stringCut[0])) rankMyPlayerInWorld = playerData.Position + 1;
        }
        return listPlayerWorld;
    }

    public List<PlayerInfo> GetPlayerCountry()
    {
        var position = 0;
        List<PlayerInfo> listPlayer = new List<PlayerInfo>();
        listPlayer.Clear();

        foreach (var playerData in listPlayerLeaderBoardCountry)
        {
            var displayName = playerData.DisplayName;

            if (string.IsNullOrEmpty(displayName))
            {
                displayName = displayNameDefaut;
            }

            var stringCut = Util.CutName(displayName, '-');
            if (stringCut[1].Equals(GameData.CountryCode))
            {
                Sprite flag = CountryResources.Instance.GetFlag(stringCut[1]);
                listPlayer.Add(new PlayerInfo(position + 1, flag, stringCut[0], playerData.StatValue));
                if (GameData.UserName.Equals(stringCut[0])) rankMyPlayerInCountry = position + 1;
                position++;
            }
        }
        return listPlayer;
    }

    public PlayerInfo GetMyPlayer() 
    {
        return myPlayer;
    }

    public int GetRankMyPlayerInWorld() { return rankMyPlayerInWorld; }
    public int GetRankMyPlayerInCountry() { return rankMyPlayerInCountry; }
}
