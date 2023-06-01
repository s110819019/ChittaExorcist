using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    public class AnimationEventHandler : MonoBehaviour
    {
        // IsAbilityDone
        public event Action OnFinish;
        
        // IsAbilityEarlyDone
        public event Action OnEarlyOut;
        
        // Movement
        public event Action OnStartMovement;
        public event Action OnStopMovement;
        
        // Action Hit Box
        public event Action OnActionHitBox;
        
        // Action Effect
        public event Action OnActionEffect;
        
        // Gravity
        public event Action OnSetGravity;
        public event Action OnResetGravity;
        
        // Block
        public event Action OnStartBlock;
        public event Action OnStopBlock;
        
        // Parry
        public event Action OnStartParry;
        public event Action OnStopParry;

        // Ranged Attack
        public event Action OnRanged;

        private void AnimationFinishTrigger() => OnFinish?.Invoke();
        // private void AnimationFinishTrigger()
        // {
        //     // Debug.Log("EventHandler Call Finish");
        //     OnFinish?.Invoke();
        // }
        private void AnimationEarlyOutTrigger() => OnEarlyOut?.Invoke();
        private void StartMovementTrigger() => OnStartMovement?.Invoke();
        private void StopMovementTrigger() => OnStopMovement?.Invoke();
        private void ActionHitBoxTrigger() => OnActionHitBox?.Invoke();
        private void ActionEffectTrigger() => OnActionEffect?.Invoke();
        private void SetGravityTrigger() => OnSetGravity?.Invoke();
        private void ResetGravityTrigger() => OnResetGravity?.Invoke();
        private void StartBlockTrigger() => OnStartBlock?.Invoke();
        private void StopBlockTrigger() => OnStopBlock?.Invoke();
        private void StartParryTrigger() => OnStartParry?.Invoke();
        private void StopParryTrigger() => OnStopParry?.Invoke();
        private void ActionRangedTrigger() => OnRanged?.Invoke();
    }
}