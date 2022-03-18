using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Worldreaver.UniUI;
using TMPro;
using Gamee_Hiukka.Common;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.Control;

namespace Gamee_Hiukka.UI 
{
    public class PopupLose : PopupBase
    {
        [SerializeField] private ButtonBase btnReplay;
        [SerializeField] private ButtonBase btnSkip;
        //[SerializeField] private ButtonBase btnNextLevel;
        [SerializeField] private TextMeshProUGUI txtLevel;

        private Action _actionReplay;
        private Action _actionSkip;
        private Action _actionClose;
        private Action _actionNextLevel;
        private Action _actionBackToHome;

        public void Initialize(Action actionClose, Action actionReplay, Action actionSkip, Action actionNextLevel, Action actionBackToHome)
        {
            _actionReplay = actionReplay;
            _actionClose = actionClose;
            _actionSkip = actionSkip;
            _actionNextLevel = actionNextLevel;
            _actionBackToHome = actionBackToHome;

            AudioManager.Instance.PlayAudioGameLose();

            txtLevel.text = string.Format(Constant.LEVEL_TEXT, GameData.LevelCurrent);

            btnReplay.onClick.RemoveListener(ReplayLevel);
            btnReplay.onClick.AddListener(ReplayLevel);

            btnSkip.onClick.RemoveListener(SkipLevel);
            btnSkip.onClick.AddListener(SkipLevel);

            Gamemanager.Instance.CameraRoom(this.gameObject);
        }

        private void SkipLevel() 
        {
            Hide();
            _actionSkip?.Invoke();
        }

        private void ReplayLevel() 
        {
            Hide();
            _actionReplay?.Invoke();
        }

        private void NextLevel()
        {
            _actionNextLevel?.Invoke();
            Hide();
        }

        private void Hide() 
        {
            _actionClose?.Invoke();
        }

        public void BackHome() 
        {
            //Hide();
            _actionBackToHome?.Invoke();
        }
    }
}

