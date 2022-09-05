using Gamee_Hiukka.Common;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Spine.Unity;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Gamee_Hiukka.Component
{
    [RequireComponent(typeof(SkeletonAnimation))]
    public class AnimatorComponent : MonoBehaviour
    {
        [SerializeField] bool isPlaying;

        string animName;
        SkeletonAnimation skeletonAnimation;
        public bool IsPlaying => isPlaying;
        public string AnimName => animName;

        private Dictionary<string, Action> _cacheEvent = new Dictionary<string, Action>();

        public void Awake()
        {
            skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
            skeletonAnimation.AnimationState.Event += HandleAnimationStateEvent;
        }

        public void Initialize(bool reload = false)
        {
            skeletonAnimation.Initialize(reload);
        }
        public void AddAnimation(string animName, int index = 2)
        {
            skeletonAnimation.AnimationState.AddAnimation(index, animName, false, 0);
        }
        public async UniTask<Spine.TrackEntry> PlayAnimation(string animName, int index = 1, bool loop = false, float speed = 1f)
        {
            this.animName = animName;
            await UniTask.WaitUntil(() => skeletonAnimation.AnimationState != null);
            skeletonAnimation.timeScale = speed;
            return skeletonAnimation.AnimationState.SetAnimation(index, animName, loop);
        }

        public void RegisterEvent(string eventName, Action actionEvent = null)
        {
            if (_cacheEvent.ContainsKey(eventName))
            {
                _cacheEvent[eventName] = actionEvent;
            }
            else
            {
                _cacheEvent.Add(eventName, actionEvent);
            }
        }

        private void HandleAnimationStateEvent(Spine.TrackEntry trackEntry, Spine.Event e)
        {
            foreach (var item in _cacheEvent.ToList())
            {
                if (item.Key.Equals(e.Data.Name))
                {
                    item.Value?.Invoke();
                }
            }
        }

        public void StopAnimation()
        {
        }

        IEnumerator WaitTimeEvent(float time, Action actionEvent)
        {
            yield return new WaitForSeconds(time);
            actionEvent?.Invoke();
        }
    }
}

