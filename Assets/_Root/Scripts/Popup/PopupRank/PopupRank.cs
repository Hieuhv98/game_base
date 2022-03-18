using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Worldreaver.UniUI;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.UI;

public class PopupRank : PopupBase
{
    [SerializeField] private ButtonBase btnWorld;
    [SerializeField] private ButtonBase btnCountry;
    [SerializeField] private Sprite imgActiveTab;
    [SerializeField] private Sprite imgNotActiveTab;
    [SerializeField] private List<PlayerInfoItem> playersInfo;
    [SerializeField] private UniButton btnBackPage;
    [SerializeField] private TextMeshProUGUI txtPageCurrent;
    [SerializeField] private UniButton btnNextPage;
    [SerializeField] private TextMeshProUGUI txtPlayerName;
    [SerializeField] private TextMeshProUGUI txtPlayerRank;
    [SerializeField] private PlayerDataAdapter adapter;
    [SerializeField] private GameObject fetch;
    [SerializeField] private TextMeshProUGUI txtNoti;

    private int pageWorldCurrent = 1;
    private int pageCountryCurrent = 1;

    private bool isWorld = true;
    private List<PlayerInfo> listPlayerWorld = new List<PlayerInfo>();
    private List<PlayerInfo> listPlayerCountry = new List<PlayerInfo>();
    private PlayFabManager playFabManager;
    private float timeWaitLogin = 10f;
    private float timeWaitLoad = 15f;
    private bool isRequesting = false;

    private int rankMyPlayerInWorld = 0;
    private int rankMyPlayerInCountry = 0;

    private int pageWorldMax = 0;
    private int pageCountryMax = 0;
    public void Initialize()
    {
        btnWorld.onClick.RemoveListener(ButtonWorldPressed);
        btnWorld.onClick.AddListener(ButtonWorldPressed);

        btnCountry.onClick.RemoveListener(ButtonCountryPressed);
        btnCountry.onClick.AddListener(ButtonCountryPressed);

        btnBackPage.onClick.RemoveListener(BackPage);
        btnBackPage.onClick.AddListener(BackPage);

        btnNextPage.onClick.RemoveListener(NextPage);
        btnNextPage.onClick.AddListener(NextPage);

        Fetch();
    }

    public void Update()
    {
    }

    // fetch
    private void Fetch()
    {
        txtPlayerName.text = GameData.UserName;

        var seqLogin = DOTween.Sequence();
        var seqLoad = DOTween.Sequence();

        seqLogin.Append(DOTween.Sequence().SetDelay(timeWaitLogin));
        seqLoad.Append(DOTween.Sequence().SetDelay(timeWaitLoad));

        playFabManager = PlayFabManager.Instance;

        if (!playFabManager.IsLogined)
        {
            foreach(var player in playersInfo) { player.gameObject.SetActive(false); }
            txtNoti.text = "Connecting...";
            fetch.SetActive(true);
            btnCountry.interactable = false;
            btnWorld.interactable = false;
            Login();
        }

        seqLogin.Play();
        seqLogin.OnUpdate(() =>
        {
            if (playFabManager.IsLogined && playFabManager.IsUpdate) 
            {
                seqLogin.Kill();

                Submit();
                seqLoad.Play();
            }
        });

        seqLoad.OnUpdate(() =>
        {
            if(playFabManager.IsSubmitedWorld && playFabManager.IsSubmitedCountry) 
            {
                if (!isRequesting) 
                {
                    isRequesting = true;
                    DOTween.Sequence().SetDelay(0.5f).OnComplete(() => 
                    {
                        playFabManager.RequestLeaderboardWorld();
                        playFabManager.RequestLeaderboardCountry();
                    });
                }

                if (playFabManager.IsLoadedWolrd && playFabManager.IsLoadedCountry)
                {
                    seqLoad.Kill();

                    var listPlayerWorld = playFabManager.GetDataPlayerLeaderBoardWorld();
                    var listPlayerCountry = playFabManager.GetDataPlayerLeaderBoardCountry();
                    adapter.Init(listPlayerWorld, listPlayerCountry);
                    LoadData();

                    DOTween.Sequence().SetDelay(.15f).OnComplete(() =>
                    {
                        pageWorldCurrent = UpdatePageCurrent(rankMyPlayerInWorld);
                        pageCountryCurrent = UpdatePageCurrent(rankMyPlayerInCountry);

                        pageCountryMax = (int)((listPlayerCountry.Count - 1) / 10) + 1;
                        pageWorldMax = (int)((listPlayerWorld.Count - 1) / 10) + 1;

                        UpdateDisplay();
                        playFabManager.ResetRequest();
                        isRequesting = false;
                        btnCountry.interactable = true;
                        btnWorld.interactable = true;
                        fetch.SetActive(false);
                    });
                }
            }
        });

        seqLogin.OnComplete(() => 
        {
            txtNoti.text = "Check Internet!";
        });

        seqLoad.OnComplete(() =>
        {
            txtNoti.text = "Not Get Data!";
        });
    }

    // login
    private void Login() 
    {
        PlayFabManager.Instance.RequestLogin();
    }
    // submit 
    private void Submit() 
    {
        PlayFabManager.Instance.SubmitScoreWorld(GameData.LevelCurrent);
        PlayFabManager.Instance.SubmitScoreCountry(GameData.LevelCurrent);
    }

    // load data
    private void LoadData() 
    {
        listPlayerWorld = adapter.GetPlayerWord();
        listPlayerCountry = adapter.GetPlayerCountry();
        rankMyPlayerInWorld = adapter.GetRankMyPlayerInWorld();
        rankMyPlayerInCountry = adapter.GetRankMyPlayerInCountry();
    }
    // todo

    private int UpdatePageCurrent(int rank) 
    {
        if (rank > 100) return 1;

        return (rank -1) / 10 + 1;
    }
    private void UpdateDisplay() 
    {

        if (isWorld) 
        {
            UpdatePlayerInfoInWorld(pageWorldCurrent);
        }
        else 
        {
            UpdatePlayerInfoInCountry(pageCountryCurrent);
        }
    }
    private void ButtonWorldPressed() 
    {
        isWorld = true;
        UpdatePlayerInfoInWorld(pageWorldCurrent);
        TabActive(btnWorld.gameObject);
        TabNotActive(btnCountry.gameObject);

    }
    private void ButtonCountryPressed() 
    {
        isWorld = false;
        UpdatePlayerInfoInCountry(pageCountryCurrent);
        TabActive(btnCountry.gameObject);
        TabNotActive(btnWorld.gameObject);
    }

    private void UpdatePlayerInfoInWorld(int pageCurrent) 
    {
        UpdatePlayeInfo(pageCurrent, listPlayerWorld);
        UpdateMyPlayerRank(rankMyPlayerInWorld);
        UpdateButtonPage(pageCurrent, pageWorldMax);
    }
    private void UpdatePlayerInfoInCountry(int pageCurrent)
    {
        UpdatePlayeInfo(pageCurrent, listPlayerCountry);
        UpdateMyPlayerRank(rankMyPlayerInCountry);
        UpdateButtonPage(pageCurrent, pageCountryMax);
    }

    private void UpdatePlayeInfo(int pageCurrent, List<PlayerInfo> listPlayer)
    {
        txtPageCurrent.text = pageCurrent.ToString();
        for (int i = 0; i < 10; i++)
        {
            var index = i + (pageCurrent - 1) * 10;
            if (index < listPlayer.Count)
            {
                playersInfo[i].Init(listPlayer[index]);

                if (!playersInfo[i].gameObject.activeSelf) 
                {
                    playersInfo[i].gameObject.SetActive(true);
                }
            }
            else
            {
                playersInfo[i].gameObject.SetActive(false);
            }
        }
    }
    private void UpdateMyPlayerRank(int rank) 
    {
        if(rank <= 100) 
        {
            txtPlayerRank.text = "Rank:" + rank.ToString();
        }
        else 
        {
            txtPlayerRank.text = "Rank: 100+";
        }
    }

    private void NextPage() 
    {
        if (isWorld) 
        {
            if (pageWorldCurrent < 10)
            {
                pageWorldCurrent++;
                txtPageCurrent.text = pageWorldCurrent.ToString();
                UpdatePlayerInfoInWorld(pageWorldCurrent);
            }
        }
        else 
        {
            if (pageCountryCurrent < 10)
            {
                pageCountryCurrent++;
                txtPageCurrent.text = pageCountryCurrent.ToString();
                UpdatePlayerInfoInCountry(pageCountryCurrent);
            }
        }
    }
    private void BackPage() 
    {
        if (isWorld)
        {
            if (pageWorldCurrent > 1)
            {
                pageWorldCurrent--;
                txtPageCurrent.text = pageWorldCurrent.ToString();
                UpdatePlayerInfoInWorld(pageWorldCurrent);
            }
        }
        else
        {
            if (pageCountryCurrent > 1)
            {
                pageCountryCurrent--;
                txtPageCurrent.text = pageCountryCurrent.ToString();
                UpdatePlayerInfoInCountry(pageCountryCurrent);
            }
        }
    }

    private void UpdateButtonPage(int page, int pageMax) 
    {
        btnNextPage.gameObject.SetActive(true);
        btnBackPage.gameObject.SetActive(true);

        if (page == 1) btnBackPage.gameObject.SetActive(false);
        if (page == pageMax) btnNextPage.gameObject.SetActive(false);
    }

    private void TabActive(GameObject tab) 
    {
        tab.GetComponent<Image>().sprite = imgActiveTab;
    }

    private void TabNotActive(GameObject tab)
    {
        tab.GetComponent<Image>().sprite = imgNotActiveTab;
    }
}
