using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E1_MeleeAttackState : E_MeleeAttackState<Enemy1>
    {
        public E1_MeleeAttackState(string animationBoolName, Enemy1 enemy, ED_MeleeAttackStateSO stateData, Transform attackPosition) : base(animationBoolName, enemy, stateData, attackPosition)
        {
            // 1. Player Detected
            // 2. Look for Player
        }
        
        #region w/ State Workflow

        public override void Enter()
        {
            base.Enter();
            AudioManager.Instance.PlayOnceAudio(StateData.audioData);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (ShouldReactAttackParried)
            {
                KnockbackReceiver.Knockback(Enemy.ParriedDetails.ParriedKnockbackDetails.KnockbackAngle,
                    Enemy.ParriedDetails.ParriedKnockbackDetails.KnockbackStrength,
                    Enemy.ParriedDetails.ParriedKnockbackDetails.KnockbackDirection);
                StateMachine.ChangeState(Enemy.IdleState);
                return;
            }
            
            if (!IsAnimationFinished) return;
        
            if (IsPlayerInMinAggroRange)
            {
                // PlayerDetected
                StateMachine.ChangeState(Enemy.PlayerDetectedState);
            }
            else
            {
                // LookForPlayer
                StateMachine.ChangeState(Enemy.LookForPlayerState);
            }
        }    

        #endregion
    }
}