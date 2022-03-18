using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace Gamee_Hiukka.Data 
{
    public class PetModel : MonoBehaviour
    {
        [SerializeField] float speed = 1;
        [SerializeField] SkeletonAnimation skeletion; 
        [SerializeField, SpineAnimation] string animDie; 
        [SerializeField, SpineAnimation] string animWin; 
        [SerializeField, SpineAnimation] string animRun;
        [SerializeField, SpineAnimation] string animIdle;

        public SkeletonAnimation Skeletion => skeletion;

        public string AnimDie => animDie;
        public string AnimWin => animWin;
        public string AnimRun => animRun;
        public string AnimIdle => animIdle;

        public float Speed => speed;
    }
}

