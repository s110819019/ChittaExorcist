using UnityEngine;

namespace ChittaExorcist.EnemySettings.FSM
{
    public class E3_MeleeAttackState : E_MeleeAttackState<Enemy3>
    {
        public E3_MeleeAttackState(string animationBoolName, Enemy3 enemy, ED_MeleeAttackStateSO stateData, Transform attackPosition) : base(animationBoolName, enemy, stateData, attackPosition)
        {
        }
        
        #region w/ State Workflow

        // public override void Enter()
        // {
        //     base.Enter();
        //     Enemy.ChargeState.SetHasAttack();
        // }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (ShouldReactAttackParried)
            {
                KnockbackReceiver.Knockback(Enemy.ParriedDetails.ParriedKnockbackDetails.KnockbackAngle,
                    Enemy.ParriedDetails.ParriedKnockbackDetails.KnockbackStrength,
                    Enemy.ParriedDetails.ParriedKnockbackDetails.KnockbackDirection);
                StateMachine.ChangeState(Enemy.IdleState);
                // Debug.Log("Should To Idle");
                return;
            }
            
            if (!IsAnimationFinished) return;
        
            // if (IsPlayerInMinAggroRange)
            // {
            //     // PlayerDetected
            //     StateMachine.ChangeState(_enemy.PlayerDetectedState);
            // }
            // else
            // {
            //     // LookForPlayer
            //     StateMachine.ChangeState(_enemy.LookForPlayerState);
            // }
        
            StateMachine.ChangeState(Enemy.ChargeState);
        }    

        #endregion
    }
}