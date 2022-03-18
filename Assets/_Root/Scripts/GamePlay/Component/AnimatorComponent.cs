using Gamee_Hiukka.Common;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Gamee_Hiukka.Component 
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorComponent : MonoBehaviour
    {
        Animator animator;
        AnimationClip animatonClip;
        [SerializeField] bool isPlaying;
        public bool IsPlaying => isPlaying;

        string animName;

        public void Start()
        {
            animator = GetComponent<Animator>();
            //UpdateAnimation(Constant.ANIM_IDLE);
        }

        public AnimationClip PlayAnimation(string animName, int layer = -1, float normalizedTime = 0.25f)
        {
            this.animName = animName;
            //animator.Play(animName, layer, normalizedTime);
            
            animator.IsInTransition(layer);
            animator.CrossFadeInFixedTime(animName, normalizedTime, layer);

            animatonClip = animator.runtimeAnimatorController.animationClips.FirstOrDefault(_ => _.name == animName);
            return animatonClip;
        }

        public void RegisterEvent(Action actionEvent = null) 
        {
            if (animatonClip == null)
            {
                StartCoroutine(WaitTimeEvent(0.5f, actionEvent));
                return;
            }
            AnimationEvent animationEvent = animatonClip.events[0];

            if (animationEvent == null) return;
            StartCoroutine(WaitTimeEvent(animationEvent.time, actionEvent));
        }

        public void StopAnimation() 
        {
            PlayAnimation(Constant.ANIM_EMPTY);
        }
        
        public void SetBoolParameter(string par, bool value) 
        {
            animator.SetBool(par, value);
        }

        public void SetTriggerParameter(string par)
        {
            animator.SetTrigger(par);
        }
        IEnumerator WaitTimeEvent(float time, Action actionEvent) 
        {
            yield return new WaitForSeconds(time);
            actionEvent?.Invoke();
        }
    }
}

