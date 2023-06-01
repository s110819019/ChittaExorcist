using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E2_MeleeAttackState : E_MeleeAttackState<Enemy2>
    {
        public E2_MeleeAttackState(string animationBoolName, Enemy2 enemy, ED_MeleeAttackStateSO stateData, Transform attackPosition) : base(animationBoolName, enemy, stateData, attackPosition)
        {
            // 1. PlayerDetected
            // 2. LookForPlayer
        }

        #region w/ State Workflow

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (ShouldReactAttackParried)
            {
                KnockbackReceiver.Knockback(Enemy.ParriedDetails.ParriedKnockbackDetails.KnockbackAngle,
                    Enemy.ParriedDetails.ParriedKnockbackDetails.KnockbackStrength,
                    Enemy.ParriedDetails.ParriedKnockbackDetails.KnockbackDirection);
                StateMachine.ChangeState(Enemy.StunState);
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