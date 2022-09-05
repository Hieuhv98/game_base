using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamee_Hiukka.Common 
{
    public enum EGameState
    {
        GAME_START,
        GAME_PLAYING,
        GAME_ENDING,
        GAME_WATTING,
        GAME_WIN,
        GAME_LOSE,
        GAME_PAUSE,
        GAME_TUTORIAL
    }

    public enum EGameLoadData
    {
        GAME_DATA_LOADING,
        GAME_DATA_READY,
    }

    public enum EPlayerState
    {
        PLAYER_IDLE,
        PLAYER_RUN,
        PLAYER_FLY,
        PLAYER_ATTACK,
        PLAYER_DIE,
        PLAYER_PIKING,
        PLAYER_VICTORY,
        PLAYER_JUMP
    }

    public enum EPopyState
    {
        POPY_IDLE,
        POPY_SLEEP,
        POPY_FLY,
        POPY_ATTACK,
        POPY_VICTORY
        
    }

    public enum EEnemyState 
    {
        ENEMY_IDLE,
        ENEMY_AFRAID,
        ENEMY_DIE,
    }

    public enum EEnemyType 
    {
        NORMAL,
        GUN,
        HIT,
    }
    public enum EEnemyGender
    {
        MALE,
        FEMALE,
    }
    public enum EETeleportDirection 
    {
        DIRECTION_UP,
        DIRECTION_DOWN,
        DIRECTION_RIGHT,
        DIRECTION_LEFT,
    }
    public enum EStickMoveType 
    {
        SICK_NONE,
        STICK_RIGHT,
        STICK_LEFT
    }

    public enum EWinType 
    {
        WIN_NORMAL,
        WIN_CHEST,
    }
    public enum EPetState
    {
        PET_IDLE,
        PET_WIN,
        PET_DIE,
        PET_RUN,
    }

    public enum ELevelTargetType 
    {
        LEVEL_KILL_THEM,
        LEVEL_OPEN_THE_BOX,
    }

    public enum EMoveType
    {
        MOVE_UP,
        MOVE_DOWN,
        MOVE_LEFT,
        MOVE_RIGHT
    }
}

