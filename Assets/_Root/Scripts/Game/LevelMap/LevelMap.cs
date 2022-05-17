using Gamee_Hiukka.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.GameUI;
using System;

namespace Gamee_Hiukka.Control
{
    public class LevelMap : MonoBehaviour
    {
        [SerializeField] int targetValue = 0;

        PlayerController player;

        private ELevelTargetType type = ELevelTargetType.LEVEL_KILL_THEM;

        public PlayerController Player => player;
        public ELevelTargetType Type => type;
        public Action<EWinType> ActionLevelWin;
        public Action ActionLevelLose;

        public void Awake()
        {
            player = this.GetComponentInChildren<PlayerController>();
        }

        void GameWin(EWinType winType = EWinType.WIN_NORMAL)
        {
            ActionLevelWin?.Invoke(winType);
        }
        void GameLose()
        {
            ActionLevelLose?.Invoke();
        }

        void OnDrawGizmos()
        {
            float verticalHeightSeen = Camera.main.orthographicSize * 2.0f;
            float verticalWidthSeen = verticalHeightSeen * Camera.main.aspect;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(verticalWidthSeen, verticalHeightSeen, 0));
        }
    }
}

