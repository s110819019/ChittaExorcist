using System;
using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class EnemyAnimationEventHandler : MonoBehaviour
    {
        // #region w/ Attack
        //
        // public E_AttackState<Enemy, EnemyStateDataSo> AttackState;
        //
        // private void TriggerAttack()
        // {
        //     AttackState.TriggerAttack();
        // }
        //
        // private void FinishAttack()
        // {
        //     AttackState.FinishAttack();
        // }
        //
        // private void TriggerAttackEffect()
        // {
        //     AttackState.TriggerAttackEffect();
        // }    
        //
        // #endregion

        #region w/ Events

        public event Action OnFinish;

        public event Action OnFinishAnim;

        public event Action OnMeleeAttack;

        public event Action OnRangedAttack;
        
        // public event Action<int> OnSetParryWindow;

        public event Action OnStartParryWindows;

        public event Action OnStopParryWindows;

        public event Action OnStartAttackReact;

        #endregion
        
        private void AnimationFinishTrigger() => OnFinish?.Invoke();
        private void AnimationFinishAnimTrigger() => OnFinishAnim?.Invoke();
        private void ActionMeleeAttackTrigger() => OnMeleeAttack?.Invoke();
        private void ActionRangedAttackTrigger() => OnRangedAttack?.Invoke();

        // private void SetParryWindows(int value) => OnSetParryWindow?.Invoke(value);

        private void StartParryWindowsTrigger() => OnStartParryWindows?.Invoke();
        private void StopParryWindowsTrigger() => OnStopParryWindows?.Invoke();

        private void StartAttackReact() => OnStartAttackReact?.Invoke();
    }
}