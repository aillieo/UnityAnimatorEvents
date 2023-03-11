// -----------------------------------------------------------------------
// <copyright file="AnimatorEventBridge.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.UnityAnimatorEvents
{
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    internal class AnimatorEventBridge : MonoBehaviour
    {
        internal Event<int> onStateEnter = new Event<int>();
        internal Event<int> onStateExit = new Event<int>();

        private void OnDestroy()
        {
            this.onStateEnter.RemoveAllListeners();
            this.onStateExit.RemoveAllListeners();
        }
    }
}
