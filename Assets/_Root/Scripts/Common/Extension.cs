using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamee_Hiukka.Common 
{
    public static class Extension
    {
        public static void OnCompleted(this AnimationClip animationClip, Action actionCompleted)
        {
            float time = animationClip != null ? animationClip.length : 1.5f;
            DOTween.Sequence().SetDelay(time).OnComplete(() =>
            {
                actionCompleted?.Invoke();
            });
        }

        public static void ChangeSkin(this SkeletonGraphic skeleton, string nameSkin)
        {
            var ske = skeleton.Skeleton;
            var newSkin = new Spine.Skin("new_skin");
            newSkin.AddSkin(ske.Data.FindSkin(nameSkin));
            ske.SetSkin(newSkin);
            ske.SetSlotsToSetupPose();
            skeleton.AnimationState.Apply(ske);
            skeleton.LateUpdate();
        }

        public static void OnCompleted(this Spine.TrackEntry entry, Action actionCompleted) 
        {
            entry.Complete += _ => { actionCompleted?.Invoke(); };
        }

        public static void Clear(this Transform transform) 
        {
            for(int  i = 0; i< transform.childCount; i++)
            {
                UnityEngine.Object.Destroy(transform.GetChild(i));
            }
        }
    }
}

