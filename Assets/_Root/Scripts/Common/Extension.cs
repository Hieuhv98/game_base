using Cysharp.Threading.Tasks;
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

        public async static void OnCompleted(this UniTask<Spine.TrackEntry> entry, Action actionCompleted)
        {
            var result = await entry;
            result.Complete += _ => { actionCompleted?.Invoke(); };
        }
        public static void OnCompleted(this Spine.TrackEntry entry, Action actionCompleted)
        {
            entry.Complete += _ => { actionCompleted?.Invoke(); };
        }

        public static void Clear(this Transform transform)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
            }
        }
        public static void DoScale(this GameObject go, float scale, float timeScale = 0.25f, float timeBack = 0.15f, Action actionCompleted = null, Ease ease = Ease.InExpo)
        {
            go.transform.DOKill(true);
            Vector3 scaleDeaut = go.transform.localScale;
            go.transform.DOScale(scaleDeaut * scale, timeScale).SetEase(ease).OnComplete(() =>
            {
                go.transform.DOScale(scaleDeaut, timeBack).OnComplete(() => actionCompleted?.Invoke());
            });
        }
        public static void UpdateLine(this LineRenderer line, List<Vector2> points)
        {
            line.positionCount = points.Count;
            for (int i = 0; i < points.Count; i++)
            {
                line.SetPosition(i, points[i]);
            }
        }

        public static float GetAnimationLenght(this Animator animator, string animationName, float speedAnimation = 1)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                if (clip.name.Equals(animationName)) { return clip.length / speedAnimation; }
            }
            return 2f;
        }
    }
}

