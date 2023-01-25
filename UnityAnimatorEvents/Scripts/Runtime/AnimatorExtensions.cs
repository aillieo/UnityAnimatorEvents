using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AillieoUtils.UnityAnimatorEvents
{
    public static class AnimatorExtensions
    {
        public static Handle<int> ListenStateEnter(this Animator animator, int stateNameHash, Action callback)
        {
            Assert.IsNotNull(animator);
            Assert.IsNotNull(callback);

            CheckStateAndEventDispatcher(animator, stateNameHash, 0);

            if (!animator.gameObject.TryGetComponent(out AnimatorEventBridge animatorEventBridge))
            {
                animatorEventBridge = animator.gameObject.AddComponent<AnimatorEventBridge>();
            }

            return animatorEventBridge.onStateEnter.AddListener(snh =>
            {
                if (snh == stateNameHash)
                {
                    callback();
                }
            });
        }

        public static Handle<int> ListenStateEnter(this Animator animator, string stateName, Action callback)
        {
            int stateNameHash = Animator.StringToHash(stateName);
            return ListenStateEnter(animator, stateNameHash, callback);
        }

        public static Handle<int> ListenStateExit(this Animator animator, int stateNameHash, Action callback)
        {
            Assert.IsNotNull(animator);
            Assert.IsNotNull(callback);

            CheckStateAndEventDispatcher(animator, stateNameHash, 0);

            if (!animator.gameObject.TryGetComponent(out AnimatorEventBridge animatorEventBridge))
            {
                animatorEventBridge = animator.gameObject.AddComponent<AnimatorEventBridge>();
            }

            return animatorEventBridge.onStateExit.AddListener(snh =>
            {
                if (snh == stateNameHash)
                {
                    callback();
                }
            });
        }

        public static Handle<int> ListenStateExit(this Animator animator, string stateName, Action callback)
        {
            int stateNameHash = Animator.StringToHash(stateName);
            return ListenStateExit(animator, stateNameHash, callback);
        }

        public static Handle<int> ListenStateEnterOnce(this Animator animator, int stateNameHash, Action callback)
        {
            Assert.IsNotNull(animator);
            Assert.IsNotNull(callback);

            CheckStateAndEventDispatcher(animator, stateNameHash, 0);

            if (!animator.gameObject.TryGetComponent(out AnimatorEventBridge animatorEventBridge))
            {
                animatorEventBridge = animator.gameObject.AddComponent<AnimatorEventBridge>();
            }

            return animatorEventBridge.onStateEnter.ListenOnce(snh =>
            {
                if (snh == stateNameHash)
                {
                    callback();
                }
            });
        }

        public static Handle<int> ListenStateEnterOnce(this Animator animator, string stateName, Action callback)
        {
            int stateNameHash = Animator.StringToHash(stateName);
            return ListenStateEnterOnce(animator, stateNameHash, callback);
        }

        public static Handle<int> ListenStateExitOnce(this Animator animator, int stateNameHash, Action callback)
        {
            Assert.IsNotNull(animator);
            Assert.IsNotNull(callback);

            CheckStateAndEventDispatcher(animator, stateNameHash, 0);

            if (!animator.gameObject.TryGetComponent(out AnimatorEventBridge animatorEventBridge))
            {
                animatorEventBridge = animator.gameObject.AddComponent<AnimatorEventBridge>();
            }

            return animatorEventBridge.onStateExit.ListenOnce(snh =>
            {
                if (snh == stateNameHash)
                {
                    callback();
                }
            });
        }

        public static Handle<int> ListenStateExitOnce(this Animator animator, string stateName, Action callback)
        {
            int stateNameHash = Animator.StringToHash(stateName);
            return ListenStateExitOnce(animator, stateNameHash, callback);
        }

        public static void RemoveAllEventListeners(this Animator animator)
        {
            if (animator.gameObject.TryGetComponent(out AnimatorEventBridge animatorEventBridge))
            {
                animatorEventBridge.onStateEnter.RemoveAllListeners();
                animatorEventBridge.onStateExit.RemoveAllListeners();
            }
        }

        public static Handle<int> Play(this Animator animator, string stateName, Action onComplete, int layer = -1, float normalizedTime = float.NegativeInfinity)
        {
            Handle<int> handle = ListenStateExitOnce(animator, stateName, onComplete);
            animator.Play(stateName, layer, normalizedTime);
            return handle;
        }

        public static Handle<int> Play(this Animator animator, int stateNameHash, Action onComplete, int layer = -1, float normalizedTime = float.NegativeInfinity)
        {
            Handle<int> handle = ListenStateExitOnce(animator, stateNameHash, onComplete);
            animator.Play(stateNameHash, layer, normalizedTime);
            return handle;
        }

        public static Handle<int> PlayInFixedTime(this Animator animator, string stateName, Action onComplete, int layer = -1, float fixedTime = float.NegativeInfinity)
        {
            Handle<int> handle = ListenStateExitOnce(animator, stateName, onComplete);
            animator.PlayInFixedTime(stateName, layer, fixedTime);
            return handle;
        }

        public static Handle<int> PlayInFixedTime(this Animator animator, int stateNameHash, Action onComplete, int layer = -1, float fixedTime = float.NegativeInfinity)
        {
            Handle<int> handle = ListenStateExitOnce(animator, stateNameHash, onComplete);
            animator.PlayInFixedTime(stateNameHash, layer, fixedTime);
            return handle;
        }

        public static Handle<int> CrossFade(this Animator animator, int stateNameHash, float normalizedTransitionDuration, Action onComplete, int layer = -1, float normalizedTimeOffset = 0f, float normalizedTransitionTime = 0f)
        {
            Handle<int> handle = ListenStateExitOnce(animator, stateNameHash, onComplete);
            animator.CrossFade(stateNameHash, normalizedTransitionDuration, layer, normalizedTimeOffset, normalizedTransitionTime);
            return handle;
        }

        public static Handle<int> CrossFade(this Animator animator, string stateName, float normalizedTransitionDuration, Action onComplete, int layer = -1, float normalizedTimeOffset = 0f, float normalizedTransitionTime = 0f)
        {
            Handle<int> handle = ListenStateExitOnce(animator, stateName, onComplete);
            animator.CrossFade(stateName, normalizedTransitionDuration, layer, normalizedTimeOffset, normalizedTransitionTime);
            return handle;
        }

        public static Handle<int> CrossFadeInFixedTime(this Animator animator, int stateNameHash, float fixedTransitionDuration, Action onComplete, int layer = -1, float fixedTimeOffset = 0f, float normalizedTransitionTime = 0f)
        {
            Handle<int> handle = ListenStateExitOnce(animator, stateNameHash, onComplete);
            animator.CrossFadeInFixedTime(stateNameHash, fixedTransitionDuration, layer, fixedTimeOffset, normalizedTransitionTime);
            return handle;
        }

        public static Handle<int> CrossFadeInFixedTime(this Animator animator, string stateName, float fixedTransitionDuration, Action onComplete, int layer = -1, float fixedTimeOffset = 0f, float normalizedTransitionTime = 0f)
        {
            Handle<int> handle = ListenStateExitOnce(animator, stateName, onComplete);
            animator.CrossFadeInFixedTime(stateName, fixedTransitionDuration, layer, fixedTimeOffset, normalizedTransitionTime);
            return handle;
        }

        private static void CheckStateAndEventDispatcher(this Animator animator, int stateNameHash, int layer)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (!animator.HasState(layer, stateNameHash))
            {
                Debug.LogError($"No matching state found, so no event will invoke. Animator {animator.name}.", animator);
                return;
            }

            AnimatorEventDispatcher animatorEventDispatcher = animator.GetBehaviour<AnimatorEventDispatcher>();
            if (animatorEventDispatcher == null)
            {
                Debug.LogError($"No {nameof(AnimatorEventDispatcher)} found, so no event will invoke. Animator {animator.name}.", animator);
                return;
            }
#endif
        }
    }
}
