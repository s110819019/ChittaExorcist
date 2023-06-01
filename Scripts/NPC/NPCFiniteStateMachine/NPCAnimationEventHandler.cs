using System;
using UnityEngine;

namespace ChittaExorcist.NPCSettings.FSM
{
    public class NPCAnimationEventHandler : MonoBehaviour
    {
        #region w/ Events

        // public event Action OnFinish;

        public event Action OnFinishReact;

        #endregion
        
        // private void AnimationFinishTrigger() => OnFinish?.Invoke();
        private void AnimationFinishReactTrigger() => OnFinishReact?.Invoke();
    }
}