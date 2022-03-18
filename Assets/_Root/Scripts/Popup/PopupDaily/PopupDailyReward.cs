using System;
using System.Collections.Generic;
using Gamee_Hiukka.Control;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.GameUI;
using Gamee_Hiukka.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Worldreaver.UniUI;

namespace Gamee_Hiukka.Daily
{
    public class PopupDailyReward : PopupBase
    {
        [SerializeField] private TextMeshProUGUI txtCoinWatchVideo;
        [SerializeField] private ButtonBase btnWatchVideo;
        [SerializeField] private ButtonBase btnClaim;
        [SerializeField] private Transform iconCoinDisplay;
        [SerializeField] DailyRewardButton[] rewardButtons;
        public List<WeekRewardData> weekRewardData;
        private DailyRewardButton rewardButtonCurrent;
        private Reward reward;
        WeekRewardData WeekData => weekRewardData[GameData.WeekRewardIndex - 1];
        
        public System.Action<bool> OnUpdate;
        private Action _actionClose;
        
        public void Initialize(Action actionClose)
        {
            _actionClose = actionClose;
            for (int i = 0; i< rewardButtons.Length; i++) 
            {
                rewardButtons[i].Initialize(WeekData.rewards[i]);

                if (WeekData.rewards[i].Active)
                {
                    rewardButtonCurrent = rewardButtons[i];
                    rewardButtons[i].actionUpdateStatus -= UpdateNotifi;
                    rewardButtons[i].actionUpdateStatus += UpdateNotifi;
                    UpdateText(WeekData.rewards[i]);

                    if (rewardButtonCurrent.HasReceiveGift)
                    {
                        rewardButtonCurrent.actionClick += Claim;
                    }
                }
            }
            
            UpdateButton();
        }

        public void WatchVideo()
        {
#if UNITY_EDITOR
            rewardButtonCurrent.OnClaim();
            AddScore(reward.Coin * DataParam.coinX2Value * Config.WatchVideoValue);
#else
            AdsManager.Instance.ShowAdsRewared((isWatched) =>
            {
                if (isWatched)
                {
                    MyAnalytic.LogEvent(MyAnalytic.DAILY_REWARD_CLAIM_BY_ADS);
                    rewardButtonCurrent.OnClaim();
                    AddScore(reward.Coin * DataParam.coinX2Value * Config.WatchVideoValue);
                }
            });
#endif
        }
        public void Claim()
        {
            MyAnalytic.LogEvent(MyAnalytic.DAILY_REWARD_CLAIM);
            rewardButtonCurrent.OnClaim();
            if(reward.IsRewardSkin) ReceiveSkin();
            else
            {
                AddScore(reward.Coin * DataParam.coinX2Value);
            }
        }

        void UpdateText(Reward reward)
        {
            this.reward = reward;
            if (reward.IsRewardSkin)
            {
                btnWatchVideo.gameObject.SetActive(false);
            }
            else
            {
                txtCoinWatchVideo.text = (reward.Coin * Config.WatchVideoValue).ToString();
            }
        }

        void UpdateButton()
        {
            btnWatchVideo.gameObject.SetActive(GameData.HasReward && !reward.IsRewardSkin);
            btnClaim.gameObject.SetActive(GameData.HasReward);
        }
        
        private void AddScore(int score)
        {
            UpdateButton();
            GamePopup.Instance.CoinGeneration.GenerateCoin(null, () =>
            {
                GameData.CoinCurrent += score * DataParam.coinX2Value;
                DataController.SaveCoinCurrent();
            }, rewardButtonCurrent.gameObject, iconCoinDisplay.gameObject);

            
        }
        
        void ReceiveSkin()
        {
            var skindata = SkinResources.Instance.GetSkinByName(rewardButtonCurrent.Reward.SkinName);
            SkinResources.Instance.SetIsHasSkin(skindata.ID);
            GameData.IDSkinCurrent = skindata.ID;
            DataController.SaveIDSkinCurrent();
            GamePopup.Instance.ShowPopupNewSkin(skindata);

            UpdateButton();
        }
        public override void Close()
        {
            if (rewardButtonCurrent != null) rewardButtonCurrent.actionClick -= Claim;
            base.Close();
            _actionClose?.Invoke();
        }
        public void UpdateNotifi()
        {
            OnUpdate?.Invoke(false);
        }
    }
}

