using Game_Base.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game_Base.Character
{
    public class PlayerController : Character
    {
        public void Start()
        {
            DOTween.Sequence().SetDelay(1f).OnComplete(() =>
            {
                base.Move();
            });
        }

        public void PlayerWin() 
        {
            base.Win();
            Gamemanager.Instance.GameWin();
        }
        public void PlayerDie() 
        {
            base.Die();
            Gamemanager.Instance.GameLose();
        }
    }
}

