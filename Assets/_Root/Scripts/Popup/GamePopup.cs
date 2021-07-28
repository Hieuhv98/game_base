using Game_Base.Pattern;
using Game_Base.UI;
using System;
using UnityEngine;
using Worldreaver.UniUI;

namespace Game_Base.GameUI 
{
    public class GamePopup : Singleton<GamePopup>
    {
        [SerializeField] private Canvas canvas;
        private UniPopup _uniPopup;
        public UniPopup Popup => _uniPopup ?? (_uniPopup = new UniPopup());

        [SerializeField] private PopupWin popupWinPrefab;
        [SerializeField] private PopupLose popupLosePrefab;


        public IUniPopupHandler popupWinHandler;
        public IUniPopupHandler popupLoseHandler;


        public void ShowPopupWin(Action actionNextLevel)
        {
            if (popupWinHandler != null)
            {
                if (popupWinHandler.ThisGameObject.activeSelf) return;

                Display();
                return;
            }

            popupWinHandler = Instantiate(popupWinPrefab, canvas.transform, false);
            Display();

            void Display()
            {
                // initialize
                var popup = (PopupWin)popupWinHandler;
                Popup.Show(popupWinHandler);
                popup.Initialize(actionNextLevel);
            }
        }

        public void ShowPopupLose(Action actionReplay)
        {
            if (popupLoseHandler != null)
            {
                if (popupLoseHandler.ThisGameObject.activeSelf) return;

                Display();
                return;
            }

            popupLoseHandler = Instantiate(popupLosePrefab, canvas.transform, false);
            Display();

            void Display()
            {
                var popup = (PopupLose)popupLoseHandler;
                popup.Initialize(actionReplay);
                Popup.Show(popupLoseHandler);
            }
        }
    }
}

