using DG.Tweening;
using Gamee_Hiukka.Common;
using Gamee_Hiukka.Component;
using Gamee_Hiukka.Data;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gamee_Hiukka.Control 
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] EPlayerState state;
        [SerializeField] GameObject model;

        SkeletonMecanim skePlayer;
        private void Awake()
        {
            skePlayer = model.GetComponent<SkeletonMecanim>();
        }
        void Start()
        {
            UpdateSkinCurrent();
        }

        public void UpdateSkinCurrent() 
        {
            var skin = SkinResources.Instance.GetSkinCurrent().SkinName;

            skePlayer.Skeleton.SetSkin(skin);
            skePlayer.Skeleton.SetSlotsToSetupPose();
        }
    }
}

