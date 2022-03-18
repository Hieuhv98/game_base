using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamee_Hiukka.Common 
{
    public static class Constant
    {
        public const string LEVEL = "Level_{0}";
        public const string LEVEL_BONUS = "LevelBonus_{0}";
        public const string LEVEL_TEXT = "LEVEL {0}";
        public const string LEVEL_BONUS_TEXT = "LEVEL BONUS";
        public const string LOADING= "Loading {0}%";
        public const string CONFIG_PATH = "Config";
        public const string UI_CONFIG_PATH = "UIConfig";
        public const string PET_DATA_PATH = "Petdata";
        public const string PET_COLLECTION_PATH = "PetCollection";
        public const string EFFECT_RESOURCE_PATH = "EffectResource";
        public const string SKIN_RESOURCE_PATH = "SkinResource";

        #region layer name
        public const string LAYER_TARGET = "Target";
        public const string LAYER_GROUND = "Ground";
        public const string LAYER_STONE = "Stone";
        public const string LAYER_ICE_BLOCK = "IceBlock";
        public const string LAYER_BULLET = "Bullet";
        public const string LAYER_PLAYER= "Player";
        public const string LAYER_PET= "Pet";
        public const string LAYER_DIE = "Die";
        #endregion

        #region tag name
        public const string TAG_STICK = "Stick";
        public const string TAG_PLAYER = "Player";
        public const string TAG_ENEMY = "Enemy";
        public const string TAG_FIRE = "Fire";
        public const string TAG_WATER = "Water";
        public const string TAG_POISON = "Poison";
        public const string TAG_ICE = "IceLiquid";
        public const string TAG_ICE_BLOCK = "IceBlock";
        public const string TAG_STONE = "Stone";
        public const string TAG_STONE_LIQUID = "StoneLiquid";
        public const string TAG_BOX = "Box";
        public const string TAG_KEY = "Key";
        public const string TAG_CHEST = "Chest";
        public const string TAG_ROPE = "Rope";
        public const string TAG_BULLET = "Bullet";
        public const string TAG_GROUND_JUMP = "Ground_Jump";
        #endregion

        #region anim name
        public const string ANIM_IDLE = "Idle";
        public const string ANIM_RUN = "Run";
        public const string ANIM_FLY = "Fly";
        public const string ANIM_DIE = "Die";
        public const string ANIM_YAWN = "Yawn";
        public const string ANIM_SEE = "See";
        public const string ANIM_SLEEP = "Sleep";
        public const string ANIM_PICK = "Pick";
        public const string ANIM_DIE_ICE = "Die_Ice";
        public const string ANIM_DIE_FIRE = "Die_Fire";
        public const string ANIM_ATTACK = "Attack";
        public const string ANIM_VICTORY = "Victory";
        public const string ANIM_TAKE_DAME = "Take_Dame";
        public const string ANIM_AFRAID = "Afraid";
        public const string ANIM_OPEN = "Open";
        public const string ANIM_CRY = "Cry";
        public const string ANIM_EMPTY = "Empty";
        #endregion
        #region parameter name
        public const string PARAMETER_IS_YAWN = "IsYawn";
        public const string PARAMETER_IS_SEE = "IsSee";
        public const string PARAMETER_IS_IDLE = "IsIdle";
        public const string PARAMETER_IS_WIN = "IsWin";

        #endregion

        #region config pin
        public const float DELTAL_STICK_HEAD = 0.215f; // f
        public const float DEAULT_STICK_A = 1.321f; // m

        public const float DELTA_MOVEMENT_STICK_HEAD = 0.5f;

        public const float DELTA_DYNAMIC_STICK_HEAD2 = 0.205f;
        public const float DELTA_DYNAMIC_STICK_HEAD = 0.205f;
        public const float DELTA_DYNAMIC_STICK = -0.544f;
        #endregion
    }
}

