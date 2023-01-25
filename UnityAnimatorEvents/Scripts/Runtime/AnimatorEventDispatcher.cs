using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils.UnityAnimatorEvents
{
    internal class AnimatorEventDispatcher : StateMachineBehaviour
    {
        private AnimatorEventBridge animatorEventBridge;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            if (animatorEventBridge == null)
            {
                animatorEventBridge = animator.gameObject.GetComponent<AnimatorEventBridge>();
            }

            if (animatorEventBridge != null)
            {
                animatorEventBridge.onStateEnter.Invoke(stateInfo.shortNameHash);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animatorEventBridge == null)
            {
                animatorEventBridge = animator.gameObject.GetComponent<AnimatorEventBridge>();
            }

            if (animatorEventBridge != null)
            {
                animatorEventBridge.onStateExit.Invoke(stateInfo.shortNameHash);
            }

            base.OnStateExit(animator, stateInfo, layerIndex);
        }
    }
}
