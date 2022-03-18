using Gamee_Hiukka.Common;
using Gamee_Hiukka.Control;
using Gamee_Hiukka.Data;
using System.Collections;
using System.Collections.Generic;
using Game_Base.Control;
using TMPro;
using UnityEngine;

namespace Gamee_Hiukka.UI 
{
    public class GamePlayUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtLevel;
        [SerializeField] private TextMeshProUGUI txtLevelTarget;
        [SerializeField] private BGController bgControl;
        [SerializeField] private GameObject petNoti;
        [SerializeField] private string killThemText = "KILL THEM ALL";
        [SerializeField] private string openTheBoxText = "OPEN THE BOX";
        [SerializeField] private List<ObjMoveUI> objMoves;

        public void Initialize() 
        {
            txtLevel.text = string.Format(Constant.LEVEL_TEXT, GameData.LevelCurrent);
        }

        public void UpdateLevelText()
        {
            txtLevel.text = string.Format(Constant.LEVEL_TEXT, GameData.LevelCurrent);
        }
        public void UpdateLevelTargetText(ELevelTargetType type)
        {
            switch (type) 
            {
                case ELevelTargetType.LEVEL_KILL_THEM:
                    txtLevelTarget.text = killThemText;
                    break;
                case ELevelTargetType.LEVEL_OPEN_THE_BOX:
                    txtLevelTarget.text = openTheBoxText;
                    break;
            }
        }

        public void UpdateBG() 
        {
            bgControl.UpdateBG();
        }

        public void UIMove()
        {
            foreach (var obj in objMoves)
            {
                obj.Move();
            }
        }

        public void UIDefaut()
        {
            foreach (var obj in objMoves)
            {
                obj.Defaut();
            }
        }
    }
}

