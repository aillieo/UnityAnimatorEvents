using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AillieoUtils
{
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    internal class AnimatorEventBridge : MonoBehaviour
    {
        internal Event<int> onStateEnter = new Event<int>();
        internal Event<int> onStateExit = new Event<int>();

        private void OnDestroy()
        {
            onStateEnter.RemoveAllListeners();
            onStateExit.RemoveAllListeners();
        }
    }
}
