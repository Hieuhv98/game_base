using System.Collections;
using System.Collections.Generic;
using Gamee_Hiukka.Control;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.UI;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamee_Hiukka.Daily
{
    public class DailyRewardButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI txtDay;
        [SerializeField] TextMeshProUGUI txtCoin;
        [SerializeField] Image imgIcon;
        [SerializeField] GameObject active;
        [SerializeField] GameObject coinGroup;
        [SerializeField] GameObject block;
        [SerializeField] GameObject skinGroup;
        [SerializeField] GameObject hero;
        [SerializeField] private SkeletonGraphic skePlayer;
        [SerializeField] GameObject check;
        [SerializeField] ButtonBase btnClick;

        Reward reward;

        public bool HasReceiveGift => reward != null ? GameData.HasReward && reward.Active : false;
        public bool Received { get => GameData.LoadValue(reward.DayNow.ToString()); set => GameData.SaveValue(reward.DayNow.ToString(), value); }
        public System.Action actionUpdateStatus;
        public System.Action actionClick;
        public Reward Reward => reward;
        public void Initialize(Reward reward)
        {
            this.reward = reward;
            OnUpdate();
        }

        public void OnUpdate()
        {
            active.SetActive(HasReceiveGift);
            check.SetActive(Received);
            btnClick.interactable = HasReceiveGift;
            txtDay.text = "Day " + reward.Day;
            block.SetActive(Received);

            if (!reward.IsRewardSkin)
            {
                skinGroup.SetActive(false);
                txtCoin.text = "+ " + reward.Coin;
                if(reward.Icon != null) imgIcon.sprite = reward.Icon;
            }
            else
            {
                //hero.SetActive(!Received);
                coinGroup.SetActive(false);
                Util.UpdateSkin(skePlayer, reward.SkinName);
            }
        }

        public void OnClaim()
        {
            GameData.HasReward = false;
            Received = true;
            actionUpdateStatus?.Invoke();
            OnUpdate();
            AdjustLog.AdjustLogEventDailyReward();
        }

        public void OnClick() 
        {
            actionClick?.Invoke();
        }
    }
}

