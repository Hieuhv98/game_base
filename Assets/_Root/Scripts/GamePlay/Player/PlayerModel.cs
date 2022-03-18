using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamee_Hiukka.Data 
{
    public class PlayerModel : ScriptableObject
    {
        [SerializeField] float hp = 1;
        [SerializeField] float speed = 10;
        [SerializeField] float dame = 1;
        [SerializeField] float attackRange = 1;
        [SerializeField] float jumpForce = 5;

        public float HP => hp;
        public float Speed => speed;
        public float Dame => dame;
        public float AttackRange => attackRange;
        public float JumpForce => jumpForce;
    }
}

