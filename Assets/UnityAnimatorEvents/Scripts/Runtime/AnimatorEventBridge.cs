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
        internal EasyDelegate<AnimatorStateInfo> onStateEnter = new EasyDelegate<AnimatorStateInfo>();
        internal EasyDelegate<AnimatorStateInfo> onStateExit = new EasyDelegate<AnimatorStateInfo>();

        private void OnDestroy()
        {
            this.onStateEnter.RemoveAllListeners();
            this.onStateExit.RemoveAllListeners();
        }
    }
}
