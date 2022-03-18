using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamee_Hiukka.Data 
{
    public class AudioResource : ScriptableObject
    {
        [Header("Common")]
        public Sound audioButtonClick;
        public Sound audioButtonClickStartGame;
        public Sound audioCoinMove;
        [Space, Header("Game Menu")]
        public Sound audioBGGameMenu;
        [Space, Header("Game Play")]
        public Sound audioBGGamePlay;
        public Sound audioGameWin;
        public Sound audioGameLose;
        [Space, Header("Player")]
        public Sound audioPlayerAttack1;
        public Sound audioPlayerAttack2;
        public Sound audioPlayerYawn;
        public Sound audioPlayerDone;
        public Sound audioPlayerDeath;
        public Sound audioPlayerCry;
        public Sound audioPlayerPickKey;
        public Sound audioPlayerOpenChest;
        public Sound audioPlayerSeeTarget;
        public Sound audioPlayerMove;

        [Space, Header("Popy")]
        public Sound audioPopyAttack;
        [Space, Header("Enemy")]
        public Sound audioEnemyMaleDie;
        public Sound audioEnemyFemaleDie;
        public Sound audioEnemyMaleScream;
        public Sound audioEnemyFemaleScream;
        public Sound audioEnemyGunFire;
        public Sound audioEnemyHit;
        [Space, Header("Pin")]
        public Sound audioPinMove;
        public Sound audioPinStuck;
        [Space, Header("Element")]
        public Sound audioBombBreak;
        public Sound audioBombMineBreak;
        public Sound audioTrapUse;
        public Sound audioTrap;
        public Sound audioArrowTrapAttack;
        public Sound audioSawUse;
        public Sound audioCutRope;
        public Sound audioOpenCabinet;
        public Sound audioTeleportIn;
        public Sound audioTeleportOut;
        public Sound audioSwitch;
        public Sound audioBoxFire;
        [Space, Header("Liquid")]
        public Sound audioLiquidCollision;
        public Sound audioIceCollision;
        public Sound audioStoneCollision;
        public Sound audioStoneLiquidCollision;
        public Sound audioBoxCollision;
        public Sound audioWaterIce;
        public Sound audioWaterFire;
        public Sound audioFireMove;
        public Sound audioWaterMove;
        public Sound audioRockMove;
        [Space, Header("Popup")]
        public Sound audioNewSkin;
        public Sound audioUnlockSkin;
        public Sound audioUnlockBox;
        public Sound audioGetEggPiece;
        public Sound audioBuilding;
        public Sound audioBuildCompleted;
        public Sound audioRoomBuildCompleted;
        public Sound audioOpenNewRoom;

        [System.Serializable]
        public class Sound
        {
            public AudioClip audioClip;
            [Range(0, 1)] public float volume = 1;
        }
    }
}

