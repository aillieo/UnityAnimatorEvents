// -----------------------------------------------------------------------
// <copyright file="AnimatorExtensions.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.UnityAnimatorEvents
{
    using System;
    using UnityEngine;
    using UnityEngine.Assertions;

    /// <summary>
    /// Extension methods for <see cref="Animator"/>.
    /// </summary>
    public static class AnimatorExtensions
    {
        /// <summary>
        /// Register a callback to the event when the animator enters a state.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateNameHash">The hash of the state.</param>
        /// <param name="callback">The callback function.</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle ListenStateEnter(this Animator animator, int stateNameHash, Action<AnimatorStateInfo> callback)
        {
            Assert.IsNotNull(animator);
            Assert.IsNotNull(callback);

            CheckStateAndEventDispatcher(animator, stateNameHash, 0);

            if (!animator.gameObject.TryGetComponent(out AnimatorEventBridge animatorEventBridge))
            {
                animatorEventBridge = animator.gameObject.AddComponent<AnimatorEventBridge>();
            }

            return animatorEventBridge.onStateEnter.AddListener(asi =>
            {
                if (asi.shortNameHash == stateNameHash)
                {
                    callback(asi);
                }
            });
        }

        /// <summary>
        /// Register a callback to the event when the animator enters a state.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateName">The name of the state.</param>
        /// <param name="callback">The callback function.</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle ListenStateEnter(this Animator animator, string stateName, Action<AnimatorStateInfo> callback)
        {
            var stateNameHash = Animator.StringToHash(stateName);
            return ListenStateEnter(animator, stateNameHash, callback);
        }

        /// <summary>
        /// Register a callback to the event when the animator exits a state.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateNameHash">The hash of the state.</param>
        /// <param name="callback">The callback function.</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle ListenStateExit(this Animator animator, int stateNameHash, Action<AnimatorStateInfo> callback)
        {
            Assert.IsNotNull(animator);
            Assert.IsNotNull(callback);

            CheckStateAndEventDispatcher(animator, stateNameHash, 0);

            if (!animator.gameObject.TryGetComponent(out AnimatorEventBridge animatorEventBridge))
            {
                animatorEventBridge = animator.gameObject.AddComponent<AnimatorEventBridge>();
            }

            return animatorEventBridge.onStateExit.AddListener(asi =>
            {
                if (asi.shortNameHash == stateNameHash)
                {
                    callback(asi);
                }
            });
        }

        /// <summary>
        /// Register a callback to the event when the animator exits a state.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateName">The name of the state.</param>
        /// <param name="callback">The callback function.</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle ListenStateExit(this Animator animator, string stateName, Action<AnimatorStateInfo> callback)
        {
            var stateNameHash = Animator.StringToHash(stateName);
            return ListenStateExit(animator, stateNameHash, callback);
        }

        /// <summary>
        /// Register a callback to the event when the animator enters a state and invoke only once.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateNameHash">The hash of the state.</param>
        /// <param name="callback">The callback function.</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle ListenStateEnterOnce(this Animator animator, int stateNameHash, Action<AnimatorStateInfo> callback)
        {
            EventHandle handle = default;
            handle = ListenStateEnter(animator, stateNameHash, asi =>
            {
                handle.Unlisten();
                callback(asi);
            });

            return handle;
        }

        /// <summary>
        /// Register a callback to the event when the animator enters a state and invoke only once.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateName">The name of the state.</param>
        /// <param name="callback">The callback function.</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle ListenStateEnterOnce(this Animator animator, string stateName, Action<AnimatorStateInfo> callback)
        {
            var stateNameHash = Animator.StringToHash(stateName);
            return ListenStateEnterOnce(animator, stateNameHash, callback);
        }

        /// <summary>
        /// Register a callback to the event when the animator exits a state and invoke only once.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateNameHash">The hash of the state.</param>
        /// <param name="callback">The callback function.</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle ListenStateExitOnce(this Animator animator, int stateNameHash, Action<AnimatorStateInfo> callback)
        {
            EventHandle handle = default;
            handle = ListenStateExit(animator, stateNameHash, asi =>
            {
                handle.Unlisten();
                callback(asi);
            });

            return handle;
        }

        /// <summary>
        /// Register a callback to the event when the animator exits a state and invoke only once.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateName">The name of the state.</param>
        /// <param name="callback">The callback function.</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle ListenStateExitOnce(this Animator animator, string stateName, Action<AnimatorStateInfo> callback)
        {
            var stateNameHash = Animator.StringToHash(stateName);
            return ListenStateExitOnce(animator, stateNameHash, callback);
        }

        /// <summary>
        /// Remove all the listeners registered to the <see cref="Animator"/> ever.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        public static void RemoveAllEventListeners(this Animator animator)
        {
            if (animator.gameObject.TryGetComponent(out AnimatorEventBridge animatorEventBridge))
            {
                animatorEventBridge.onStateEnter.RemoveAllListeners();
                animatorEventBridge.onStateExit.RemoveAllListeners();
            }
        }

        /// <summary>
        /// Plays a state and invoke the callback on play end.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateName">The state name.</param>
        /// <param name="onEnd">The callback on play end.</param>
        /// <param name="layer">The layer index. If layer is -1, it plays the first state with the given state name or hash.</param>
        /// <param name="normalizedTime">The time offset between zero and one.</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle Play(this Animator animator, string stateName, Action onEnd, int layer = -1, float normalizedTime = float.NegativeInfinity)
        {
            return Play(animator, stateName, _ => onEnd(), layer, normalizedTime);
        }

        /// <summary>
        /// Plays a state and invoke the callback on play end.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateName">The state name.</param>
        /// <param name="onEnd">The callback on play end.</param>
        /// <param name="layer">The layer index. If layer is -1, it plays the first state with the given state name or hash.</param>
        /// <param name="normalizedTime">The time offset between zero and one.</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle Play(this Animator animator, string stateName, Action<AnimatorStateInfo> onEnd, int layer = -1, float normalizedTime = float.NegativeInfinity)
        {
            EventHandle handle = ListenStateExitOnce(animator, stateName, onEnd);
            animator.Play(stateName, layer, normalizedTime);
            return handle;
        }

        /// <summary>
        /// Plays a state and invoke the callback on play end.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateNameHash">The state hash name. If stateNameHash is 0, it changes the current state time.</param>
        /// <param name="onEnd">The callback on play end.</param>
        /// <param name="layer">The layer index. If layer is -1, it plays the first state with the given state name or hash.</param>
        /// <param name="normalizedTime">The time offset between zero and one.</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle Play(this Animator animator, int stateNameHash, Action onEnd, int layer = -1, float normalizedTime = float.NegativeInfinity)
        {
            return Play(animator, stateNameHash, _ => onEnd(), layer, normalizedTime);
        }

        /// <summary>
        /// Plays a state and invoke the callback on play end.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateNameHash">The state hash name. If stateNameHash is 0, it changes the current state time.</param>
        /// <param name="onEnd">The callback on play end.</param>
        /// <param name="layer">The layer index. If layer is -1, it plays the first state with the given state name or hash.</param>
        /// <param name="normalizedTime">The time offset between zero and one.</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle Play(this Animator animator, int stateNameHash, Action<AnimatorStateInfo> onEnd, int layer = -1, float normalizedTime = float.NegativeInfinity)
        {
            EventHandle handle = ListenStateExitOnce(animator, stateNameHash, onEnd);
            animator.Play(stateNameHash, layer, normalizedTime);
            return handle;
        }

        /// <summary>
        /// Plays a state and invoke the callback on play end.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateName">The state name.</param>
        /// <param name="onEnd">The callback on play end.</param>
        /// <param name="layer">The layer index. If layer is -1, it plays the first state with the given state name or hash.</param>
        /// <param name="fixedTime">The time offset (in seconds).</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle PlayInFixedTime(this Animator animator, string stateName, Action onEnd, int layer = -1, float fixedTime = float.NegativeInfinity)
        {
            return PlayInFixedTime(animator, stateName, _ => onEnd(), layer, fixedTime);
        }

        /// <summary>
        /// Plays a state and invoke the callback on play end.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateName">The state name.</param>
        /// <param name="onEnd">The callback on play end.</param>
        /// <param name="layer">The layer index. If layer is -1, it plays the first state with the given state name or hash.</param>
        /// <param name="fixedTime">The time offset (in seconds).</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle PlayInFixedTime(this Animator animator, string stateName, Action<AnimatorStateInfo> onEnd, int layer = -1, float fixedTime = float.NegativeInfinity)
        {
            EventHandle handle = ListenStateExitOnce(animator, stateName, onEnd);
            animator.PlayInFixedTime(stateName, layer, fixedTime);
            return handle;
        }

        /// <summary>
        /// Plays a state and invoke the callback on play end.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateNameHash">The state hash name. If stateNameHash is 0, it changes the current state time.</param>
        /// <param name="onEnd">The callback on play end.</param>
        /// <param name="layer">The layer index. If layer is -1, it plays the first state with the given state name or hash.</param>
        /// <param name="fixedTime">The time offset (in seconds).</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle PlayInFixedTime(this Animator animator, int stateNameHash, Action onEnd, int layer = -1, float fixedTime = float.NegativeInfinity)
        {
            return PlayInFixedTime(animator, stateNameHash, _ => onEnd(), layer, fixedTime);
        }

        /// <summary>
        /// Plays a state and invoke the callback on play end.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateNameHash">The state hash name. If stateNameHash is 0, it changes the current state time.</param>
        /// <param name="onEnd">The callback on play end.</param>
        /// <param name="layer">The layer index. If layer is -1, it plays the first state with the given state name or hash.</param>
        /// <param name="fixedTime">The time offset (in seconds).</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle PlayInFixedTime(this Animator animator, int stateNameHash, Action<AnimatorStateInfo> onEnd, int layer = -1, float fixedTime = float.NegativeInfinity)
        {
            EventHandle handle = ListenStateExitOnce(animator, stateNameHash, onEnd);
            animator.PlayInFixedTime(stateNameHash, layer, fixedTime);
            return handle;
        }

        /// <summary>
        /// Creates a crossfade from the current state to any other state using normalized times.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateNameHash">The hash name of the state.</param>
        /// <param name="normalizedTransitionDuration">The duration of the transition (normalized).</param>
        /// <param name="onEnd">The callback on play end.</param>
        /// <param name="layer">The layer where the crossfade occurs.</param>
        /// <param name="normalizedTimeOffset">The time of the state (normalized).</param>
        /// <param name="normalizedTransitionTime">The time of the transition (normalized).</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle CrossFade(this Animator animator, int stateNameHash, float normalizedTransitionDuration, Action onEnd, int layer = -1, float normalizedTimeOffset = 0f, float normalizedTransitionTime = 0f)
        {
            return CrossFade(animator, stateNameHash, normalizedTransitionDuration, _ => onEnd(), layer, normalizedTimeOffset, normalizedTransitionTime);
        }

        /// <summary>
        /// Creates a crossfade from the current state to any other state using normalized times.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateNameHash">The hash name of the state.</param>
        /// <param name="normalizedTransitionDuration">The duration of the transition (normalized).</param>
        /// <param name="onEnd">The callback on play end.</param>
        /// <param name="layer">The layer where the crossfade occurs.</param>
        /// <param name="normalizedTimeOffset">The time of the state (normalized).</param>
        /// <param name="normalizedTransitionTime">The time of the transition (normalized).</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle CrossFade(this Animator animator, int stateNameHash, float normalizedTransitionDuration, Action<AnimatorStateInfo> onEnd, int layer = -1, float normalizedTimeOffset = 0f, float normalizedTransitionTime = 0f)
        {
            EventHandle handle = ListenStateExitOnce(animator, stateNameHash, onEnd);
            animator.CrossFade(stateNameHash, normalizedTransitionDuration, layer, normalizedTimeOffset, normalizedTransitionTime);
            return handle;
        }

        /// <summary>
        /// Creates a crossfade from the current state to any other state using normalized times.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateName">The state name.</param>
        /// <param name="normalizedTransitionDuration">The duration of the transition (normalized).</param>
        /// <param name="onEnd">The callback on play end.</param>
        /// <param name="layer">The layer where the crossfade occurs.</param>
        /// <param name="normalizedTimeOffset">The time of the state (normalized).</param>
        /// <param name="normalizedTransitionTime">The time of the transition (normalized).</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle CrossFade(this Animator animator, string stateName, float normalizedTransitionDuration, Action onEnd, int layer = -1, float normalizedTimeOffset = 0f, float normalizedTransitionTime = 0f)
        {
            return CrossFade(animator, stateName, normalizedTransitionDuration, _ => onEnd(), layer, normalizedTimeOffset, normalizedTransitionTime);
        }

        /// <summary>
        /// Creates a crossfade from the current state to any other state using normalized times.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateName">The state name.</param>
        /// <param name="normalizedTransitionDuration">The duration of the transition (normalized).</param>
        /// <param name="onEnd">The callback on play end.</param>
        /// <param name="layer">The layer where the crossfade occurs.</param>
        /// <param name="normalizedTimeOffset">The time of the state (normalized).</param>
        /// <param name="normalizedTransitionTime">The time of the transition (normalized).</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle CrossFade(this Animator animator, string stateName, float normalizedTransitionDuration, Action<AnimatorStateInfo> onEnd, int layer = -1, float normalizedTimeOffset = 0f, float normalizedTransitionTime = 0f)
        {
            EventHandle handle = ListenStateExitOnce(animator, stateName, onEnd);
            animator.CrossFade(stateName, normalizedTransitionDuration, layer, normalizedTimeOffset, normalizedTransitionTime);
            return handle;
        }

        /// <summary>
        /// Creates a crossfade from the current state to any other state using times in seconds.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateNameHash">The hash name of the state.</param>
        /// <param name="fixedTransitionDuration">The duration of the transition (in seconds).</param>
        /// <param name="onEnd">The callback on play end.</param>
        /// <param name="layer">The layer where the crossfade occurs.</param>
        /// <param name="fixedTimeOffset">The time of the state (in seconds).</param>
        /// <param name="normalizedTransitionTime">The time of the transition (normalized).</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle CrossFadeInFixedTime(this Animator animator, int stateNameHash, float fixedTransitionDuration, Action onEnd, int layer = -1, float fixedTimeOffset = 0f, float normalizedTransitionTime = 0f)
        {
            return CrossFadeInFixedTime(animator, stateNameHash, fixedTransitionDuration, _ => onEnd(), layer, fixedTimeOffset, normalizedTransitionTime);
        }

        /// <summary>
        /// Creates a crossfade from the current state to any other state using times in seconds.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateNameHash">The hash name of the state.</param>
        /// <param name="fixedTransitionDuration">The duration of the transition (in seconds).</param>
        /// <param name="onEnd">The callback on play end.</param>
        /// <param name="layer">The layer where the crossfade occurs.</param>
        /// <param name="fixedTimeOffset">The time of the state (in seconds).</param>
        /// <param name="normalizedTransitionTime">The time of the transition (normalized).</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle CrossFadeInFixedTime(this Animator animator, int stateNameHash, float fixedTransitionDuration, Action<AnimatorStateInfo> onEnd, int layer = -1, float fixedTimeOffset = 0f, float normalizedTransitionTime = 0f)
        {
            EventHandle handle = ListenStateExitOnce(animator, stateNameHash, onEnd);
            animator.CrossFadeInFixedTime(stateNameHash, fixedTransitionDuration, layer, fixedTimeOffset, normalizedTransitionTime);
            return handle;
        }

        /// <summary>
        /// Creates a crossfade from the current state to any other state using times in seconds.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateName">The state name.</param>
        /// <param name="fixedTransitionDuration">The duration of the transition (in seconds).</param>
        /// <param name="onEnd">The callback on play end.</param>
        /// <param name="layer">The layer where the crossfade occurs.</param>
        /// <param name="fixedTimeOffset">The time of the state (in seconds).</param>
        /// <param name="normalizedTransitionTime">The time of the transition (normalized).</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle CrossFadeInFixedTime(this Animator animator, string stateName, float fixedTransitionDuration, Action onEnd, int layer = -1, float fixedTimeOffset = 0f, float normalizedTransitionTime = 0f)
        {
            return CrossFadeInFixedTime(animator, stateName, fixedTransitionDuration, _ => onEnd(), layer, fixedTimeOffset, normalizedTransitionTime);
        }

        /// <summary>
        /// Creates a crossfade from the current state to any other state using times in seconds.
        /// </summary>
        /// <param name="animator">The <see cref="Animator"/> instance.</param>
        /// <param name="stateName">The state name.</param>
        /// <param name="fixedTransitionDuration">The duration of the transition (in seconds).</param>
        /// <param name="onEnd">The callback on play end.</param>
        /// <param name="layer">The layer where the crossfade occurs.</param>
        /// <param name="fixedTimeOffset">The time of the state (in seconds).</param>
        /// <param name="normalizedTransitionTime">The time of the transition (normalized).</param>
        /// <returns>The <see cref="EventHandle"/> for this callback.</returns>
        public static EventHandle CrossFadeInFixedTime(this Animator animator, string stateName, float fixedTransitionDuration, Action<AnimatorStateInfo> onEnd, int layer = -1, float fixedTimeOffset = 0f, float normalizedTransitionTime = 0f)
        {
            EventHandle handle = ListenStateExitOnce(animator, stateName, onEnd);
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
