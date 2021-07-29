using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game_Base.Character 
{
    public interface ICharacter
    {
        bool IsMove { set; get; }

        void Move();

        void Win();
        void Die();
    }
}

