using Gamee_Hiukka.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoItem : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private Image top;
    [SerializeField] private TextMeshProUGUI txtPlayerRank;
    [SerializeField] private Image imgFlag;
    [SerializeField] private TextMeshProUGUI txtPlayerName;
    [SerializeField] private TextMeshProUGUI txtPlayerLevel;
    [SerializeField] private Sprite imgRank1;
    [SerializeField] private Sprite imgRank2;
    [SerializeField] private Sprite imgRank3;
    [SerializeField] private Sprite imgRankCurrent;
    [SerializeField] private Sprite imgRankNormal;
    [SerializeField] private Sprite imgTop1;
    [SerializeField] private Sprite imgTop2;
    [SerializeField] private Sprite imgTop3;

    public void Init(PlayerInfo playerInfo) 
    {
        top.gameObject.SetActive(false);
        txtPlayerRank.gameObject.SetActive(true);
        txtPlayerRank.text = playerInfo.rank.ToString();
        imgFlag.sprite = playerInfo.flag;
        imgFlag.SetNativeSize();
        txtPlayerName.text = playerInfo.name;
        txtPlayerLevel.text = playerInfo.level.ToString();
        bg.sprite = imgRankNormal;

        if (GameData.UserName.Equals(playerInfo.name)) bg.sprite = imgRankCurrent;

        if (playerInfo.rank.Equals(1))
        {
            top.gameObject.SetActive(true);
            txtPlayerRank.gameObject.SetActive(false);
            top.sprite = imgTop1;
            bg.sprite = imgRank1;
        }
        if (playerInfo.rank.Equals(2))
        {
            top.gameObject.SetActive(true);
            txtPlayerRank.gameObject.SetActive(false);
            top.sprite = imgTop2;
            bg.sprite = imgRank2;
        }
        if (playerInfo.rank.Equals(3))
        {
            top.gameObject.SetActive(true);
            txtPlayerRank.gameObject.SetActive(false);
            top.sprite = imgTop3;
            bg.sprite = imgRank3;
        }
    }
}

public class PlayerInfo
{
    public PlayerInfo() { }
    public PlayerInfo(int rank, Sprite flag, string name, int level) 
    {
        this.rank = rank;
        this.flag = flag;
        this.name = name;
        this.level = level;
    }
    public int rank;
    public Sprite flag;
    public string name;
    public int level;
}
