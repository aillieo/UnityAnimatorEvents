// -----------------------------------------------------------------------
// <copyright file="AnimatorEventDispatcher.cs" company="AillieoTech">
// Copyright (c) AillieoTech. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace AillieoUtils.UnityAnimatorEvents
{
    using UnityEngine;

    internal class AnimatorEventDispatcher : StateMachineBehaviour
    {
        private AnimatorEventBridge animatorEventBridge;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            if (this.animatorEventBridge == null)
            {
                this.animatorEventBridge = animator.gameObject.GetComponent<AnimatorEventBridge>();
            }

            if (this.animatorEventBridge != null)
            {
                this.animatorEventBridge.onStateEnter.Invoke(stateInfo);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (this.animatorEventBridge == null)
            {
                this.animatorEventBridge = animator.gameObject.GetComponent<AnimatorEventBridge>();
            }

            if (this.animatorEventBridge != null)
            {
                this.animatorEventBridge.onStateExit.Invoke(stateInfo);
            }

            base.OnStateExit(animator, stateInfo, layerIndex);
        }
    }
}
