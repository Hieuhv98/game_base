using System;
using Gamee_Hiukka.Common;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Gamee_Hiukka.Data;

namespace Gamee_Hiukka.Control 
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Gamemanager gamemanager;

        public void Update()
        {
            if (gamemanager.State != EGameState.GAME_PLAYING) return;
        }
    }
}
