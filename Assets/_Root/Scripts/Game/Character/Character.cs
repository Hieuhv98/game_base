using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game_Base.Character 
{
    public abstract class Character : MonoBehaviour, ICharacter
    {
        public bool IsMove { get; set; }

        public CharacterModel model;

        public void Move()
        {
            Debug.Log("Move");
        }

        public void Win()
        {
            Debug.Log("Win");
        }

        public void Die()
        {
            Debug.Log("Die");
        }

        [System.Serializable]
        public class CharacterModel
        {
            public ECharacterType eCharacterType;
            public int heath;
            public float speed;
        }

        public enum ECharacterType
        {
            CHARACTER_PLAYER,
            CHARACTER_ENEMY,
        }
    }
}

